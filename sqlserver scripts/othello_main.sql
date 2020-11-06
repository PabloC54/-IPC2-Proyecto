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

create table Estadisticas(
username varchar(20) not null,
partidas_ganadas int not null,
partidas_perdidas int not null,
partidas_empatadas int not null,
campeonatos int not null,
campeonatos_ganados int not null,
campeonatos_puntos int not null)

create table Partida(
idPartida varchar(10) primary key not null,
modalidad varchar(10) not null,
movimientos int not null,
usuario_anfitrion varchar(20) not null,
numero_ronda int)

create table Campeonato(
nombre varchar(10) primary key not null,
cantidad_equipos int not null)

create table Registro_Campeonato(
usuario varchar(20) not null,
nombre varchar(10) not null)

create table Ronda(
numero_ronda int primary key not null,
jugadores_restantes int not null,
nombre varchar(10) not null)



--CREACIÓN DE LAS LLAVES FORANEAS
alter table Estadisticas add foreign key (username) references Usuario(username)

alter table Partida add foreign key (usuario_anfitrion) references Usuario(username)

alter table Partida add foreign key (numero_ronda) references Ronda(numero_ronda)

alter table Registro_Campeonato add foreign key (usuario) references Usuario(username)

alter table Registro_Campeonato add foreign key (nombre) references Campeonato(nombre)

alter table Ronda add foreign key (nombre) references Campeonato(nombre)


--CONSULTAS
select * from Usuario
select * from Estadisticas

delete from Estadisticas
delete from Usuario


update Reporte set partidas_ganadas=0, partidas_perdidas=0, partidas_empatadas=0, campeonatos=0, campeonatos_ganados=0, campeonatos_puntos=0
