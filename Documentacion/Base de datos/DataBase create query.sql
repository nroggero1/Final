CREATE TABLE Provincia 
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Nombre nvarchar(50) NOT NULL
)
GO 

CREATE TABLE Localidad
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Nombre nvarchar(50) NOT NULL,
	IdProvincia int NOT NULL REFERENCES Provincia(Id),
	CodigoPostal smallint NOT NULL
)
GO

CREATE TABLE Proveedor
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CodigoTributario bigint NOT NULL,
	Direccion nvarchar(50) NOT NULL,
	IdLocalidad int NOT NULL REFERENCES Localidad(Id),
	IdProvincia int NOT NULL REFERENCES Provincia(Id),
	Telefono nvarchar(20), 
	Mail NVARCHAR(50),
	Denominacion NVARCHAR(100) NOT NULL,
	FechaAlta datetime NOT NULL,
	Activo bit NOT NULL
)
GO

CREATE TABLE Cliente
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	CodigoTributario bigint NOT NULL,
	Direccion nvarchar(50) NOT NULL,
	IdLocalidad int NOT NULL REFERENCES Localidad(Id),
	IdProvincia int NOT NULL REFERENCES Provincia(Id),
	Telefono nvarchar(20), 
	Mail NVARCHAR(50),
	Denominacion NVARCHAR(100),
	FechaAlta datetime NOT NULL,
	Activo bit NOT NULL
)
GO

CREATE TABLE Marca
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Nombre nvarchar(50) NOT NULL,
	Activo bit NOT NULL,
	FechaAlta datetime NOT NULL
)
GO

CREATE TABLE Categoria
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Nombre nvarchar(50) NOT NULL,
	FechaAlta datetime NOT NULL,
	Activo bit NOT NULL,
) 
GO

CREATE TABLE Producto
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Nombre nvarchar(50) NOT NULL,
	Descripcion nvarchar(50) NOT NULL,
	CodigoBarras nvarchar(30) NOT NULL,
	IdCategoria int NOT NULL REFERENCES Categoria(Id),
	IdMarca int NOT NULL REFERENCES Marca(Id),
	PrecioCompra decimal(12, 2) NOT NULL,
	PorcentajeGanancia int NOT NULL,
	PrecioVentaSugerido decimal(12, 2) NOT NULL,
	PrecioVenta decimal(12, 2) NOT NULL,
	Stock int NOT NULL,
	StockMinimo int NOT NULL,
	Activo bit NOT NULL,
	FechaAlta datetime NOT NULL
)
GO

CREATE TABLE Usuario
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NombreUsuario varchar(50) NOT NULL,
	Nombre varchar(50) NOT NULL,
	Apellido varchar(50) NOT NULL,
	Mail NVARCHAR(50),
	FechaNacimiento [datetime] NOT NULL,
	Clave varchar(50) NOT NULL,
	Administrador bit NOT NULL,
	FechaAlta datetime NOT NULL,
	Activo bit NOT NULL
) 
GO

CREATE TABLE Compra
(	
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Fecha datetime,
	IdUsuario int REFERENCES Usuario(Id),
	IdProveedor int REFERENCES Proveedor(Id),
	Importe DECIMAL(10,2) NOT NULL
) 
GO

CREATE TABLE DetalleCompra
(	
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	IdCompra int REFERENCES Compra(Id),
	IdProducto int REFERENCES Producto(Id),
	Cantidad int NOT NULL,
	PrecioUnitario DECIMAL(10,2) NOT NULL,
) 
GO

CREATE TABLE Venta
(	
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Fecha datetime,
	IdUsuario int REFERENCES Usuario(Id),
	IdCliente int REFERENCES Cliente(Id),
	Importe DECIMAL(10,2) NOT NULL
) 
GO

CREATE TABLE DetalleVenta
(	
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	IdVenta int REFERENCES Venta(Id),
	IdProducto int REFERENCES Producto(Id),
	Cantidad int NOT NULL,
	PrecioUnitario DECIMAL(10,2) NOT NULL,
) 
GO

