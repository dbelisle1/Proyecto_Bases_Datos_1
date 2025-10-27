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
  IdUsuario    NUMBER NULL UNIQUE,
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
  IdUsuario                 NUMBER NULL UNIQUE,
  Nombres                   NVARCHAR2(100) NOT NULL,
  Apellidos                 NVARCHAR2(100) NOT NULL,
  Direccion                 NVARCHAR2(200),
  CantidadInmueblesVendidos NUMBER CHECK (CantidadInmueblesVendidos >= 0),
  Eliminado                 NUMBER(1) DEFAULT 0 NOT NULL CHECK (Eliminado IN (0,1)),
  CONSTRAINT fk_vendedores_usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE Agentes (
  IdAgente  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  IdUsuario NUMBER NULL UNIQUE,
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
INSERT INTO TiposInmueble (IdTipoInmueble, NombreTipo, Eliminado) VALUES (3, 'Apartamento', 0);
INSERT INTO TiposInmueble (IdTipoInmueble, NombreTipo, Eliminado) VALUES (4, 'Terreno', 0);
COMMIT;

--FormasPago
INSERT INTO FormasPago (IDFORMAPAGO, NOMBREFORMAPAGO, Eliminado)
VALUES (1, 'Tarjeta', 0);

INSERT INTO FormasPago (IDFORMAPAGO, NOMBREFORMAPAGO, Eliminado)
VALUES (2, 'Prestamo', 0);
COMMIT;

INSERT INTO FormasPago (IdFormaPago, NombreFormaPago, Eliminado) VALUES (3, 'Contado', 0);
INSERT INTO FormasPago (IdFormaPago, NombreFormaPago, Eliminado) VALUES (4, 'Transferencia', 0);
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
INSERT INTO ESTADOOFERTA (IDESTADOOFERTA, NOMBREESTADO, Eliminado)
VALUES (5, 'Pendiente', 0);
COMMIT;

COMMIT;

--Condicion
INSERT INTO CONDICION (IDCONDICION, NOMBRECONDICION, Eliminado)
VALUES (1, 'Nuevo', 0);

INSERT INTO CONDICION (IDCONDICION, NOMBRECONDICION, Eliminado)
VALUES (2, 'Usado', 0);

INSERT INTO CONDICION (IDCONDICION, NOMBRECONDICION, Eliminado)
VALUES (3, 'Contruccion', 0);

INSERT INTO CONDICION (IDCONDICION, NOMBRECONDICION, Eliminado)
VALUES (4, 'En Proceso', 0);

COMMIT;

--Vendedores

INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado)
VALUES (2, 'vendedorUsr', 'vendedorUsr', 'vendedorUsr@mail.com', 0);

INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado) VALUES (2, 'vendedor2', 'vendedor2', 'vendedor2@mail.com
', 0);
INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado) VALUES (2, 'vendedor3', 'vendedor3', 'vendedor3@mail.com
', 0);

INSERT INTO Vendedores (IdUsuario, Nombres, Apellidos, Direccion) 
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='vendedorUsr'), 'vendedorUsr', 'vendedorUsrAp', 'DirvendedorUsr');
COMMIT;

INSERT INTO Vendedores (IdUsuario, Nombres, Apellidos, Direccion, CantidadInmueblesVendidos, Eliminado)
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='vendedor2'), 'Vendedor2N', 'Vendedor2A', 'DirVendedor2', 0, 0);

INSERT INTO Vendedores (IdUsuario, Nombres, Apellidos, Direccion, CantidadInmueblesVendidos, Eliminado)
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='vendedor3'), 'Vendedor3N', 'Vendedor3A', 'DirVendedor3', 0, 0);
COMMIT;


--Compradores

INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado)
VALUES (3, 'CompUsr', 'CompUsr', 'CompUsr@mail.com', 0);
INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado) VALUES (3, 'comp2', 'comp2', 'comp2@mail.com
', 0);
INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado) VALUES (3, 'comp3', 'comp3', 'comp3@mail.com
', 0);
COMMIT;

INSERT INTO Compradores (IdUsuario, Nombres, Apellidos, Direccion, Telefono, EstadoCivil, Nacionalidad, Edad) 
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='CompUsr'), 'CompUsr', 'CompUsrAp', 'DirCompUsr', 'telCompUsr', 'estCompUsr', 'nacCompUsr',25);
COMMIT;
INSERT INTO Compradores (IdUsuario, Nombres, Apellidos, Direccion, Telefono, EstadoCivil, Nacionalidad, Edad, Eliminado)
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='comp2'), 'Comp2N', 'Comp2A', 'DirComp2', '5022002', 'Soltero', 'Guatemala', 28, 0);

