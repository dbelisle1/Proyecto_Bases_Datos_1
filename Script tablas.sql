 USE BDDPY1;
GO 

DECLARE @tables TABLE (name SYSNAME);
INSERT INTO @tables(name) VALUES
('Prestamos'),
('Ventas'),
('Contraofertas'),
('Ofertas'),
('Notificaciones'),
('Publicaciones'),
('Inmuebles'),
('FormasPago'),
('EstadoOferta'),
('TiposInmueble'),
('Agentes'),
('Vendedores'),
('Compradores'),
('Usuarios'),
('TipoUsuario'),
('InmuebleCondicion'),
('Condicion');

-- Quitar todas las FKs (agnóstico al nombre)
DECLARE @sql NVARCHAR(MAX) = N'';
;WITH fk AS (
  SELECT fk.name AS fk_name, s.name AS fk_table_schema, t.name AS fk_table
  FROM sys.foreign_keys fk
  JOIN sys.tables t ON t.object_id = fk.parent_object_id
  JOIN sys.schemas s ON s.schema_id = t.schema_id
  WHERE s.name='dbo'
    AND (t.name IN (SELECT name FROM @tables)
     OR OBJECT_NAME(fk.referenced_object_id) IN (SELECT name FROM @tables))
)
SELECT @sql = @sql + N'ALTER TABLE ['+fk_table_schema+N'].['+fk_table+N'] DROP CONSTRAINT ['+fk_name+N'];'+CHAR(13)
FROM fk;
IF (@sql<>N'') EXEC sp_executesql @sql;

-- Drop tables (SQL Server 2016+)
DROP TABLE IF EXISTS dbo.Ventas;
DROP TABLE IF EXISTS dbo.Contraofertas;
DROP TABLE IF EXISTS dbo.Ofertas;
DROP TABLE IF EXISTS dbo.Notificaciones;

DROP TABLE IF EXISTS dbo.Publicaciones;

DROP TABLE IF EXISTS dbo.Inmuebles;

DROP TABLE IF EXISTS dbo.FormasPago;
DROP TABLE IF EXISTS dbo.EstadoOferta;
DROP TABLE IF EXISTS dbo.TiposInmueble;
DROP TABLE IF EXISTS dbo.Agentes;
DROP TABLE IF EXISTS dbo.Vendedores;
DROP TABLE IF EXISTS dbo.Compradores;
DROP TABLE IF EXISTS dbo.Usuarios;
DROP TABLE IF EXISTS dbo.TipoUsuario;
DROP TABLE IF EXISTS dbo.InmuebleCondicion;
DROP TABLE IF EXISTS dbo.Condicion;

-- Asegurar limpieza si existían tablas previas con otros nombres
DROP TABLE IF EXISTS dbo.Prestamos;
GO

