namespace Restaurante.Web.Models
{
    public class Cuenta
    {
        public int Id_Cuenta { get; set; }

        public int Id_Usuario { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool Activa { get; set; } = true;

        public Usuario? Usuario { get; set; }
    }
}
