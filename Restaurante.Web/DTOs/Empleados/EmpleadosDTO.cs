namespace Restaurante.Web.DTOs.Empleados
{
    public class EmpleadoDTO
    {
        public Guid IdEmpleado { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public Guid IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }
}