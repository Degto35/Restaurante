namespace Restaurante.Web.DTOs.Users
{
    public class CrearUsuarioDTO
    {
        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Id_Rol { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
