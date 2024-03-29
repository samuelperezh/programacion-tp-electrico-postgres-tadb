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
    nombre_cargador varchar(20) not null
);

comment on table cargadores is 'Tabla que contiene los cargadores de los buses del sistema de transporte público eléctrico';
comment on column cargadores.id is 'Identificador del cargador';
comment on column cargadores.nombre_cargador is 'Nombre del cargador';

-- Tabla: autobuses
create table autobuses (
    id int primary key generated always as identity,
    nombre_autobus varchar(20) not null
);

comment on table autobuses is 'Tabla que contiene los autobuses del sistema de transporte público eléctrico';
comment on column autobuses.id is 'Identificador del autobus';
comment on column autobuses.nombre_autobus is 'Nombre del autobus';

-- Tabla: horarios
create table horarios (
    id int primary key not null,
    horario_pico boolean not null
);

comment on table horarios is 'Tabla que contiene los horarios de operación del sistema de transporte público eléctrico';
comment on column horarios.id is 'Identificador del horario';
comment on column horarios.horario_pico is 'Indica si el horario es pico o no';

-- Tabla: utilizacion_cargadores
create table utilizacion_cargadores (
    cargador_id int references cargadores(id) not null,
    autobus_id int references autobuses(id) not null,
    horario_id int references horarios(id) not null,
    primary key (cargador_id, horario_id)
);

comment on table utilizacion_cargadores is 'Tabla que contiene la utilización de los cargadores de los buses del sistema de transporte público eléctrico';
comment on column utilizacion_cargadores.cargador_id is 'Identificador del cargador';
comment on column utilizacion_cargadores.autobus_id is 'Identificador del autobus';
comment on column utilizacion_cargadores.horario_id is 'Identificador del horario';

-- Tabla: operacion_autobuses
create table operacion_autobuses (
    autobus_id int references autobuses(id) not null,
    horario_id int references horarios(id) not null,
    primary key (autobus_id, horario_id)
);

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
    p_nombre varchar(10)
)
language plpgsql
as $$
    begin
        insert into cargadores (nombre_cargador)
        values (p_nombre);
    end;
$$;

-- Actualización: p_actualiza_cargador
create or replace procedure p_actualiza_cargador(
    p_id int,
    p_nombre varchar(10)
)-+
language plpgsql
as $$
    begin
        update cargadores
        set nombre_cargador = p_nombre
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
-- Cada vez que se inserte un autobus, se debe poner a operar en horario pico
create or replace procedure p_inserta_autobus(
    p_nombre varchar(10)
)
language plpgsql
as
$$
declare
    l_autobus_id integer;
begin
    insert into autobuses (nombre_autobus)
    values (p_nombre)
    returning id into l_autobus_id;

    insert into operacion_autobuses (autobus_id, horario_id)
    select l_autobus_id, id
    from horarios
    where horario_pico = true;
end;
$$;

-- Actualización: p_actualiza_autobus
create or replace procedure p_actualiza_autobus(
    p_id int,
    p_nombre varchar(10)
)
language plpgsql
as $$
    begin
        update autobuses
        set nombre_autobus = p_nombre
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
    p_cargador_id int,
    p_autobus_id int,
    p_horario_id int
)
language plpgsql
as $$
    begin
        delete from utilizacion_cargadores
        where cargador_id=p_cargador_id and autobus_id = p_autobus_id and horario_id = p_horario_id;
    end;
$$;

-- ------------------------------------------------------
-- Procedimientos relacionados con la tabla operacion_autobuses
-- ------------------------------------------------------

-- Inserción: p_inserta_operacion_autobus 
create or replace procedure p_inserta_operacion_autobus(
    p_autobus_id int,
    p_horario_id int
)
language plpgsql
as $$
    begin
        insert into operacion_autobuses (autobus_id, horario_id)
        values (p_autobus_id, p_horario_id);
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
    p_autobus_id int,
    p_horario_id int
)
language plpgsql
as $$
    begin
        delete from operacion_autobuses
        where autobus_id = p_autobus_id and horario_id = p_horario_id;
    end;
$$;

-- -----------------------------------------------------------
-- Funciones para consultar el porcentaje de la disponibilidad 
-- -----------------------------------------------------------

create or replace function f_porcentaje_cargadores_utilizados(p_horario_id int) returns numeric
language plpgsql
as $$
declare
    l_cargadores_usados float;
    l_total_cargadores float;
begin
    select count(distinct cargador_id) into l_cargadores_usados
    from utilizacion_cargadores
    where horario_id = p_horario_id;

    select count(id) into l_total_cargadores
    from cargadores;

    return l_cargadores_usados / l_total_cargadores * 100;
end;
$$;

create or replace function f_porcentaje_autobuses_operacion(p_horario_id int) returns numeric
language plpgsql
as $$
declare
    l_autobuses_operacion float;
    l_total_autobuses float;
begin
    select count(distinct autobus_id) into l_autobuses_operacion
    from operacion_autobuses
    where horario_id = p_horario_id;

    select count(id) into l_total_autobuses
    from autobuses;

    return l_autobuses_operacion / l_total_autobuses * 100;
end;
$$;

-- -----------------------------------------------------------
-- Vistas para mejorar la visualización de los datos
-- -----------------------------------------------------------

create or replace view v_porcentajes as (
    select
        h.id as hora,
        h.horario_pico as horario_pico,
        f_porcentaje_cargadores_utilizados(h.id) as porcentaje_cargadores_utilizados,
        f_porcentaje_autobuses_operacion(h.id) as porcentaje_autobuses_operacion
    from horarios h
);

create or replace view v_total_operacion_autobuses as (
    select
        h.id as hora,
        (select count(distinct autobus_id) from operacion_autobuses where horario_id = h.id) total_operacion_autobuses
    from horarios h
);

create or replace view v_total_utilizacion_cargadores as (
    select
        h.id as hora,
        (select count(distinct cargador_id) from utilizacion_cargadores where horario_id = h.id) total_utilizacion_cargadores
    from horarios h
);

-----------------------------------------------------------------------------
--Función sobre el mapeo del estado de un autobus en un horario específico,
-- El autobus tendrá 3 estados diferentes: cargando, operando y parqueado
-----------------------------------------------------------------------------
create or replace function f_estado_autobus(p_horario_id int, p_autobus_id int) returns table (estado varchar(10))
language plpgsql
as $$
declare
    l_estado varchar(10);
begin
    select case
        when (select count(distinct cargador_id) from utilizacion_cargadores where horario_id = p_horario_id and autobus_id = p_autobus_id) > 0 then 'Cargando'
        when (select count(distinct autobus_id) from operacion_autobuses where horario_id = p_horario_id and autobus_id = p_autobus_id) > 0 then 'Operando'
        else 'Parqueado'
    end into l_estado;

    return query
        select l_estado;
end;
$$;