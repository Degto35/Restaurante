namespace Restaurante.Web.DTOs.Users
{
    public class ActualizarUsuarioDTO
    {
        public int Id_Usuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Id_Rol { get; set; }

        public bool Activo { get; set; }

        public string? Password { get; set; }
    }
}
