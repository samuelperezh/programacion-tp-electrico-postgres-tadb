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

-- ***********************
-- Creación de las tablas
-- ***********************

-- Tabla: horarios
create table horarios (
    id int primary key not null,
    horario_pico boolean not null
)

-- Tabla: cargadores
create table cargadores (
    id int primary key generated always as identity,
    cargador varchar(10) not null
)

-- Tabla: autobuses
create table autobuses (
    id int primary key generated always as identity,
    autobus varchar(10) not null
)

-- Tabla: utilizacion_cargadores
create table utilizacion_cargadores (
    cargador_id int references cargadores(id) unique not null,
    autobus_id int references autobuses(id)unique not null,
    horario_id int references horarios(id)unique not null,
    primary key (cargador_id, autobus_id, horario_id)
)

-- Tabla: operacion_autobuses / estado_autobuses
create table operacion_autobuses (
    autobus_id int references autobuses(id) unique not null,
    horario_id int references horarios(id) unique not null,
    primary key (autobus_id, horario_id)
)

