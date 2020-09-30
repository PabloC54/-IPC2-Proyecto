--CREACIÓN DE LA BASE DE DATOS
create database [Othello_db]
use [Othello_db]


--CREACIÓN DE LAS TABLAS Y LLAVES PRIMARIAS
create table Usuario(
username varchar(20) primary key not null,
nombres varchar(30) not null,
apellidos varchar(30) not null,
correo_electronico varchar(50) not null,
contraseña varchar(30) not null,
fecha_nacimiento date not null,
pais varchar(30) not null,
habilitado bit not null)

create table Partida(
idPartida varchar(10) primary key not null,
privacidad varchar(10) not null,
movimientos int not null,
usuario_anfitrion varchar(20) not null,
usuario_invitado varchar(20),
numero_ronda int)

create table Torneo(
nombre_torneo varchar(10) primary key not null,
cantidad_jugadores int not null,
usuario_anfitrion varchar(20) not null,
registro_torneo varchar(10) not null)

create table Registro_Torneo(
usuario varchar(20) not null,
nombre_torneo varchar(10) not null)

create table Ronda(
numero_ronda int primary key not null,
jugadores_restantes int not null,
nombre_torneo varchar(10) not null)

create table Resultado_Partida(
usuario_ganador varchar(20) not null,
reporte varchar(200) not null)


--CREACIÓN DE LAS LLAVES FORANEAS
alter table Partida add foreign key (usuario_anfitrion) references Usuario(username)

alter table Partida add foreign key (usuario_invitado) references Usuario(username)

alter table Partida add foreign key (numero_ronda) references Ronda(numero_ronda)

alter table Registro_Torneo add foreign key (usuario) references Usuario(username)

alter table Registro_Torneo add foreign key (nombre_torneo) references Torneo(nombre_torneo)

alter table Ronda add foreign key (nombre_torneo) references Torneo(nombre_torneo)


--CONSULTAS
select * from Usuario