INSERT INTO Compradores (IdUsuario, Nombres, Apellidos, Direccion, Telefono, EstadoCivil, Nacionalidad, Edad, Eliminado)
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='comp3'), 'Comp3N', 'Comp3A', 'DirComp3', '5022003', 'Casado', 'Guatemala', 35, 0);
COMMIT;


-- Agente

INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado)
VALUES (1, 'agenteUsr', 'agenteUsr', 'agenteUsr@mail.com', 0);
INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado) VALUES (1, 'agente2', 'agente2', 'agente2@mail.com
', 0);
INSERT INTO Usuarios (IdTipoUsuario, NombreUsuario, Contrasena, Correo, Eliminado) VALUES (1, 'agente3', 'agente3', 'agente3@mail.com
', 0);

INSERT INTO AgenteS (IdUsuario, Codigo, Nombres, Apellidos, Telefono, Correo)
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='agenteUsr'), 'agenteUsrCod', 'NagenteUsr', 'ApagenteUsr', 'TelagenteUsr', 'CoragenteUsr');
COMMIT;

INSERT INTO Agentes (IdUsuario, Codigo, Nombres, Apellidos, Telefono, Correo, Eliminado)
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='agente2'), 'AG002', 'Agente2N', 'Agente2A', '5021002', 'agente2@mail.com
', 0);

