# Integration test for the REST API (development only: it uses the seeded users).
# The application must be running. From the folder that contains this file:
#   .\test-api.ps1
#   .\test-api.ps1 -BaseUrl https://localhost:7232 -ControllersPath .\Restaurante.Web\Controllers
#
# How permissions are checked without creating data:
# authorization runs BEFORE the body is validated, so a user WITHOUT permission gets 403,
# while a user WITH permission gets another code (200 for a list, 400 for an empty body).

param(
    [string]$BaseUrl = "https://localhost:7232",
    [string]$ControllersPath = ".\Restaurante.Web\Controllers"
)

$adminEmail   = "admin@restaurante.com"
$waiterEmail  = "laura@restaurante.com"   # role Mesero: has the *.show permissions
$cashierEmail = "pedro@restaurante.com"   # role Cajero: only orders.show and orders.pay
$password     = "1234"                    # development password of the seeded users

$script:passed = 0
$script:failed = 0

function Report([string]$Name, [bool]$Ok, [string]$Detail) {
    if ($Ok) {
        $script:passed++
        Write-Host ("  OK    {0}  ->  {1}" -f $Name, $Detail) -ForegroundColor Green
    }
    else {
        $script:failed++
        Write-Host ("  FALLA {0}  ->  {1}" -f $Name, $Detail) -ForegroundColor Red
    }
}

function Write-Json([string]$Content) {
    $file = [System.IO.Path]::GetTempFileName()
    [System.IO.File]::WriteAllText($file, $Content)
    return $file
}

function Get-Token([string]$Email, [string]$Pass) {
    $file = Write-Json ('{"email":"' + $Email + '","password":"' + $Pass + '"}')
    $json = & curl.exe -k -s -X POST "$BaseUrl/api/Auth/login" -H "Content-Type: application/json" --data-binary "@$file"
    Remove-Item $file -ErrorAction SilentlyContinue

    try { return ($json | ConvertFrom-Json).result.token }
    catch { return $null }
}

function Invoke-Api([string]$Method, [string]$Path, [string]$Token, [string]$Body) {
    $curlArgs = @("-k", "-s", "-o", "NUL", "-w", "%{http_code}", "-X", $Method, "$BaseUrl$Path")
    $file = $null

    if ($Token) { $curlArgs += @("-H", "Authorization: Bearer $Token") }

    if ($Body) {
        $file = Write-Json $Body
        $curlArgs += @("-H", "Content-Type: application/json", "--data-binary", "@$file")
    }

    $code = & curl.exe @curlArgs
    if ($file) { Remove-Item $file -ErrorAction SilentlyContinue }

    return [int]$code
}

# Rule "allowed": the request passed authentication and authorization
# (2xx, or 400/409/422 because the data is empty or does not exist).
function Test-Case([string]$Name, [int]$Actual, [string]$Rule) {
    switch ($Rule) {
        "401"     { $ok = ($Actual -eq 401) }
        "403"     { $ok = ($Actual -eq 403) }
        "allowed" { $ok = (($Actual -ge 200 -and $Actual -lt 300) -or ($Actual -in 400, 409, 422)) }
        default   { $ok = $false }
    }

    $detail = if ($ok) { "$Actual" } else { "$Actual (esperado: $Rule)" }
    Report $Name $ok $detail
}

# ---------------------------------------------------------------------------
Write-Host "`n== Codigos de permiso usados en los controladores ==" -ForegroundColor Cyan

# Admin skips every permission check, so a typo in a code (for example "table.create")
# is invisible to it. This check catches those typos by reading the source files.
$validCodes = @()
foreach ($module in "categories", "dishes", "tables", "users", "roles") {
    foreach ($action in "show", "create", "update", "delete") { $validCodes += "$module.$action" }
}
$validCodes += "orders.show", "orders.create", "orders.update", "orders.pay"

if (Test-Path $ControllersPath) {
    $hits = Get-ChildItem -Path $ControllersPath -Recurse -Filter *.cs |
            Select-String -Pattern 'CustomAuthorize\("([^"]+)"\)' -AllMatches
    $checked = 0

    foreach ($hit in $hits) {
        foreach ($m in $hit.Matches) {
            $code = $m.Groups[1].Value
            $checked++

            if ($validCodes -notcontains $code) {
                Report ("{0}:{1}" -f $hit.Filename, $hit.LineNumber) $false "codigo desconocido '$code'"
            }
        }
    }

    if ($checked -eq 0) {
        Report "Controladores" $false "no se encontro ningun CustomAuthorize en $ControllersPath"
    }
    else {
        Report "Codigos revisados" $true "$checked usos, todos existen en el seeder"
    }
}
else {
    Write-Host "  (se omite: no existe la carpeta $ControllersPath; usa -ControllersPath)" -ForegroundColor Yellow
}

