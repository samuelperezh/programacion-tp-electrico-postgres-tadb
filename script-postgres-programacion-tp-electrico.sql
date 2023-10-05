-- Script PostgreSQL
-- Curso de Tópicos Avanzados de base de datos - UPB 202320
-- Samuel Pérez Hurtado ID 000459067 - Luisa María Flórez Múnera ID 000449529

-- Proyecto: Gestión de Programación de Transporte Público Eléctrico
-- Motor de Base de datos: PostgreSQL 15.3

-- Crear el esquema de la base de datos
create database programacion_tp_electrico_db;

-- Crear el usuario con el que se realizarán las acciones con privilegios mínimos
create user programacion_tp_electrico_usr with encrypted password 'programacion_transporte';

-- Asignar privilegios al nuevo usuario solo en la base de datos creada
grant create, connect on database programacion_tp_electrico_db to programacion_tp_electrico_usr;
grant create on schema public to programacion_tp_electrico_usr;
grant select, insert, update, delete, trigger on all tables in schema public to programacion_tp_electrico_usr;

-- ********************************************
-- Creación de las tablas
-- ********************************************

-- Tabla: cargadores
create table cargadores (
    id int primary key generated always as identity,
    cargador varchar(10) not null
)

comment on table cargadores is 'Tabla que contiene los cargadores de los buses del sistema de transporte público eléctrico';
comment on column cargadores.id is 'Identificador del cargador';
comment on column cargadores.cargador is 'Nombre del cargador';

-- Tabla: autobuses
create table autobuses (
    id int primary key generated always as identity,
    autobus varchar(10) not null
)

comment on table autobuses is 'Tabla que contiene los autobuses del sistema de transporte público eléctrico';
comment on column autobuses.id is 'Identificador del autobus';
comment on column autobuses.autobus is 'Nombre del autobus';

-- Tabla: horarios
create table horarios (
    id int primary key not null,
    horario_pico boolean not null
)

comment on table horarios is 'Tabla que contiene los horarios de operación del sistema de transporte público eléctrico';
comment on column horarios.id is 'Identificador del horario';
comment on column horarios.horario_pico is 'Indica si el horario es pico o no';

-- Tabla: utilizacion_cargadores
create table utilizacion_cargadores (
    cargador_id int references cargadores(id) unique not null,
    autobus_id int references autobuses(id)unique not null,
    horario_id int references horarios(id)unique not null,
    primary key (cargador_id, autobus_id, horario_id)
)

comment on table utilizacion_cargadores is 'Tabla que contiene la utilización de los cargadores de los buses del sistema de transporte público eléctrico';
comment on column utilizacion_cargadores.cargador_id is 'Identificador del cargador';
comment on column utilizacion_cargadores.autobus_id is 'Identificador del autobus';
comment on column utilizacion_cargadores.horario_id is 'Identificador del horario';

-- Tabla: operacion_autobuses / estado_autobuses
create table operacion_autobuses (
    autobus_id int references autobuses(id) unique not null,
    horario_id int references horarios(id) unique not null,
    primary key (autobus_id, horario_id)
)

comment on table operacion_autobuses is 'Tabla que contiene la operación de los autobuses del sistema de transporte público eléctrico';
comment on column operacion_autobuses.autobus_id is 'Identificador del autobus';
comment on column operacion_autobuses.horario_id is 'Identificador del horario';

-- ********************************************
-- Creación de los procedimientos almacenados
-- ********************************************

-- ------------------------------------------------------
-- Procedimientos relacionados con la tabla cargadores
-- ------------------------------------------------------

-- Inserción: p_inserta_cargador
create or replace procedure p_inserta_cargador(
    p_cargador varchar(10)
)
language plpgsql
as $$
    begin
        insert into cargadores (cargador)
        values (p_cargador);
    end;
$$;

-- Actualización: p_actualiza_cargador
create or replace procedure p_actualiza_cargador(
    p_id int,
    p_cargador varchar(10)
)
language plpgsql
as $$
    begin
        update cargadores
        set cargador = p_cargador
        where id = p_id;
    end;
$$;

-- Eliminación: p_elimina_cargador
create or replace procedure p_elimina_cargador(
    p_id int
)
language plpgsql
as $$
    begin
        delete from cargadores
        where id = p_id;
    end;
$$;