INSERT INTO Agentes (IdUsuario, Codigo, Nombres, Apellidos, Telefono, Correo, Eliminado)
VALUES ((SELECT IdUsuario FROM Usuarios WHERE NombreUsuario='agente3'), 'AG003', 'Agente3N', 'Agente3A', '5021003', 'agente3@mail.com
', 0);
COMMIT;

-- Inmuebles

INSERT INTO INMUEBLES (IdVendedor, IdTipoInmueble, Direccion, Precio, Metraje, AntiguedadAnos, Modelo, Material, Descripcion)
Values (1, 1, 'DirInm1', 10, 20, 30, 'ModInm1', 'MatInm1', 'DescInm1');
COMMIT;

INSERT INTO INMUEBLECONDICION(IDINMUEBLE, IDCONDICION) VALUES (1,1);
INSERT INTO INMUEBLECONDICION(IDINMUEBLE, IDCONDICION) VALUES (1,3);
INSERT INTO INMUEBLECONDICION(IDINMUEBLE, IDCONDICION) VALUES (1,2);
COMMIT;

INSERT INTO INMUEBLES (IdVendedor, IdTipoInmueble, Direccion, Precio, Metraje, AntiguedadAnos, Modelo, Material, Descripcion)
Values (1, 1, 'DirInm2', 10, 20, 30, 'ModInm2', 'MatInm2', 'DescInm2');
COMMIT;

INSERT INTO INMUEBLECONDICION(IDINMUEBLE, IDCONDICION) VALUES (2,1);
INSERT INTO INMUEBLECONDICION(IDINMUEBLE, IDCONDICION) VALUES (2,4);
INSERT INTO INMUEBLECONDICION(IDINMUEBLE, IDCONDICION) VALUES (2,2);
COMMIT;

INSERT INTO Inmuebles (IdVendedor, IdTipoInmueble, Direccion, Precio, Metraje, AntiguedadAnos, Modelo, Material, Descripcion, Eliminado)
VALUES (
(SELECT v.IdVendedor FROM Vendedores v JOIN Usuarios u ON v.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='vendedor2'),
2, 'DirInm3', 120000, 180, 5, 'ModInm3', 'Block', 'Casa de 3 habitaciones', 0
);


INSERT INTO Inmuebles (IdVendedor, IdTipoInmueble, Direccion, Precio, Metraje, AntiguedadAnos, Modelo, Material, Descripcion, Eliminado)
VALUES (
(SELECT v.IdVendedor FROM Vendedores v JOIN Usuarios u ON v.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='vendedor2'),
3, 'DirInm4', 80000, 85, 2, 'ModInm4', 'Concreto', 'Apartamento céntrico', 0
);

INSERT INTO Inmuebles (IdVendedor, IdTipoInmueble, Direccion, Precio, Metraje, AntiguedadAnos, Modelo, Material, Descripcion, Eliminado)
VALUES (
(SELECT v.IdVendedor FROM Vendedores v JOIN Usuarios u ON v.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='vendedor3'),
4, 'DirInm5', 50000, 1000, 0, 'ModInm5', 'Tierra', 'Terreno en desarrollo', 0
);
COMMIT;

-- Inmueble 1: agregar (1,2,3) si faltan
INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT 1, 1 FROM dual
WHERE NOT EXISTS (SELECT 1 FROM InmuebleCondicion WHERE IdInmueble=1 AND IdCondicion=1);

INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT 1, 2 FROM dual
WHERE NOT EXISTS (SELECT 1 FROM InmuebleCondicion WHERE IdInmueble=1 AND IdCondicion=2);

INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT 1, 3 FROM dual
WHERE NOT EXISTS (SELECT 1 FROM InmuebleCondicion WHERE IdInmueble=1 AND IdCondicion=3);

-- Inmueble 2: agregar (2,3,4) si faltan
INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT 2, 2 FROM dual
WHERE NOT EXISTS (SELECT 1 FROM InmuebleCondicion WHERE IdInmueble=2 AND IdCondicion=2);

INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT 2, 3 FROM dual
WHERE NOT EXISTS (SELECT 1 FROM InmuebleCondicion WHERE IdInmueble=2 AND IdCondicion=3);

INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT 2, 4 FROM dual
WHERE NOT EXISTS (SELECT 1 FROM InmuebleCondicion WHERE IdInmueble=2 AND IdCondicion=4);

-- Inmueble 3 (DirInm3): ya tenía (2,4); agregamos (1,3) si faltan
INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT i.IdInmueble, 1 FROM Inmuebles i
WHERE i.Direccion='DirInm3'
AND NOT EXISTS (SELECT 1 FROM InmuebleCondicion ic WHERE ic.IdInmueble=i.IdInmueble AND ic.IdCondicion=1);

INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT i.IdInmueble, 3 FROM Inmuebles i
WHERE i.Direccion='DirInm3'
AND NOT EXISTS (SELECT 1 FROM InmuebleCondicion ic WHERE ic.IdInmueble=i.IdInmueble AND ic.IdCondicion=3);

-- Inmueble 4 (DirInm4): ya tenía (1); agregamos (3,4) si faltan
INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT i.IdInmueble, 3 FROM Inmuebles i
WHERE i.Direccion='DirInm4'
AND NOT EXISTS (SELECT 1 FROM InmuebleCondicion ic WHERE ic.IdInmueble=i.IdInmueble AND ic.IdCondicion=3);

INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT i.IdInmueble, 4 FROM Inmuebles i
WHERE i.Direccion='DirInm4'
AND NOT EXISTS (SELECT 1 FROM InmuebleCondicion ic WHERE ic.IdInmueble=i.IdInmueble AND ic.IdCondicion=4);

-- Inmueble 5 (DirInm5): ya tenía (3); agregamos (1,4) si faltan
INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT i.IdInmueble, 1 FROM Inmuebles i
WHERE i.Direccion='DirInm5'
AND NOT EXISTS (SELECT 1 FROM InmuebleCondicion ic WHERE ic.IdInmueble=i.IdInmueble AND ic.IdCondicion=1);

INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion)
SELECT i.IdInmueble, 4 FROM Inmuebles i
WHERE i.Direccion='DirInm5'
AND NOT EXISTS (SELECT 1 FROM InmuebleCondicion ic WHERE ic.IdInmueble=i.IdInmueble AND ic.IdCondicion=4);

COMMIT;


-- ===== Publicaciones de los nuevos inmuebles =====
INSERT INTO Publicaciones (IdInmueble, IdAgente, FechaPublicacion, Eliminado)
VALUES (
(SELECT IdInmueble FROM Inmuebles WHERE Direccion='DirInm3'),
(SELECT IdAgente FROM Agentes WHERE Codigo='AG002'),
SYSTIMESTAMP - INTERVAL '20' DAY, 0
);

