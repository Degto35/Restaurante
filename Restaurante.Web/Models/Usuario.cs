namespace Restaurante.Web.Models
{
    public class Usuario
    {
        public int Id_Usuario { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Id_Rol { get; set; }

        public bool Activo { get; set; } = true;

        public Rol? Rol { get; set; }

        public Cuenta? Cuenta { get; set; }
    }
}
