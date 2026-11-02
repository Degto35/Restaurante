namespace Restaurante.Web.DTOs.Employees
{
    public class EmployeesDTO
    {
        public Guid IdEmployee { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public Guid IdRole { get; set; }
        public string NameRole { get; set; } = string.Empty;
    }
}