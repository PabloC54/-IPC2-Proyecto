--PROCEDIMIENTOS
create proc Usuario_nuevo
@username varchar(20),
@nombres varchar(30),
@apellidos varchar(30),
@correo_electronico varchar(50),
@contrase�a varchar(30),
@fecha_nacimiento date,
@pais varchar(30)

AS
BEGIN

insert into Usuario values(
@username,
@nombres,
@apellidos,
@correo_electronico,
@contrase�a, 
@fecha_nacimiento,
@pais, 1)

END
