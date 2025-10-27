using Proyecto_Inmuebles.Models;

namespace Proyecto_Inmuebles.Services
{
    public static class NavbarService
    {
        public static List<NavItem> fillNavbar(int userType)
        {
            List<NavItem> navbarItems = new List<NavItem>();

            navbarItems.Add(new NavItem { Text = "Home", Controller = "Home", Action = "Index" });
            navbarItems.Add(new NavItem { Text = "Perfil", Controller = "Perfil", Action = "Index" });

            if ( userType == 99)
            {
                navbarItems.Add(new NavItem { Text = "Reportes", Controller = "Reportes", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "TipoUsuario", Controller = "TipoUsuario", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "TiposInmueble", Controller = "TiposInmueble", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "FormasPago", Controller = "FormasPago", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "EstadoOferta", Controller = "EstadoOferta", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Condicion", Controller = "Condicion", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Usuarios", Controller = "Usuarios", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Compradores", Controller = "Compradores", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Vendedores", Controller = "Vendedores", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Agentes", Controller = "Agentes", Action = "Index" });

            }
            //Agente
            if (userType == 1)
            {
                navbarItems.Add(new NavItem { Text = "Ventas", Controller = "Ventas", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Publicaciones", Controller = "Publicaciones", Action = "Index" });

            }

            //Vendedor
            if (userType == 2)
            {
                navbarItems.Add(new NavItem { Text = "Inmuebles", Controller = "Inmuebles", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Publicaciones", Controller = "Publicaciones", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Ofertas", Controller = "Ofertas", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Contraofertas", Controller = "Contraofertas", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Notificaciones", Controller = "Notificaciones", Action = "Index" });
            }

            //Comprador
            if (userType == 3)
            {
                navbarItems.Add(new NavItem { Text = "Publicaciones", Controller = "Publicaciones", Action = "PublicacionesComprador" });
                navbarItems.Add(new NavItem { Text = "Prestamos", Controller = "Prestamos", Action = "Index" });
                navbarItems.Add(new NavItem { Text = "Notificaciones", Controller = "Notificaciones", Action = "Index" });

            }

            return navbarItems;
        }
    }
}
