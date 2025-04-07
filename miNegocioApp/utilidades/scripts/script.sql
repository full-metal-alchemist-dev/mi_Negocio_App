SELECT p.id, p.nombre, p.precio,p.cantidad, ptipo.descripcion, pcate.descripcion FROM productos as p INNER JOIN productotipo AS ptipo ON ptipo.id = p.productotipoid INNER JOIN productocategoria AS pcate ON pcate.id = p.productocategoriaid;

DELIMITER $$

-- Drop the procedure if it already exists
DROP PROCEDURE IF EXISTS ObtenerProductosDetalles$$

CREATE PROCEDURE ObtenerProductosDetalles()
BEGIN
    SELECT 
        p.id, 
        p.nombre, 
        p.precio, 
        p.cantidad, 
        ptipo.descripcion AS tipo, 
        pcate.descripcion AS categoria
    FROM productos AS p
    INNER JOIN productotipo AS ptipo ON ptipo.id = p.productotipoid
    INNER JOIN productocategoria AS pcate ON pcate.id = p.productocategoriaid;
END$$

DELIMITER ;





-- Crear procedimiento almacenado para obtener categorías por tipo
DELIMITER $$

-- Drop the procedure if it already exists
DROP PROCEDURE IF EXISTS ObtenerProductoCategoriasPorTipo$$

CREATE PROCEDURE ObtenerProductoCategoriasPorTipo(IN tipoId INT)
BEGIN
    SELECT id, descripcion
    FROM productocategoria
    WHERE productotipoId = tipoId;
END $$

DELIMITER ;





CREATE TABLE pedidos (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    estado INT NOT NULL,                -- 1 = Pendiente, 2 = Pagado
    id_mesa INT NOT NULL,
    id_estado_servicio INT NOT NULL,    -- 1 = No servicio, 2 = Con servicio
    id_estado_delivery INT NOT NULL,    -- 1 = No Delivery, 2 = Con Delivery
    fecha DATE NOT NULL,
    hora TIME NOT NULL,
    id_mesero INT DEFAULT 1,            -- Valor por defecto 1
    FOREIGN KEY (id_mesa) REFERENCES mesas(id),           -- Asegúrate de que la tabla 'mesas' exista
    FOREIGN KEY (id_estado_servicio) REFERENCES estados_servicio(id), -- Asegúrate de que esta tabla exista
    FOREIGN KEY (id_estado_delivery) REFERENCES estados_delivery(id)  -- Asegúrate de que esta tabla exista
);




cuales son los procedimientos almacenados para crud de   

CREATE TABLE pedidos (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    estado INT NOT NULL,                -- 1 = Pendiente, 2 = Pagado
    id_mesa INT NOT NULL,
    id_estado_servicio INT NOT NULL,    -- 1 = Sin servicio, 2 = Con servicio
    id_estado_delivery INT NOT NULL,    -- 1 = Sin Delivery, 2 = Con Delivery
    fecha DATE NOT NULL,
    hora TIME NOT NULL,
    id_mesero INT DEFAULT 1,            -- Valor por defecto 1
    FOREIGN KEY (id_mesa) REFERENCES mesas(id),           -- Asegúrate de que la tabla 'mesas' exista
    FOREIGN KEY (id_estado_servicio) REFERENCES estados_servicio(id), -- Asegúrate de que esta tabla exista
    FOREIGN KEY (id_estado_delivery) REFERENCES estados_delivery(id)  -- Asegúrate de que esta tabla exista
);



DELIMITER $$

-- CREACIÓN DE LAS TABLAS

-- Tabla Pedidos
CREATE TABLE pedidos (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    estado INT NOT NULL,                -- 1 = Pendiente, 2 = Pagado
    id_mesa INT NOT NULL,
    id_estado_servicio INT NOT NULL,    -- 1 = Sin servicio, 2 = Con servicio
    id_estado_delivery INT NOT NULL,    -- 1 = Sin Delivery, 2 = Con Delivery
    fecha DATE NOT NULL,
    hora TIME NOT NULL,
    id_mesero INT DEFAULT 1,            -- Valor por defecto 1
    FOREIGN KEY (id_mesa) REFERENCES mesas(id),           -- Asegúrate de que la tabla 'mesas' exista
    FOREIGN KEY (id_estado_servicio) REFERENCES estados_servicio(id), -- Asegúrate de que esta tabla exista
    FOREIGN KEY (id_estado_delivery) REFERENCES estados_delivery(id)  -- Asegúrate de que esta tabla exista
);

-- Tabla PedidosDetalles
CREATE TABLE pedidosDetalles (
    id INT AUTO_INCREMENT PRIMARY KEY, 
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (id_pedido) REFERENCES pedidos(id),        -- Relación con la tabla 'pedidos'
    FOREIGN KEY (id_producto) REFERENCES productos(id)     -- Asegúrate de que la tabla 'productos' exista
);