-- =========================
-- CATÁLOGOS
-- =========================
CREATE TABLE dbo.TipoUsuario (
  IdTipoUsuario INT IDENTITY(1,1) PRIMARY KEY,
  NombreTipo NVARCHAR(50) NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.TiposInmueble (
  IdTipoInmueble INT IDENTITY(1,1) PRIMARY KEY,
  NombreTipo NVARCHAR(50) NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.FormasPago (
  IdFormaPago INT IDENTITY(1,1) PRIMARY KEY,
  NombreFormaPago NVARCHAR(50) NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.EstadoOferta (
  IdEstadoOferta INT IDENTITY(1,1) PRIMARY KEY,
  NombreEstado NVARCHAR(20) NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0
);

CREATE TABLE dbo.Condicion (
  IdCondicion INT IDENTITY(1,1) PRIMARY KEY,
  NombreCondicion NVARCHAR(100) NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0
);
-- =========================
-- USUARIOS y PERFILES 1:1
-- =========================
CREATE TABLE dbo.Usuarios (
  IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
  IdTipoUsuario INT NOT NULL,
  NombreUsuario NVARCHAR(50) NOT NULL UNIQUE,
  Contrasena NVARCHAR(100) NOT NULL,  -- texto plano de práctica
  Correo NVARCHAR(150) NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdTipoUsuario) REFERENCES dbo.TipoUsuario(IdTipoUsuario)
);

CREATE TABLE dbo.Compradores (
  IdComprador INT IDENTITY(1,1) PRIMARY KEY,
  IdUsuario INT NOT NULL UNIQUE,
  Nombres NVARCHAR(100) NOT NULL,
  Apellidos NVARCHAR(100) NOT NULL,
  Direccion NVARCHAR(200) NULL,
  Telefono NVARCHAR(30) NULL,
  EstadoCivil NVARCHAR(30) NULL,
  Nacionalidad NVARCHAR(60) NULL,
  Edad INT NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Vendedores (
  IdVendedor INT IDENTITY(1,1) PRIMARY KEY,
  IdUsuario INT NOT NULL UNIQUE,
  Nombres NVARCHAR(100) NOT NULL,
  Apellidos NVARCHAR(100) NOT NULL,
  Direccion NVARCHAR(200) NULL,
  CantidadInmueblesVendidos INT NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuarios(IdUsuario)
);

CREATE TABLE dbo.Agentes (
  IdAgente INT IDENTITY(1,1) PRIMARY KEY,
  IdUsuario INT NOT NULL UNIQUE,
  Codigo NVARCHAR(20) NOT NULL UNIQUE,
  Nombres NVARCHAR(100) NOT NULL,
  Apellidos NVARCHAR(100) NOT NULL,
  Telefono NVARCHAR(30) NULL,
  Correo NVARCHAR(150) NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuarios(IdUsuario)
);

-- =========================
-- INMUEBLES 
-- =========================
/* El vendedor registra el inmueble y luego lo publica con un agente. */
CREATE TABLE dbo.Inmuebles (
  IdInmueble INT IDENTITY(1,1) PRIMARY KEY,
  IdVendedor INT NOT NULL, 
  IdTipoInmueble INT NOT NULL,
  Direccion NVARCHAR(200) NOT NULL,
  Precio DECIMAL(12,2) NOT NULL,
  Metraje DECIMAL(12,2) NULL,
  AntiguedadAnos INT NULL,
  Modelo NVARCHAR(50) NULL,
  Material NVARCHAR(50) NULL,
  Descripcion NVARCHAR(500) NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdVendedor) REFERENCES dbo.Vendedores(IdVendedor),
  FOREIGN KEY (IdTipoInmueble) REFERENCES dbo.TiposInmueble(IdTipoInmueble)
);

-- =========================
-- INMUEBLECONDICION (M:N entre Inmuebles y Condicion)
-- =========================
CREATE TABLE dbo.InmuebleCondicion (
  IdInmueble  INT NOT NULL,
  IdCondicion INT NOT NULL,
  Eliminado   BIT NOT NULL DEFAULT 0,
  CONSTRAINT PK_InmuebleCondicion PRIMARY KEY (IdInmueble, IdCondicion),
  CONSTRAINT FK_InmuebleCondicion_Inmuebles
    FOREIGN KEY (IdInmueble)  REFERENCES dbo.Inmuebles(IdInmueble),
  CONSTRAINT FK_InmuebleCondicion_Condicion
    FOREIGN KEY (IdCondicion) REFERENCES dbo.Condicion(IdCondicion)
);

-- =========================
-- PUBLICACIONES (único lugar con IdAgente)
-- =========================
/* El vendedor elige el agente al publicar; el vendedor se deriva del inmueble. */
CREATE TABLE dbo.Publicaciones (
  IdPublicacion INT IDENTITY(1,1) PRIMARY KEY,
  IdInmueble INT NOT NULL UNIQUE,
  IdAgente INT NOT NULL,
  FechaPublicacion DATETIME2(0) NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdInmueble) REFERENCES dbo.Inmuebles(IdInmueble),
  FOREIGN KEY (IdAgente) REFERENCES dbo.Agentes(IdAgente)
);

-- =========================
-- TRANSACCIONALES
-- =========================
/* Ofertas: Vendedor/Agente/Inmueble DERIVABLES (Publicaciones -> Inmuebles) */
CREATE TABLE dbo.Ofertas (
  IdOferta INT IDENTITY(1,1) PRIMARY KEY,
  IdPublicacion INT NOT NULL,
  IdComprador INT NOT NULL,
  Monto DECIMAL(12,2) NOT NULL,
  IdFormaPago INT NOT NULL,
  PlazoDias INT NULL,
  FechaHora DATETIME2(0) NOT NULL,
  IdEstadoOferta INT NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdPublicacion) REFERENCES dbo.Publicaciones(IdPublicacion),
  FOREIGN KEY (IdComprador) REFERENCES dbo.Compradores(IdComprador),
  FOREIGN KEY (IdFormaPago) REFERENCES dbo.FormasPago(IdFormaPago),
  FOREIGN KEY (IdEstadoOferta) REFERENCES dbo.EstadoOferta(IdEstadoOferta)
);

