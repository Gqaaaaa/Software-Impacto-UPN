CREATE DATABASE SistemaVentasFinal;
GO

USE SistemaVentasFinal;
GO

CREATE TABLE Categoria (
  id VARCHAR(100) PRIMARY KEY,
  descripcion VARCHAR(100) NOT NULL
);

CREATE TABLE Cliente (
  id VARCHAR(100) PRIMARY KEY,
  nombres VARCHAR(100),
  apellidos VARCHAR(100),
  dni CHAR(8) UNIQUE,
  telefono VARCHAR(15),
  direccion VARCHAR(150)
);

CREATE TABLE Proveedor (
  id VARCHAR(100) PRIMARY KEY,
  razonSocial VARCHAR(150),
  ruc CHAR(11) UNIQUE,
  telefono VARCHAR(15),
  direccion VARCHAR(150)
);

CREATE TABLE Empleado (
  id VARCHAR(100) PRIMARY KEY,
  nombres VARCHAR(100),
  apellidos VARCHAR(100),
  dni CHAR(8) UNIQUE,
  telefono VARCHAR(15),
  direccion VARCHAR(150)
);

CREATE TABLE Usuario (
  id VARCHAR(100) PRIMARY KEY,
  usuario VARCHAR(50) UNIQUE,
  contrasena VARCHAR(100),
  estado BIT,
  idEmpleado VARCHAR(100),
  FOREIGN KEY (idEmpleado) REFERENCES Empleado(id)
);

CREATE TABLE Rol (
  id VARCHAR(100) PRIMARY KEY,
  nombre VARCHAR(50) UNIQUE
);

CREATE TABLE UsuarioRol (
  idUsuario VARCHAR(100),
  idRol VARCHAR(100),
  estado BIT NOT NULL DEFAULT 0,
  PRIMARY KEY (idUsuario, idRol),
  FOREIGN KEY (idUsuario) REFERENCES Usuario(id),
  FOREIGN KEY (idRol) REFERENCES Rol(id)
);

CREATE TABLE Producto (
  id VARCHAR(100) PRIMARY KEY,
  nombre VARCHAR(100),
  descripcion VARCHAR(150),
  precio_compra DECIMAL(10,2),
  precio_venta DECIMAL(10,2),
  stock INT,
  idCategoria VARCHAR(100),
  FOREIGN KEY (idCategoria) REFERENCES Categoria(id)
);

CREATE TABLE Compra (
  id VARCHAR(100) PRIMARY KEY,
  fecha DATE,
  tipo_documento VARCHAR(20),
  num_documento VARCHAR(20),
  igv DECIMAL(10,2),
  estado VARCHAR(20),
  idProveedor VARCHAR(100),
  idUsuario VARCHAR(100),
  FOREIGN KEY (idProveedor) REFERENCES Proveedor(id),
  FOREIGN KEY (idUsuario) REFERENCES Usuario(id)
);

CREATE TABLE Detalle_Compra (
  id VARCHAR(100) PRIMARY KEY,
  idCompra VARCHAR(100),
  idProducto VARCHAR(100),
  cantidad INT,
  precio DECIMAL(10,2),
  FOREIGN KEY (idCompra) REFERENCES Compra(id),
  FOREIGN KEY (idProducto) REFERENCES Producto(id)
);

CREATE TABLE Venta (
  id VARCHAR(100) PRIMARY KEY,
  fecha DATE,
  tipo_documento VARCHAR(20),
  num_documento VARCHAR(20),
  igv DECIMAL(10,2),
  estado VARCHAR(20),
  idCliente VARCHAR(100),
  idUsuario VARCHAR(100),
  FOREIGN KEY (idCliente) REFERENCES Cliente(id),
  FOREIGN KEY (idUsuario) REFERENCES Usuario(id)
);

CREATE TABLE Detalle_Venta (
  id VARCHAR(100) PRIMARY KEY,
  idVenta VARCHAR(100),
  idProducto VARCHAR(100),
  cantidad INT,
  precio DECIMAL(10,2),
  FOREIGN KEY (idVenta) REFERENCES Venta(id),
  FOREIGN KEY (idProducto) REFERENCES Producto(id)
);