-- PROCEDIMIENTOS ALMACENADOS

-- 1. PROCEDIMIENTO PARA INSERTAR UN NUEVO PEDIDO

CREATE PROCEDURE InsertarPedido(
    IN p_estado INT,
    IN p_id_mesa INT,
    IN p_id_estado_servicio INT,
    IN p_id_estado_delivery INT,
    IN p_fecha DATE,
    IN p_hora TIME,
    IN p_id_mesero INT,
    OUT p_id_pedido INT
)
BEGIN
    -- Insertar un nuevo pedido en la tabla 'pedidos'
    INSERT INTO pedidos (estado, id_mesa, id_estado_servicio, id_estado_delivery, fecha, hora, id_mesero)
    VALUES (p_estado, p_id_mesa, p_id_estado_servicio, p_id_estado_delivery, p_fecha, p_hora, p_id_mesero);
    
    -- Obtener el ID del nuevo pedido insertado
    SET p_id_pedido = LAST_INSERT_ID();
END $$

-- 2. PROCEDIMIENTO PARA INSERTAR LOS DETALLES DEL PEDIDO (PEDIDOSDETALLES)

CREATE PROCEDURE InsertarPedidoDetalles(
    IN p_id_pedido INT,
    IN p_id_producto INT,
    IN p_cantidad INT,
    IN p_precio DECIMAL(10,2),
    OUT p_subtotal DECIMAL(10,2)
)
BEGIN
    -- Calcular el subtotal
    SET p_subtotal = p_precio * p_cantidad;
    
    -- Insertar un nuevo detalle en la tabla 'pedidosDetalles'
    INSERT INTO pedidosDetalles (id_pedido, id_producto, cantidad, precio, subtotal)
    VALUES (p_id_pedido, p_id_producto, p_cantidad, p_precio, p_subtotal);
END $$

-- 3. PROCEDIMIENTO PARA OBTENER PEDIDOS POR ID MESA

CREATE PROCEDURE ObtenerPedidosPorMesa(
    IN p_id_mesa INT
)
BEGIN
    SELECT * FROM pedidos WHERE id_mesa = p_id_mesa;
END $$

-- 4. PROCEDIMIENTO PARA OBTENER DETALLES DE UN PEDIDO POR ID PEDIDO

CREATE PROCEDURE ObtenerDetallesPorPedido(
    IN p_id_pedido INT
)
BEGIN
    SELECT * FROM pedidosDetalles WHERE id_pedido = p_id_pedido;
END $$

-- 5. PROCEDIMIENTO PARA ACTUALIZAR UN PEDIDO

CREATE PROCEDURE ActualizarPedidoPorIDYMesa(
    IN p_id INT,
    IN p_estado INT,
    IN p_id_mesa INT,
    IN p_id_estado_servicio INT,
    IN p_id_estado_delivery INT,
    IN p_fecha DATE,
    IN p_hora TIME,
    IN p_id_mesero INT
)
BEGIN
    UPDATE pedidos
    SET
        estado = p_estado,
        id_mesa = p_id_mesa,
        id_estado_servicio = p_id_estado_servicio,
        id_estado_delivery = p_id_estado_delivery,
        fecha = p_fecha,
        hora = p_hora,
        id_mesero = p_id_mesero
    WHERE id = p_id AND id_mesa = p_id_mesa;
END $$

-- 6. PROCEDIMIENTO PARA ELIMINAR UN PEDIDO

CREATE PROCEDURE EliminarPedidoPorIDYMesa(
    IN p_id INT,
    IN p_id_mesa INT
)
BEGIN
    DELETE FROM pedidos WHERE id = p_id AND id_mesa = p_id_mesa;
END $$

-- 7. PROCEDIMIENTO PARA ELIMINAR LOS DETALLES DE UN PEDIDO

CREATE PROCEDURE EliminarDetallesPorPedido(
    IN p_id_pedido INT
)
BEGIN
    DELETE FROM pedidosDetalles WHERE id_pedido = p_id_pedido;
END $$

-- 8. PROCEDIMIENTO PARA OBTENER PEDIDOS Y DETALLES DE UNA MESA

CREATE PROCEDURE ObtenerPedidosYDetallesPorMesa(
    IN p_id_mesa INT
)
BEGIN
    SELECT p.*, pd.*
    FROM pedidos p
    JOIN pedidosDetalles pd ON p.id = pd.id_pedido
    WHERE p.id_mesa = p_id_mesa;
END $$

-- Fin de los procedimientos almacenados

DELIMITER ;
