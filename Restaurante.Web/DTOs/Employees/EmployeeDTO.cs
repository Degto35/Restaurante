using System.ComponentModel.DataAnnotations;

namespace Restaurante.Web.DTOs.Employees
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        public string? Telephone { get; set; }

        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        public Guid RoleId { get; set; }

        [Display(Name = "Rol")]
        public string RoleName { get; set; } = string.Empty;
    }
}