# Integration test for the REST API (development only: it uses the seeded users).
# The application must be running. From the folder that contains this file:
#   .\test-api.ps1 -ControllersPath .\Controllers
#
# How permissions are checked without creating data:
# authorization runs BEFORE the body is validated, so a user WITHOUT permission gets 403,
# while a user WITH permission gets another code (200 for a list, 400 for an empty body
# or an id that does not exist).
#
# Assumed roles (default seeder): Mesero = orders.show/create/update (no orders.pay),
# Cajero = orders.show and orders.pay only.

param(
    [string]$BaseUrl = "https://localhost:7232",
    [string]$ControllersPath = ".\Restaurante.Web\Controllers"
)

$adminEmail   = "admin@restaurante.com"
$waiterEmail  = "laura@restaurante.com"
$cashierEmail = "pedro@restaurante.com"
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

# One request for each of the four identities.
# $Rules = expected result for: none, admin, waiter, cashier (use "-" to skip one).
function Test-Endpoint([string]$Label, [string]$Method, [string]$Path, [string]$Body, [string[]]$Rules) {
    $tokens = @($null, $script:adminToken, $script:waiterToken, $script:cashierToken)
    $who    = @("sin token", "admin", "mesero", "cajero")

    for ($i = 0; $i -lt 4; $i++) {
        if ($Rules[$i] -eq "-") { continue }
        $code = Invoke-Api $Method $Path $tokens[$i] $Body
        Test-Case ("{0,-34} {1}" -f $Label, $who[$i]) $code $Rules[$i]
    }
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

$script:adminToken   = Get-Token $adminEmail $password
$script:waiterToken  = Get-Token $waiterEmail $password
$script:cashierToken = Get-Token $cashierEmail $password

if (-not $script:adminToken -or -not $script:waiterToken -or -not $script:cashierToken) {
    Write-Host "No se pudo obtener el token (admin, mesero o cajero). Revisa los usuarios y la contrasena." -ForegroundColor Red
    exit 1
}

Write-Host "  Tokens obtenidos para admin, mesero y cajero."

$id = "00000000-0000-0000-0000-000000000001"

# ---------------------------------------------------------------------------
Write-Host "`n== Cuenta y autenticacion ==" -ForegroundColor Cyan
Test-Case "GET api/health (sin token)"               (Invoke-Api "GET" "/api/health" $null $null) "allowed"
Test-Case "GET api/account/profile (sin token)"      (Invoke-Api "GET" "/api/account/profile" $null $null) "401"
Test-Case "GET api/account/profile (admin)"          (Invoke-Api "GET" "/api/account/profile" $script:adminToken $null) "allowed"
Test-Case "GET api/account/profile (cajero)"         (Invoke-Api "GET" "/api/account/profile" $script:cashierToken $null) "allowed"
Test-Case "GET api/account/profile (token alterado)" (Invoke-Api "GET" "/api/account/profile" ($script:adminToken + "x") $null) "401"

# ---------------------------------------------------------------------------
# Categories, dishes and tables: the waiter has only the .show permission here,
# the cashier has none.
foreach ($r in @("categories", "dishes", "diningtables")) {
    Write-Host "`n== api/$r ==" -ForegroundColor Cyan

    #                 label                  method   path             body   none   admin      waiter     cashier
    Test-Endpoint "GET    lista"             "GET"    "/api/$r"        $null  @("401", "allowed", "allowed", "403")
    Test-Endpoint "GET    por id"            "GET"    "/api/$r/$id"    $null  @("401", "allowed", "allowed", "403")
    Test-Endpoint "POST   crear (vacio)"     "POST"   "/api/$r"        "{}"   @("401", "allowed", "-",       "403")
    Test-Endpoint "PUT    editar (vacio)"    "PUT"    "/api/$r/$id"    "{}"   @("401", "allowed", "-",       "403")
    Test-Endpoint "DELETE eliminar (id falso)" "DELETE" "/api/$r/$id"  $null  @("401", "allowed", "-",       "403")
}

Write-Host "`n== api/diningtables (disponibilidad) ==" -ForegroundColor Cyan
Test-Endpoint "PATCH  disponibilidad"        "PATCH"  "/api/diningtables/$id/availability" $null @("401", "allowed", "-", "403")

# ---------------------------------------------------------------------------
# Orders: waiter = show/create/update (no pay); cashier = show/pay only.
Write-Host "`n== api/orders ==" -ForegroundColor Cyan

#                 label                      method   path                           body   none   admin      waiter     cashier
Test-Endpoint "GET    lista"                 "GET"    "/api/orders"                  $null  @("401", "allowed", "allowed", "allowed")
Test-Endpoint "GET    por id"                "GET"    "/api/orders/$id"              $null  @("401", "allowed", "allowed", "allowed")
Test-Endpoint "POST   crear pedido"          "POST"   "/api/orders"                  "{}"   @("401", "allowed", "allowed", "403")
Test-Endpoint "PUT    cambiar estado"        "PUT"    "/api/orders/status"           "{}"   @("401", "allowed", "allowed", "403")
Test-Endpoint "POST   agregar plato"         "POST"   "/api/orders/details"          "{}"   @("401", "allowed", "allowed", "403")
Test-Endpoint "DELETE quitar plato"          "DELETE" "/api/orders/details/$id"      $null  @("401", "allowed", "allowed", "403")
Test-Endpoint "POST   cobrar (orders.pay)"   "POST"   "/api/orders/pay"              "{}"   @("401", "allowed", "403",     "allowed")

Write-Host "`n== Resultado ==" -ForegroundColor Cyan
Write-Host ("  Pasaron: {0}   Fallaron: {1}" -f $script:passed, $script:failed)

if ($script:failed -gt 0) { exit 1 }
