namespace Restaurante.Web.DTOs.Account
{
    public class UserTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}