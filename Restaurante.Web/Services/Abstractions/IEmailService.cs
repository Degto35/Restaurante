namespace Restaurante.Web.Services.Abstractions
{
    public interface IEmailService
    {
        
        Task<bool> SendAsync(string toEmail, string subject, string htmlBody);
    }
}