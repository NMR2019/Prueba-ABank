-- 1) Creacion de la base prueba_abank
CREATE DATABASE prueba_abank

-- 2) Secuencia para id (autogenerado)
CREATE SEQUENCE usuarios_id_seq
  START WITH 1
  INCREMENT BY 1
  NO MINVALUE
  NO MAXVALUE
  CACHE 1;

-- 3) Creacion de la Tabla Usuarios
CREATE TABLE Usuarios (
  id integer NOT NULL DEFAULT nextval('usuarios_id_seq'),
  nombres varchar(100) NOT NULL,
  apellidos varchar(100) NOT NULL,
  fecha_nacimiento date NOT NULL,
  direccion text NOT NULL,
  password varchar(120) NOT NULL,
  telefono varchar(20) NOT NULL,
  email varchar(255) NOT NULL,
  estado varchar(1) NOT NULL DEFAULT 'A',         
  fecha_creacion timestamptz NOT NULL,
  fecha_modificacion timestamptz,
  CONSTRAINT usuarios_pkey PRIMARY KEY (id),
  CONSTRAINT usuarios_estado_chk CHECK (estado IN ('A','I'))
);

-- 4) Función trigger para llenar/actualizar fechas
CREATE OR REPLACE FUNCTION usuarios_set_timestamps()
RETURNS TRIGGER AS
$$
BEGIN
  IF (TG_OP = 'INSERT') THEN
    NEW.fecha_creacion := COALESCE(NEW.fecha_creacion, now());
    NEW.fecha_modificacion := COALESCE(NEW.fecha_modificacion, now());
    RETURN NEW;
  ELSIF (TG_OP = 'UPDATE') THEN
    NEW.fecha_modificacion := now();
    RETURN NEW;
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- 5) Trigger que llama a la función
CREATE TRIGGER trg_usuarios_timestamps
BEFORE INSERT OR UPDATE ON Usuarios
FOR EACH ROW
EXECUTE FUNCTION usuarios_set_timestamps();




select * from Usuarios