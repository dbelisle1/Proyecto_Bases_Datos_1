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
    }
}
