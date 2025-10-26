-- ============================================================
-- Real estate schema (Oracle) — inline CHECK constraints
-- ============================================================

-- ---------- Safe drops (ignore errors if not exists)
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Notificaciones CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Ventas CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Prestamos CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Contraofertas CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Ofertas CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Publicaciones CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE InmuebleCondicion CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Inmuebles CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE FormasPago CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE EstadoOferta CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Condicion CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE TiposInmueble CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Agentes CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Vendedores CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Compradores CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE Usuarios CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE TipoUsuario CASCADE CONSTRAINTS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/

-- ============================================================
-- 1) Catalogs
-- ============================================================
CREATE TABLE TipoUsuario (
  IdTipoUsuario NUMBER PRIMARY KEY,
  NombreTipo    NVARCHAR2(50) NOT NULL,
  Eliminado     NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1))
);

CREATE TABLE TiposInmueble (
  IdTipoInmueble NUMBER PRIMARY KEY,
  NombreTipo     NVARCHAR2(50) NOT NULL,
  Eliminado      NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1))
);

CREATE TABLE FormasPago (
  IdFormaPago     NUMBER PRIMARY KEY,
  NombreFormaPago  NVARCHAR2(50) NOT NULL,
  Eliminado        NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1))
);

CREATE TABLE EstadoOferta (
  IdEstadoOferta NUMBER PRIMARY KEY,
  NombreEstado   NVARCHAR2(20) NOT NULL,
  Eliminado      NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1))
);

CREATE TABLE Condicion (
  IdCondicion    NUMBER PRIMARY KEY,
  NombreCondicion NVARCHAR2(100) NOT NULL,
  Eliminado       NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1))
);

-- ============================================================
-- 2) Users and 1:1 profiles
-- ============================================================
CREATE TABLE Usuarios (
  IdUsuario     NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdTipoUsuario NUMBER NOT NULL,
  NombreUsuario NVARCHAR2(50) NOT NULL UNIQUE,
  Contrasena    NVARCHAR2(100) NOT NULL,
  Correo        NVARCHAR2(150),
  Eliminado     NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_usuarios_tipousuario FOREIGN KEY (IdTipoUsuario) REFERENCES TipoUsuario(IdTipoUsuario)
);

