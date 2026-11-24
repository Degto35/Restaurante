using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Users
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;

        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Documento")]
        public string Document { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        public string? PhoneNumber { get; set; }

        public Guid RoleId { get; set; }

        [Display(Name = "Rol")]
        public string RoleName { get; set; } = string.Empty;
    }
}