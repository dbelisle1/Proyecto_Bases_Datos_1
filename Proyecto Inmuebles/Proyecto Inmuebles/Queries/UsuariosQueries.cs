namespace Proyecto_Inmuebles.Queries
{
    public static class UsuariosQueries
    {
        public static string SelectUserPassQuery()
        {
            return $@"SELECT * FROM USUARIOS WHERE NOMBREUSUARIO = :username AND CONTRASENA = :password";
        }
        /// <summary>
        /// :IdTipoUsuario, :NombreUsuario, :Contrasena, :Correo
        /// </summary>
        /// <returns></returns>
        public static string InsertUsuarioQuery()
        {
            return $@"INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo)
                        VALUES (:IdTipoUsuario, :NombreUsuario, :Contrasena, :Correo) RETURNING IdUsuario INTO :IdUsuario";
        }

        public static string SelectUsuariosQuery()
        {
            return "SELECT * FROM Usuarios";
        }

        public static string SelectUsuarioFiltroQuery()
        {
            return "SELECT * FROM Usuarios WHERE IdUsuario = :IdUsuario";
        }

        public static string UpdateUsuarioQuery()
        {
            return "UPDATE Usuarios SET IdTipoUsuario = :IdTipoUsuario, NombreUsuario = :NombreUsuario, Contrasena = :Contrasena, Correo = :Correo " +
                   "WHERE IdUsuario = :IdUsuario";
        }

        public static string DeleteUsuarioQuery()
        {
            return "UPDATE Usuarios SET Eliminado = 1 WHERE IdUsuario = :IdUsuario";
        }
    }
}