-- ------------------------------------------------------
-- Procedimientos relacionados con la tabla autobuses
-- ------------------------------------------------------

-- Inserción: p_inserta_autobus
create or replace procedure p_inserta_autobus(
    p_autobus varchar(10)
)
language plpgsql
as $$
    begin
        insert into autobuses (autobus)
        values (p_autobus);
    end;
$$;

-- Actualización: p_actualiza_autobus
create or replace procedure p_actualiza_autobus(
    p_id int,
    p_autobus varchar(10)
)
language plpgsql
as $$
    begin
        update autobuses
        set autobus = p_autobus
        where id = p_id;
    end;
$$;

-- Eliminación: p_elimina_autobus
create or replace procedure p_elimina_autobus(
    p_id int
)
language plpgsql
as $$
    begin
        delete from autobuses
        where id = p_id;
    end;
$$;

-- ------------------------------------------------------
-- Procedimientos relacionados con la tabla horarios
-- ------------------------------------------------------

-- Inserción: p_inserta_horario
create or replace procedure p_inserta_horario(
    p_id int,
    p_horario_pico boolean
)
language plpgsql
as $$
    begin
        insert into horarios (id, horario_pico)
        values (p_id, p_horario_pico);
    end;
$$;

-- Actualización: p_actualiza_horario
create or replace procedure p_actualiza_horario(
    p_id int,
    p_horario_pico boolean
)
language plpgsql
as $$
    begin
        update horarios
        set horario_pico = p_horario_pico
        where id = p_id;
    end;
$$;

-- Eliminación: p_elimina_horario
create or replace procedure p_elimina_horario(
    p_id int
)
language plpgsql
as $$
    begin
        delete from horarios
        where id = p_id;
    end;
$$;

-- ------------------------------------------------------
-- Procedimientos relacionados con la tabla utilizacion_cargadores
-- ------------------------------------------------------

-- Inserción: p_inserta_utilizacion_cargador
create or replace procedure p_inserta_utilizacion_cargador(
    p_cargador_id int,
    p_autobus_id int,
    p_horario_id int
)
language plpgsql
as $$
    begin
        insert into utilizacion_cargadores (cargador_id, autobus_id, horario_id)
        values (p_cargador_id, p_autobus_id, p_horario_id);
    end;
$$;

-- Actualización: p_actualiza_utilizacion_cargador
create or replace procedure p_actualiza_utilizacion_cargador(
    p_cargador_id int,
    p_autobus_id int,
    p_horario_id int
)
language plpgsql
as $$
    begin
        update utilizacion_cargadores
        set horario_id = p_horario_id
        where cargador_id = p_cargador_id and autobus_id = p_autobus_id;
    end;
$$;

-- Eliminación: p_elimina_utilizacion_cargador
create or replace procedure p_elimina_utilizacion_cargador(
    p_horario_id int
)
language plpgsql
as $$
    begin
        delete from utilizacion_cargadores
        where horario_id = p_horario_id;
    end;
$$;

-- ------------------------------------------------------
-- Procedimientos relacionados con la tabla operacion_autobuses
-- ------------------------------------------------------

-- Inserción: p_inserta_operacion_autobus 
create or replace procedure core.p_inserta_operacion_autobus(
                    in p_autobus_id integer,
                    in p_horario_id integer)
    language plpgsql
as
$$
    declare
    l_autobus_operacion_id integer;

    begin
        -- Insertamos la operación de autobús
        insert into operacion_autobuses (autobus_id, horario_id)
        values (p_autobus_id, p_horario_id);

        -- Insertamos la indicación de que el autobús está operando en horario pico
        insert into horarios (id, horario_pico)
        values (p_horario_id, true);
    end;
$$;

-- Actualización: p_actualiza_operacion_autobus
create or replace procedure p_actualiza_operacion_autobus(
    p_autobus_id int,
    p_horario_id int
)
language plpgsql
as $$
    begin
        update operacion_autobuses
        set horario_id = p_horario_id
        where autobus_id = p_autobus_id;
    end;
$$;

-- Eliminación: p_elimina_operacion_autobus
create or replace procedure p_elimina_operacion_autobus(
    p_horario_id int
)
language plpgsql
as $$
    begin
        delete from operacion_autobuses
        where horario_id = p_horario_id;
    end;
$$;