CREATE TABLE Compradores (
  IdComprador  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdUsuario    NUMBER NOT NULL UNIQUE,
  Nombres      NVARCHAR2(100) NOT NULL,
  Apellidos    NVARCHAR2(100) NOT NULL,
  Direccion    NVARCHAR2(200),
  Telefono     NVARCHAR2(30),
  EstadoCivil  NVARCHAR2(30),
  Nacionalidad NVARCHAR2(60),
  Edad         NUMBER(4) CHECK (Edad >= 0),
  Eliminado    NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_compradores_usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE Vendedores (
  IdVendedor                NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdUsuario                 NUMBER NOT NULL UNIQUE,
  Nombres                   NVARCHAR2(100) NOT NULL,
  Apellidos                 NVARCHAR2(100) NOT NULL,
  Direccion                 NVARCHAR2(200),
  CantidadInmueblesVendidos NUMBER CHECK (CantidadInmueblesVendidos >= 0),
  Eliminado                 NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_vendedores_usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE Agentes (
  IdAgente  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdUsuario NUMBER NOT NULL UNIQUE,
  Codigo    NVARCHAR2(20) NOT NULL UNIQUE,
  Nombres   NVARCHAR2(100) NOT NULL,
  Apellidos NVARCHAR2(100) NOT NULL,
  Telefono  NVARCHAR2(30),
  Correo    NVARCHAR2(150),
  Eliminado NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_agentes_usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

-- ============================================================
-- 3) Properties and conditions
-- ============================================================
CREATE TABLE Inmuebles (
  IdInmueble     NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdVendedor     NUMBER NOT NULL,
  IdTipoInmueble NUMBER NOT NULL,
  Direccion      NVARCHAR2(200) NOT NULL,
  Precio         NUMBER(12,2) NOT NULL CHECK (Precio >= 0),
  Metraje        NUMBER(12,2) CHECK (Metraje >= 0),
  AntiguedadAnos NUMBER CHECK (AntiguedadAnos >= 0),
  Modelo         NVARCHAR2(50),
  Material       NVARCHAR2(50),
  Descripcion    NVARCHAR2(500),
  Eliminado      NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_inmuebles_vendedores     FOREIGN KEY (IdVendedor)     REFERENCES Vendedores(IdVendedor),
  CONSTRAINT fk_inmuebles_tipoinmueble   FOREIGN KEY (IdTipoInmueble) REFERENCES TiposInmueble(IdTipoInmueble)
);

CREATE TABLE InmuebleCondicion (
  IdInmueble  NUMBER NOT NULL,
  IdCondicion NUMBER NOT NULL,
  Eliminado   NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT pk_inmueblecondicion PRIMARY KEY (IdInmueble, IdCondicion),
  CONSTRAINT fk_inmueblecondicion_inmuebles  FOREIGN KEY (IdInmueble)  REFERENCES Inmuebles(IdInmueble),
  CONSTRAINT fk_inmueblecondicion_condicion  FOREIGN KEY (IdCondicion) REFERENCES Condicion(IdCondicion)
);

-- ============================================================
-- 4) Listings (only place with agent)
-- ============================================================
CREATE TABLE Publicaciones (
  IdPublicacion    NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdInmueble       NUMBER NOT NULL,
  IdAgente         NUMBER NOT NULL,
  FechaPublicacion TIMESTAMP(0) NOT NULL,
  Eliminado        NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_publicaciones_inmuebles FOREIGN KEY (IdInmueble) REFERENCES Inmuebles(IdInmueble),
  CONSTRAINT fk_publicaciones_agentes   FOREIGN KEY (IdAgente)   REFERENCES Agentes(IdAgente)
);
CREATE UNIQUE INDEX ux_publicaciones_inmueble ON Publicaciones(IdInmueble);

-- ============================================================
-- 5) Transactions
-- ============================================================
CREATE TABLE Ofertas (
  IdOferta       NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdPublicacion  NUMBER NOT NULL,
  IdComprador    NUMBER NOT NULL,
  Monto          NUMBER(12,2) NOT NULL CHECK (Monto >= 0),
  IdFormaPago    NUMBER NOT NULL,
  PlazoDias      NUMBER CHECK (PlazoDias >= 0),
  FechaHora      TIMESTAMP(0) NOT NULL,
  IdEstadoOferta NUMBER NOT NULL,
  Eliminado      NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_ofertas_publicaciones   FOREIGN KEY (IdPublicacion)  REFERENCES Publicaciones(IdPublicacion),
  CONSTRAINT fk_ofertas_compradores     FOREIGN KEY (IdComprador)    REFERENCES Compradores(IdComprador),
  CONSTRAINT fk_ofertas_formaspago      FOREIGN KEY (IdFormaPago)    REFERENCES FormasPago(IdFormaPago),
  CONSTRAINT fk_ofertas_estadooferta    FOREIGN KEY (IdEstadoOferta) REFERENCES EstadoOferta(IdEstadoOferta)
);

CREATE TABLE Contraofertas (
  IdContraoferta NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdOferta       NUMBER NOT NULL,
  Monto          NUMBER(12,2) NOT NULL CHECK (Monto >= 0),
  PlazoDias      NUMBER CHECK (PlazoDias >= 0),
  FechaHora      TIMESTAMP(0) NOT NULL,
  IdEstadoOferta NUMBER NOT NULL,
  Eliminado      NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_contraofertas_ofertas      FOREIGN KEY (IdOferta)       REFERENCES Ofertas(IdOferta),
  CONSTRAINT fk_contraofertas_estadooferta FOREIGN KEY (IdEstadoOferta) REFERENCES EstadoOferta(IdEstadoOferta)
);

CREATE TABLE Prestamos (
  IdPrestamo     NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdComprador    NUMBER NOT NULL,
  CodigoPrestamo NVARCHAR2(50) NOT NULL,
  Descripcion    NVARCHAR2(300),
  Eliminado      NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_prestamos_compradores FOREIGN KEY (IdComprador) REFERENCES Compradores(IdComprador)
);

