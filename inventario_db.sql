-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS inventario_db
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE inventario_db;

-- ========================================
-- Tabla: producto
-- ========================================
CREATE TABLE producto (
  id_producto INT NOT NULL AUTO_INCREMENT,
  descripcion VARCHAR(255),
  existencia INT,
  precio DOUBLE,
  PRIMARY KEY (id_producto)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Datos iniciales para producto
INSERT INTO producto (id_producto, descripcion, existencia, precio) VALUES
(5, 'Cinturón Negro XXL', 100, 15.25),
(6, 'Escalera XXL', 30, 200.50),
(8, 'Caldero imperial Dinastía Kang L', 20, 450.28),
(9, 'Corbata negra', 500, 10.00),
(10, 'Playera Negra XXL', 450, 15.60),
(12, 'Smartphone Redmi Note 6', 100, 250.99),
(18, 'Pantalón Jean XXL', 20, 15.50),
(19, 'Pantalón Jean XXL', 20, 15.50),
(38, 'Latigo negro', 21, 15.00),
(39, 'Cafetera electrica', 100, 20.00);

-- ========================================
-- Tabla: usuario
-- ========================================
CREATE TABLE usuario (
  id_usuario INT NOT NULL AUTO_INCREMENT,
  usuario VARCHAR(255),
  estado VARCHAR(1),
  contrasenia VARCHAR(255),
  PRIMARY KEY (id_usuario)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Datos iniciales para usuario
INSERT INTO usuario (id_usuario, usuario, estado, contrasenia) VALUES
(1, 'admin', 'A', '$2a$11$W4DsEcLS4N1nxNZULDr84eGRjPB0f/GBskbIkz9qudU21/pY0JNJC');
