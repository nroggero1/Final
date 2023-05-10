--Insertar una marca
CREATE PROCEDURE CrearMarca
    @Nombre nvarchar(50)
AS
BEGIN
    INSERT INTO Marca (Nombre, Activo, FechaAlta)
    VALUES (@Nombre, 1, GETDATE());
END

--Editar una marca
CREATE PROCEDURE EditarMarca
    @Id int,
    @Nombre nvarchar(50)
AS
BEGIN
    UPDATE Marca
    SET Nombre = @Nombre
    WHERE Id = @Id;
END

--Inactivar una marca
CREATE PROCEDURE EliminarMarca
    @Id int
AS
BEGIN
    UPDATE Marca
    SET Activo = 0
    WHERE Id = @Id;
END


--Insertar una categoria
CREATE PROCEDURE CrearCategoria
    @Nombre nvarchar(50)
AS
BEGIN
    INSERT INTO Marca (Nombre, Activo, FechaAlta)
    VALUES (@Nombre, 1, GETDATE());
END

--Editar una categoria
CREATE PROCEDURE EditarCategoria
    @Id int,
    @Nombre nvarchar(50)
AS
BEGIN
    UPDATE Categoria
    SET Nombre = @Nombre
    WHERE Id = @Id;
END

--Inactivar una categoria
CREATE PROCEDURE EliminarCategoria
    @Id int
AS
BEGIN
    UPDATE Categoria
    SET Activo = 0
    WHERE Id = @Id;
END