CREATE TABLE Ventas (
  IdVenta          NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdOfertaAceptada NUMBER NOT NULL,
  IdPrestamo       NUMBER, -- NULL if not financed
  IdFormaPago      NUMBER NOT NULL,
  FechaCierre      TIMESTAMP(0) NOT NULL,
  PrecioFinal      NUMBER(12,2) NOT NULL CHECK (PrecioFinal >= 0),
  PlazoDias        NUMBER CHECK (PlazoDias >= 0),
  Eliminado        NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_ventas_ofertaaceptada FOREIGN KEY (IdOfertaAceptada) REFERENCES Ofertas(IdOferta),
  CONSTRAINT fk_ventas_prestamos      FOREIGN KEY (IdPrestamo)       REFERENCES Prestamos(IdPrestamo),
  CONSTRAINT fk_ventas_formaspago     FOREIGN KEY (IdFormaPago)      REFERENCES FormasPago(IdFormaPago)
);

-- ============================================================
-- 6) Notifications
-- ============================================================
CREATE TABLE Notificaciones (
  IdNotificacion NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdOferta       NUMBER,
  IdContraoferta NUMBER,
  FechaHora      TIMESTAMP(0) NOT NULL,
  Descripcion    NVARCHAR2(500) NOT NULL,
  Eliminado      NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_notif_ofertas       FOREIGN KEY (IdOferta)       REFERENCES Ofertas(IdOferta),
  CONSTRAINT fk_notif_contraofertas FOREIGN KEY (IdContraoferta) REFERENCES Contraofertas(IdContraoferta),
  -- Must stay table-level (multi-column):
  CONSTRAINT ck_notif_xor CHECK (
    (IdOferta IS NOT NULL AND IdContraoferta IS NULL) OR
    (IdOferta IS NULL AND IdContraoferta IS NOT NULL)
  )
);

-------------------INSERT----------------------------
--tipousuario
INSERT INTO TipoUsuario (IdTipoUsuario, NombreTipo, Eliminado)
VALUES (1, 'Agente', 0);

INSERT INTO TipoUsuario (IdTipoUsuario, NombreTipo, Eliminado)
VALUES (2, 'Vendedor', 0);

INSERT INTO TipoUsuario (IdTipoUsuario, NombreTipo, Eliminado)
VALUES (3, 'Comprador', 0);

INSERT INTO TipoUsuario (IdTipoUsuario, NombreTipo, Eliminado)
VALUES (99, 'ADMIN', 1);
COMMIT;
--USUARIOS
INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado)
VALUES (99, 'admin', 'admin', 'admin@mail.com', 0);

--TIPOSINMUEBLE
INSERT INTO TiposInmueble (IdTipoInmueble, NombreTipo, Eliminado)
VALUES (1, 'Finca', 0);

INSERT INTO TiposInmueble (IdTipoInmueble, NombreTipo, Eliminado)
VALUES (2, 'Casa', 0);
COMMIT;

--FormasPago
INSERT INTO FormasPago (IDFORMAPAGO, NOMBREFORMAPAGO, Eliminado)
VALUES (1, 'Tarjeta', 0);

INSERT INTO FormasPago (IDFORMAPAGO, NOMBREFORMAPAGO, Eliminado)
VALUES (2, 'Prestamo', 0);
COMMIT;

--Estado Oferta
INSERT INTO ESTADOOFERTA (IDESTADOOFERTA, NOMBREESTADO, Eliminado)
VALUES (1, 'Disponible', 0);

INSERT INTO ESTADOOFERTA (IDESTADOOFERTA, NOMBREESTADO, Eliminado)
VALUES (2, 'Aceptada', 0);

INSERT INTO ESTADOOFERTA (IDESTADOOFERTA, NOMBREESTADO, Eliminado)
VALUES (3, 'Rechazada', 0);

INSERT INTO ESTADOOFERTA (IDESTADOOFERTA, NOMBREESTADO, Eliminado)
VALUES (4, 'ContraOferta', 0);


COMMIT;

--Condicion
INSERT INTO CONDICION (IDCONDICION, NOMBRECONDICION, Eliminado)
VALUES (1, 'Nuevo', 0);

INSERT INTO CONDICION (IDCONDICION, NOMBRECONDICION, Eliminado)
VALUES (2, 'Usado', 0);

INSERT INTO CONDICION (IDCONDICION, NOMBRECONDICION, Eliminado)
VALUES (3, 'Contruccion', 0);

COMMIT;

SELECT u.* FROM Usuarios u JOIN TipoUsuario t ON u.IdTipoUsuario = t.IdTipoUsuario WHERE t.IdTipoUsuario = 1