INSERT INTO Publicaciones (IdInmueble, IdAgente, FechaPublicacion, Eliminado)
VALUES (
(SELECT IdInmueble FROM Inmuebles WHERE Direccion='DirInm4'),
(SELECT IdAgente FROM Agentes WHERE Codigo='AG002'),
SYSTIMESTAMP - INTERVAL '18' DAY, 0
);

INSERT INTO Publicaciones (IdInmueble, IdAgente, FechaPublicacion, Eliminado)
VALUES (
(SELECT IdInmueble FROM Inmuebles WHERE Direccion='DirInm5'),
(SELECT IdAgente FROM Agentes WHERE Codigo='AG003'),
SYSTIMESTAMP - INTERVAL '12' DAY, 0
);
COMMIT;

-- ===== Ofertas sobre las nuevas publicaciones =====
-- Publicación Inm3 (DirInm3)
-- Oferta rechazada de comp3
INSERT INTO Ofertas (IdPublicacion, IdComprador, Monto, IdFormaPago, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3'),
(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp3'),
112000, 4, 0, SYSTIMESTAMP - INTERVAL '17' DAY, 3, 0
); -- 3=Rechazada

-- Oferta con contraoferta de comp2
INSERT INTO Ofertas (IdPublicacion, IdComprador, Monto, IdFormaPago, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3'),
(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp2'),
115000, 2, 360, SYSTIMESTAMP - INTERVAL '16' DAY, 4, 0
); -- 4=ContraOferta
COMMIT;

-- ===== Contraofertas (sobre oferta de comp2 en Inm3) =====
INSERT INTO Contraofertas (IdOferta, Monto, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
AND o.IdComprador=(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp2')),
118000, 360, SYSTIMESTAMP - INTERVAL '15' DAY, 5, 0
); -- 5=Pendiente

-- Nueva contraoferta aceptada
INSERT INTO Contraofertas (IdOferta, Monto, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
AND o.IdComprador=(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp2')),
118000, 360, SYSTIMESTAMP - INTERVAL '14' DAY, 2, 0
); -- 2=Aceptada
COMMIT;

-- ===== Oferta final aceptada para cerrar venta (Inm3) =====
INSERT INTO Ofertas (IdPublicacion, IdComprador, Monto, IdFormaPago, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3'),
(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp2'),
118000, 2, 360, SYSTIMESTAMP - INTERVAL '13' DAY, 2, 0
); -- 2=Aceptada
COMMIT;

-- ===== Ofertas para Inm4 (DirInm4) y Inm5 (DirInm5) =====
-- Inm4: oferta al contado aceptada por comp3
INSERT INTO Ofertas (IdPublicacion, IdComprador, Monto, IdFormaPago, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm4'),
(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp3'),
78000, 3, 0, SYSTIMESTAMP - INTERVAL '10' DAY, 2, 0
); -- Contado, Aceptada

-- Inm5: oferta pendiente de comp2
INSERT INTO Ofertas (IdPublicacion, IdComprador, Monto, IdFormaPago, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm5'),
(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp2'),
48000, 4, 0, SYSTIMESTAMP - INTERVAL '9' DAY, 5, 0
); -- Pendiente

-- Inm5: oferta rechazada de comp3
INSERT INTO Ofertas (IdPublicacion, IdComprador, Monto, IdFormaPago, PlazoDias, FechaHora, IdEstadoOferta, Eliminado)
VALUES (
(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm5'),
(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp3'),
45000, 3, 0, SYSTIMESTAMP - INTERVAL '8' DAY, 3, 0
); -- Rechazada
COMMIT;

-- ===== Préstamos para nuevos cierres =====
INSERT INTO Prestamos (IdComprador, CodigoPrestamo, Descripcion, Eliminado)
VALUES (
(SELECT c.IdComprador FROM Compradores c JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp2'),
'PR-0002', 'Préstamo hipotecario para Inm3', 0
);
COMMIT;

-- ===== Ventas nuevas =====
-- Venta 2: Inm3 financiado con PR-0002
INSERT INTO Ventas (IdOfertaAceptada, IdPrestamo, IdFormaPago, FechaCierre, PrecioFinal, PlazoDias, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
AND o.IdEstadoOferta=2),
(SELECT MAX(p.IdPrestamo) FROM Prestamos p JOIN Compradores c ON p.IdComprador=c.IdComprador
JOIN Usuarios u ON c.IdUsuario=u.IdUsuario WHERE u.NombreUsuario='comp2' AND p.CodigoPrestamo='PR-0002'),
2, SYSTIMESTAMP - INTERVAL '7' DAY, 118000, 360, 0
);

-- Venta 3: Inm4 al contado (sin préstamo)
INSERT INTO Ventas (IdOfertaAceptada, IdPrestamo, IdFormaPago, FechaCierre, PrecioFinal, PlazoDias, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm4')
AND o.IdEstadoOferta=2),
NULL,
3, SYSTIMESTAMP - INTERVAL '6' DAY, 78000, 0, 0
);
COMMIT;

-- ===== Notificaciones adicionales =====
-- Inm3: Oferta rechazada, contraofertas y aceptación
INSERT INTO Notificaciones (IdOferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MIN(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
AND o.IdEstadoOferta=3),
SYSTIMESTAMP - INTERVAL '17' DAY + INTERVAL '10' MINUTE, 'Oferta rechazada en publicación de Inm3', 0
);

INSERT INTO Notificaciones (IdOferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
AND o.IdEstadoOferta=4),
SYSTIMESTAMP - INTERVAL '16' DAY + INTERVAL '5' MINUTE, 'Nueva oferta con solicitud de contraoferta en Inm3', 0
);

INSERT INTO Notificaciones (IdContraoferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MIN(c.IdContraoferta) FROM Contraofertas c
WHERE c.IdOferta IN (
SELECT o.IdOferta FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
)),
SYSTIMESTAMP - INTERVAL '15' DAY + INTERVAL '15' MINUTE, 'Contraoferta enviada (pendiente) para Inm3', 0
);

INSERT INTO Notificaciones (IdContraoferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MAX(c.IdContraoferta) FROM Contraofertas c
WHERE c.IdOferta IN (
SELECT o.IdOferta FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
)),
SYSTIMESTAMP - INTERVAL '14' DAY + INTERVAL '20' MINUTE, 'Contraoferta aceptada para Inm3', 0
);

INSERT INTO Notificaciones (IdOferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm3')
AND o.IdEstadoOferta=2),
SYSTIMESTAMP - INTERVAL '13' DAY + INTERVAL '25' MINUTE, 'Oferta aceptada: se procede al cierre (Inm3)', 0
);

-- Inm4: oferta aceptada al contado
INSERT INTO Notificaciones (IdOferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm4')
AND o.IdEstadoOferta=2),
SYSTIMESTAMP - INTERVAL '10' DAY + INTERVAL '12' MINUTE, 'Oferta al contado aceptada (Inm4)', 0
);

-- Inm5: ofertas pendiente y rechazada
INSERT INTO Notificaciones (IdOferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MIN(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm5')
AND o.IdEstadoOferta=5),
SYSTIMESTAMP - INTERVAL '9' DAY + INTERVAL '8' MINUTE, 'Oferta pendiente en Inm5', 0
);

INSERT INTO Notificaciones (IdOferta, FechaHora, Descripcion, Eliminado)
VALUES (
(SELECT MAX(o.IdOferta) FROM Ofertas o
WHERE o.IdPublicacion=(SELECT p.IdPublicacion FROM Publicaciones p JOIN Inmuebles i ON p.IdInmueble=i.IdInmueble WHERE i.Direccion='DirInm5')
AND o.IdEstadoOferta=3),
SYSTIMESTAMP - INTERVAL '8' DAY + INTERVAL '4' MINUTE, 'Oferta rechazada en Inm5', 0
);
COMMIT;




---------------REPORTES----------------\

-- Formas de pago usadas por un comprador en sus ventas

SELECT u.NOMBREUSUARIO AS COMPRADOR, fp.NOMBREFORMAPAGO, COUNT(*) AS VECES
FROM VENTAS v
JOIN OFERTAS o ON o.IDOFERTA = v.IDOFERTAACEPTADA
JOIN COMPRADORES c ON c.IDCOMPRADOR = o.IDCOMPRADOR
JOIN USUARIOS u ON u.IDUSUARIO = c.IDUSUARIO
LEFT JOIN FORMASPAGO fp ON fp.IDFORMAPAGO = v.IDFORMAPAGO
WHERE c.IDCOMPRADOR = 3--(:IdComprador IS NULL OR c.IDCOMPRADOR = :IdComprador)
  AND NVL(v.ELIMINADO,0)=0
GROUP BY u.NOMBREUSUARIO, fp.NOMBREFORMAPAGO
ORDER BY VECES DESC;


--UPDATE VENTAS SET IDFORMAPAGO = 3 WHERE IDVENTA = 2;
--commit;

-- Inmuebles por vendedor y tipo (con tipo)
SELECT v.IDVENDEDOR,
       v.NOMBRES || ' ' || v.APELLIDOS AS VENDEDOR,
       ti.NOMBRETIPO,
       COUNT(*) AS INMUEBLES,
       SUM(i.PRECIO) AS TOTAL_LISTADO,
       ROUND(AVG(i.PRECIO)) AS PRECIO_PROM
FROM INMUEBLES i
JOIN VENDEDORES v ON v.IDVENDEDOR = i.IDVENDEDOR
JOIN TIPOSINMUEBLE ti ON ti.IDTIPOINMUEBLE = i.IDTIPOINMUEBLE
WHERE NVL(i.ELIMINADO,0)=0 AND v.IdVendedor = 2 AND ti.IDTIPOINMUEBLE = 3 -- :IdVendedor :IdTipoInmueble
GROUP BY v.IDVENDEDOR, v.NOMBRES, v.APELLIDOS, ti.NOMBRETIPO
ORDER BY INMUEBLES DESC;

--Top interés por inmueble
SELECT i.IDINMUEBLE, i.DIRECCION, COUNT(o.IDOFERTA) AS OFERTAS
FROM INMUEBLES i
JOIN PUBLICACIONES p ON p.IDINMUEBLE = i.IDINMUEBLE
LEFT JOIN OFERTAS o ON o.IDPUBLICACION = p.IDPUBLICACION
WHERE o.FECHAHORA BETWEEN DATE '2025-10-09' AND DATE  '2025-10-18' --:StartDate AND :EndDate
GROUP BY i.IDINMUEBLE, i.DIRECCION
ORDER BY OFERTAS DESC;

-- Resumen por vendedor
SELECT vnd.IDVENDEDOR,
       vnd.NOMBRES || ' ' || vnd.APELLIDOS AS VENDEDOR,
       COUNT(DISTINCT i.IDINMUEBLE) AS INMUEBLES_LISTADOS,
       COUNT(DISTINCT v.IDOFERTAACEPTADA) AS INMUEBLES_VENDIDOS,
       NVL(SUM(v.PRECIOFINAL), 0) AS INGRESOS
FROM VENDEDORES vnd
LEFT JOIN INMUEBLES i   ON i.IDVENDEDOR = vnd.IDVENDEDOR AND NVL(i.ELIMINADO,0)=0
LEFT JOIN PUBLICACIONES p ON p.IDINMUEBLE = i.IDINMUEBLE AND NVL(p.ELIMINADO,0)=0
LEFT JOIN OFERTAS o     ON o.IDPUBLICACION = p.IDPUBLICACION AND NVL(o.ELIMINADO,0)=0
LEFT JOIN VENTAS v      ON v.IDOFERTAACEPTADA = o.IDOFERTA AND NVL(v.ELIMINADO,0)=0
WHERE vnd.IDVENDEDOR = 3 --(:IdVendedor IS NULL OR vnd.IDVENDEDOR = :IdVendedor)
  --AND (:StartDate IS NULL OR p.FECHAPUBLICACION >= :StartDate)
  --AND (:EndDate   IS NULL OR p.FECHAPUBLICACION <  :EndDate)
GROUP BY vnd.IDVENDEDOR, vnd.NOMBRES, vnd.APELLIDOS
ORDER BY INGRESOS DESC;

-- Ventas por agente y forma de pago
SELECT a.CODIGO,
       a.NOMBRES || ' ' || a.APELLIDOS AS AGENTE,
       fp.NOMBREFORMAPAGO,
       COUNT(*) AS VENTAS,
       SUM(v.PRECIOFINAL) AS INGRESOS,
       ROUND(AVG(v.PRECIOFINAL)) AS TICKET_PROM
FROM VENTAS v
JOIN OFERTAS o  ON o.IDOFERTA = v.IDOFERTAACEPTADA
JOIN PUBLICACIONES p ON p.IDPUBLICACION = o.IDPUBLICACION
JOIN AGENTES a ON a.IDAGENTE = p.IDAGENTE
LEFT JOIN FORMASPAGO fp ON fp.IDFORMAPAGO = v.IDFORMAPAGO
WHERE v.FECHACIERRE BETWEEN DATE '2025-10-19' AND DATE  '2025-10-21'--:StartDate AND :EndDate
  AND NVL(v.ELIMINADO,0)=0
GROUP BY a.CODIGO, a.NOMBRES, a.APELLIDOS, fp.NOMBREFORMAPAGO
ORDER BY INGRESOS DESC;

--Tasa de aceptación de ofertas por agente
SELECT a.CODIGO,
       SUM(CASE WHEN o.IDESTADOOFERTA=2 THEN 1 ELSE 0 END) AS ACEPTADAS,
       COUNT(*) AS TOTAL,
       ROUND(100 * SUM(CASE WHEN o.IDESTADOOFERTA=2 THEN 1 ELSE 0 END) / NULLIF(COUNT(*),0), 2) AS PCT_ACEPTACION
FROM OFERTAS o
JOIN PUBLICACIONES p ON p.IDPUBLICACION = o.IDPUBLICACION
JOIN AGENTES a ON a.IDAGENTE = p.IDAGENTE
WHERE o.FECHAHORA BETWEEN DATE '2025-10-09' AND DATE  '2025-10-18' --:StartDate AND :EndDate
  AND NVL(o.ELIMINADO,0)=0
GROUP BY a.CODIGO
ORDER BY PCT_ACEPTACION DESC;

-- Contraofertas: volumen y efectividad por agente
SELECT a.CODIGO,
       COUNT(*) AS CONTRAOFERTAS,
       SUM(CASE WHEN c.IDESTADOOFERTA=2 THEN 1 ELSE 0 END) AS ACEPTADAS,
       ROUND(100 * SUM(CASE WHEN c.IDESTADOOFERTA=2 THEN 1 ELSE 0 END) / NULLIF(COUNT(*),0), 2) AS PCT_ACEPTADAS
FROM CONTRAOFERTAS c
JOIN OFERTAS o ON o.IDOFERTA = c.IDOFERTA
JOIN PUBLICACIONES p ON p.IDPUBLICACION = o.IDPUBLICACION
JOIN AGENTES a ON a.IDAGENTE = p.IDAGENTE
WHERE c.FECHAHORA BETWEEN DATE '2025-10-09' AND DATE  '2025-10-13' --:StartDate AND :EndDate
  AND NVL(c.ELIMINADO,0)=0
GROUP BY a.CODIGO
ORDER BY CONTRAOFERTAS DESC;

-- Resultados por agente All Time
SELECT a.CODIGO,
       COUNT(DISTINCT p.IDPUBLICACION) AS PUBLICACIONES,
       COUNT(DISTINCT v.IDOFERTAACEPTADA) AS VENTAS,
       NVL(SUM(v.PRECIOFINAL),0) AS INGRESOS

FROM AGENTES a
LEFT JOIN PUBLICACIONES p ON p.IDAGENTE = a.IDAGENTE AND NVL(p.ELIMINADO,0)=0
LEFT JOIN OFERTAS o ON o.IDPUBLICACION = p.IDPUBLICACION AND NVL(o.ELIMINADO,0)=0
LEFT JOIN VENTAS v ON v.IDOFERTAACEPTADA = o.IDOFERTA AND NVL(v.ELIMINADO,0)=0
WHERE a.IDAGENTE = 3--(:IdAgente IS NULL OR a.IDAGENTE = :IdAgente)
 -- AND (:StartDate IS NULL OR p.FECHAPUBLICACION >= :StartDate)
  --AND (:EndDate   IS NULL OR p.FECHAPUBLICACION <  :EndDate)
GROUP BY a.CODIGO
ORDER BY INGRESOS DESC NULLS LAST;