# ---------------------------------------------------------------------------
Write-Host "`n== Preparacion ==" -ForegroundColor Cyan

$health = Invoke-Api "GET" "/api/health" $null $null
if ($health -ne 200) {
    Write-Host "La API no responde en $BaseUrl (codigo $health). Ejecuta la aplicacion primero." -ForegroundColor Red
    exit 1
}

$adminToken   = Get-Token $adminEmail $password
$waiterToken  = Get-Token $waiterEmail $password
$cashierToken = Get-Token $cashierEmail $password

if (-not $adminToken -or -not $waiterToken -or -not $cashierToken) {
    Write-Host "No se pudo obtener el token (admin, mesero o cajero). Revisa los usuarios y la contrasena." -ForegroundColor Red
    exit 1
}

Write-Host "  Tokens obtenidos para admin, mesero y cajero."

$fakeId = "00000000-0000-0000-0000-000000000001"
$resources = @("categories", "dishes", "diningtables", "orders")

Write-Host "`n== Cuenta y autenticacion ==" -ForegroundColor Cyan
Test-Case "GET api/health (sin token)"             (Invoke-Api "GET" "/api/health" $null $null) "allowed"
Test-Case "GET api/account/profile (sin token)"    (Invoke-Api "GET" "/api/account/profile" $null $null) "401"
Test-Case "GET api/account/profile (admin)"        (Invoke-Api "GET" "/api/account/profile" $adminToken $null) "allowed"
Test-Case "GET api/account/profile (cajero)"       (Invoke-Api "GET" "/api/account/profile" $cashierToken $null) "allowed"
Test-Case "GET api/account/profile (token alterado)" (Invoke-Api "GET" "/api/account/profile" ($adminToken + "x") $null) "401"

foreach ($r in $resources) {
    Write-Host "`n== api/$r ==" -ForegroundColor Cyan

    # No token: always 401
    Test-Case "GET    sin token"            (Invoke-Api "GET"    "/api/$r" $null $null) "401"
    Test-Case "POST   sin token"            (Invoke-Api "POST"   "/api/$r" $null "{}") "401"
    Test-Case "DELETE sin token"            (Invoke-Api "DELETE" "/api/$r/$fakeId" $null $null) "401"

    # Admin: passes everything
    Test-Case "GET    admin"                (Invoke-Api "GET"    "/api/$r" $adminToken $null) "allowed"
    Test-Case "POST   admin (cuerpo vacio)" (Invoke-Api "POST"   "/api/$r" $adminToken "{}") "allowed"
    Test-Case "PUT    admin (cuerpo vacio)" (Invoke-Api "PUT"    "/api/$r" $adminToken "{}") "allowed"
    Test-Case "DELETE admin (id falso)"     (Invoke-Api "DELETE" "/api/$r/$fakeId" $adminToken $null) "allowed"

    # Waiter: has the .show permission of all four modules
    Test-Case "GET    mesero (tiene .show)" (Invoke-Api "GET" "/api/$r" $waiterToken $null) "allowed"

    # Cashier: only orders.show and orders.pay
    if ($r -eq "orders") {
        Test-Case "GET    cajero (orders.show)" (Invoke-Api "GET" "/api/$r" $cashierToken $null) "allowed"
    }
    else {
        Test-Case "GET    cajero (sin permiso)" (Invoke-Api "GET" "/api/$r" $cashierToken $null) "403"
    }

    Test-Case "POST   cajero (sin permiso)" (Invoke-Api "POST"   "/api/$r" $cashierToken "{}") "403"
    Test-Case "PUT    cajero (sin permiso)" (Invoke-Api "PUT"    "/api/$r" $cashierToken "{}") "403"
    Test-Case "DELETE cajero (sin permiso)" (Invoke-Api "DELETE" "/api/$r/$fakeId" $cashierToken $null) "403"
}

Write-Host "`n== Resultado ==" -ForegroundColor Cyan
Write-Host ("  Pasaron: {0}   Fallaron: {1}" -f $script:passed, $script:failed)

if ($script:failed -gt 0) { exit 1 }
