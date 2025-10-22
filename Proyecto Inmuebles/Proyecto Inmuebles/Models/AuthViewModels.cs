namespace Proyecto_Inmuebles.Models
{
    public class LoginViewModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? UserTypeRegister { get; set; }
    }

    public class RegisterViewModel
    {
        public int? IdTipoUsuario { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public string? Correo { get; set; }

        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Direccion { get; set; }

        // Agente

        public string Codigo { get; set; } = null!;

        // Vendedor

        // Comprador

        public string? Telefono { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Nacionalidad { get; set; }
        public int? Edad { get; set; }

      
       
    }

    public class NavItem
    {
        public string Text { get; set; } = "";
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";
    }
}
