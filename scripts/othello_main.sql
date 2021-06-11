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
idPartida int primary key not null identity(1,1),
modalidad varchar(10) not null,
resultado varchar(10) not null,
username varchar(20) not null,
idRonda VARCHAR(22))

create table Equipo(
nombre_equipo varchar(10) primary key not null,
username varchar(20) not null,
username2 varchar(20) not null,
username3 varchar(20) not null)

create table Registro_Campeonato(
nombre_equipo varchar(10) not null,
nombre_campeonato varchar(20) not null,
resultado varchar(10) not null,
puntos int not null)

create table Campeonato(
nombre_campeonato varchar(20) primary key not null,
cantidad_equipos int not null)

create table Ronda(
idRonda VARCHAR(22) primary key not null,
numero_ronda int not null,
cantidad_equipos int not null,
nombre_campeonato varchar(20) not null)


--CREACIÓN DE LAS LLAVES FORANEAS
alter table Equipo add foreign key (username) references Usuario(username)
alter table Equipo add foreign key (username2) references Usuario(username)
alter table Equipo add foreign key (username3) references Usuario(username)

alter table Partida add foreign key (username) references Usuario(username)
alter table Partida add foreign key (idRonda) references Ronda(idRonda);

alter table Registro_Campeonato add foreign key (nombre_equipo) references Equipo(nombre_equipo)
alter table Registro_Campeonato add foreign key (nombre_campeonato) references Campeonato(nombre_campeonato)

alter table Ronda add foreign key (nombre_campeonato) references Campeonato(nombre_campeonato)


--CONSULTAS
select * from Usuario
select * from Partida
select * from Equipo
select * from Registro_Campeonato
select * from Campeonato
select * from Ronda




delete from Partida
delete from Registro_Campeonato
delete from Campeonato
delete from Ronda

insert into Partida(modalidad, resultado, username) values ('normal', 'derrota','user')

