



usando visual basic .net como lenguaje de programacion
usando mysql phpmyadmin user root,  pass vacio, base de datos minegocio,
utilizando buenas practicas
enseñame como crear paso a paso
crear una aplicacion de escritorio crud
indicandome paso a paso cuales seran los nombres de los archivos a crear y sus tipos, 
asi tambien en que directorios especificos debo crearlos
el crud debe trabajar con 2 windows form
1er windows form sera el menu principal que nos llevara a los  otros dos 
formularios
2do formulario productos enseñame cual es el mejor modelo o patron para que sea escalable a seguir 
el escrit de las tablas y como conectarme a la base de datos para llenar datagrids en el formulario

3er formulario clientes enseñame cual es el mejor modelo o patron para que sea escalable a seguir 
el escrit de las tablas y como conectarme a la base de datos para llenar datagrids en el formulario

enseñame si debo crear clases de las distinta entiedades de la base de datos
paso a paso  incluso como debo instalar o asegurarme de tener los drivers nugetts por consola
de como saber que podre conectarme con la base de datos en mysql

con una apariencia llamativa en los formularios

soy un principiante por eso necesito que sea detallado paso a paso cada punto







CREATE DATABASE minegocio;
USE minegocio;

CREATE TABLE productos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    precio DECIMAL(10, 2) NOT NULL,
    cantidad INT NOT NULL
);

INSERT INTO productos (nombre, precio, cantidad) VALUES 
('Laptop HP', 750.99, 5),
('Mouse Logitech', 25.50, 15),
('Teclado Mecánico Redragon', 45.75, 10),
('Monitor Samsung 24"', 180.99, 7),
('Disco Duro SSD 1TB', 120.00, 12);





CREATE TABLE clientes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    direccion VARCHAR(255) NOT NULL,
    telefono VARCHAR(20) NOT NULL
);

INSERT INTO clientes (nombre, direccion, telefono) VALUES 
('Carlos Pérez', 'Av. Central 123, Managua', '505-8888-1234'),
('Ana López', 'Calle 10 #45, León', '505-7777-5678'),
('José Martínez', 'Colonia Centro, Granada', '505-9999-8765'),
('María Fernández', 'Barrio San Juan, Masaya', '505-6666-3456'),
('Pedro Ramírez', 'Km 5 Carretera Norte, Managua', '505-5555-7890');






-- Insertar un producto
DELIMITER //
CREATE PROCEDURE InsertarProducto(
    IN p_nombre VARCHAR(100),
    IN p_precio DECIMAL(10,2),
    IN p_cantidad INT
)
BEGIN
    INSERT INTO productos (nombre, precio, cantidad) 
    VALUES (p_nombre, p_precio, p_cantidad);
END //
DELIMITER ;

-- Obtener todos los productos
DELIMITER //
CREATE PROCEDURE ObtenerProductos()
BEGIN
    SELECT * FROM productos;
END //
DELIMITER ;

-- Obtener un producto por ID
DELIMITER //
CREATE PROCEDURE ObtenerProductoPorID(IN p_id INT)
BEGIN
    SELECT * FROM productos WHERE id = p_id;
END //
DELIMITER ;

-- Actualizar un producto
DELIMITER //
CREATE PROCEDURE ActualizarProducto(
    IN p_id INT,
    IN p_nombre VARCHAR(100),
    IN p_precio DECIMAL(10,2),
    IN p_cantidad INT
)
BEGIN
    UPDATE productos 
    SET nombre = p_nombre, precio = p_precio, cantidad = p_cantidad 
    WHERE id = p_id;
END //
DELIMITER ;

-- Eliminar un producto
DELIMITER //
CREATE PROCEDURE EliminarProducto(IN p_id INT)
BEGIN
    DELETE FROM productos WHERE id = p_id;
END //
DELIMITER ;




-- Insertar un cliente
DELIMITER //
CREATE PROCEDURE InsertarCliente(
    IN c_nombre VARCHAR(100),
    IN c_direccion VARCHAR(255),
    IN c_telefono VARCHAR(20)
)
BEGIN
    INSERT INTO clientes (nombre, direccion, telefono) 
    VALUES (c_nombre, c_direccion, c_telefono);
END //
DELIMITER ;

-- Obtener todos los clientes
DELIMITER //
CREATE PROCEDURE ObtenerClientes()
BEGIN
    SELECT * FROM clientes;
END //
DELIMITER ;

-- Obtener un cliente por ID
DELIMITER //
CREATE PROCEDURE ObtenerClientePorID(IN c_id INT)
BEGIN
    SELECT * FROM clientes WHERE id = c_id;
END //
DELIMITER ;

-- Actualizar un cliente
DELIMITER //
CREATE PROCEDURE ActualizarCliente(
    IN c_id INT,
    IN c_nombre VARCHAR(100),
    IN c_direccion VARCHAR(255),
    IN c_telefono VARCHAR(20)
)
BEGIN
    UPDATE clientes 
    SET nombre = c_nombre, direccion = c_direccion, telefono = c_telefono 
    WHERE id = c_id;
END //
DELIMITER ;

-- Eliminar un cliente
DELIMITER //
CREATE PROCEDURE EliminarCliente(IN c_id INT)
BEGIN
    DELETE FROM clientes WHERE id = c_id;
END //
DELIMITER ;
