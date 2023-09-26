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