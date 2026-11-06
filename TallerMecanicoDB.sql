
CREATE DATABASE TallerMecanicoDB;
GO

USE TallerMecanicoDB;
GO

-- Tabla de clientes
CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(60) NOT NULL,
    Apellido NVARCHAR(60) NOT NULL,
    Telefono NVARCHAR(15) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE
);
GO

-- Tabla de vehículos
CREATE TABLE Vehiculos (
    IdVehiculo INT IDENTITY(1,1) PRIMARY KEY,
    Placa NVARCHAR(10) NOT NULL UNIQUE,
    Marca NVARCHAR(50) NOT NULL,
    Modelo NVARCHAR(50) NOT NULL,
    Anio INT NOT NULL,
    IdCliente INT NOT NULL,
    CONSTRAINT FK_Vehiculos_Clientes
        FOREIGN KEY (IdCliente)
        REFERENCES Clientes(IdCliente)
);
GO

-- Tabla de órdenes de servicio
CREATE TABLE OrdenesServicio (
    IdOrden INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME2 NOT NULL,
    Descripcion NVARCHAR(200) NOT NULL,
    Estado NVARCHAR(30) NOT NULL,
    Costo DECIMAL(10,2) NOT NULL,
    IdVehiculo INT NOT NULL,
    CONSTRAINT FK_Ordenes_Vehiculos
        FOREIGN KEY (IdVehiculo)
        REFERENCES Vehiculos(IdVehiculo)
);
GO

-- Datos de ejemplo
INSERT INTO Clientes (Nombre, Apellido, Telefono, Correo)
VALUES
('Carlos', 'Perez', '987654321', 'carlos@gmail.com'),
('Maria', 'Lopez', '912345678', 'maria@gmail.com'),
('Juan', 'Torres', '998877665', 'juan@gmail.com');
GO

INSERT INTO Vehiculos (Placa, Marca, Modelo, Anio, IdCliente)
VALUES
('ABC-123', 'Toyota', 'Corolla', 2022, 1),
('XYZ-456', 'Hyundai', 'Accent', 2021, 1),
('DEF-789', 'Kia', 'Rio', 2023, 2);
GO

INSERT INTO OrdenesServicio
(Fecha, Descripcion, Estado, Costo, IdVehiculo)
VALUES
('2026-11-01', 'Cambio de aceite', 'Completado', 150.00, 1),
('2026-11-02', 'Revision de frenos', 'Pendiente', 250.00, 1),
('2026-11-03', 'Mantenimiento general', 'En proceso', 350.00, 2),
('2026-11-04', 'Cambio de neumaticos', 'Pendiente', 500.00, 3);
GO

-- Verificar registros
SELECT * FROM Clientes;
SELECT * FROM Vehiculos;
SELECT * FROM OrdenesServicio;
GO