/* Contraofertas: contexto DERIVABLE (Oferta -> Publicaciones -> Inmuebles) + Estado */
CREATE TABLE dbo.Contraofertas (
  IdContraoferta INT IDENTITY(1,1) PRIMARY KEY,
  IdOferta INT NOT NULL,
  Monto DECIMAL(12,2) NOT NULL,
  PlazoDias INT NULL,
  FechaHora DATETIME2(0) NOT NULL,
  IdEstadoOferta INT NOT NULL, 
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdOferta) REFERENCES dbo.Ofertas(IdOferta),
  FOREIGN KEY (IdEstadoOferta) REFERENCES dbo.EstadoOferta(IdEstadoOferta)
);

/* PRESTAMOS: registro asociado al comprador; se usará opcionalmente en Ventas */
CREATE TABLE dbo.Prestamos (
  IdPrestamo INT IDENTITY(1,1) PRIMARY KEY,
  IdComprador INT NOT NULL,                 -- quién solicitó/usa el préstamo
  CodigoPrestamo NVARCHAR(50) NOT NULL,     -- identificador del banco
  Descripcion NVARCHAR(300) NULL,           -- notas/estado simple
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdComprador) REFERENCES dbo.Compradores(IdComprador)
);

/* Ventas: referencia opcional a Prestamos (NULL si no hubo préstamo) */
CREATE TABLE dbo.Ventas (
  IdVenta INT IDENTITY(1,1) PRIMARY KEY,
  IdOfertaAceptada INT NOT NULL,
  IdPrestamo INT NULL,
  IdFormaPago INT NOT NULL,
  FechaCierre DATETIME2(0) NOT NULL,
  PrecioFinal DECIMAL(12,2) NOT NULL,
  PlazoDias INT NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdOfertaAceptada) REFERENCES dbo.Ofertas(IdOferta),
  FOREIGN KEY (IdPrestamo) REFERENCES dbo.Prestamos(IdPrestamo),
  FOREIGN KEY (IdFormaPago) REFERENCES dbo.FormasPago(IdFormaPago),
);

-- =========================
-- NOTIFICACIONES (ficticias)
-- =========================
/*
  Referencia Oferta O Contraoferta (XOR), guarda fecha/hora/descripcion.
  El contexto se deriva por:
    - Ofertas -> Publicaciones -> Inmuebles
    - Contraofertas -> Ofertas -> Publicaciones -> Inmuebles
*/
CREATE TABLE dbo.Notificaciones (
  IdNotificacion BIGINT IDENTITY(1,1) PRIMARY KEY,
  IdOferta INT NULL,
  IdContraoferta INT NULL,
  FechaHora DATETIME2(0) NOT NULL,
  Descripcion NVARCHAR(500) NOT NULL,
  Eliminado BIT NOT NULL DEFAULT 0,
  FOREIGN KEY (IdOferta) REFERENCES dbo.Ofertas(IdOferta),
  FOREIGN KEY (IdContraoferta) REFERENCES dbo.Contraofertas(IdContraoferta)
);