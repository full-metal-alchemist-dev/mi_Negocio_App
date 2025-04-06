-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 05-04-2025 a las 16:45:07
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `minegocio`
--

DELIMITER $$
--
-- Procedimientos
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `ActualizarCliente` (IN `c_id` INT, IN `c_nombre` VARCHAR(100), IN `c_direccion` VARCHAR(255), IN `c_telefono` VARCHAR(20))   BEGIN
    UPDATE clientes 
    SET nombre = c_nombre, direccion = c_direccion, telefono = c_telefono 
    WHERE id = c_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ActualizarPedidoPorIDYMesa` (IN `p_id` INT, IN `p_estado` INT, IN `p_id_mesa` INT, IN `p_id_estado_servicio` INT, IN `p_id_estado_delivery` INT, IN `p_fecha` DATE, IN `p_hora` TIME, IN `p_id_mesero` INT)   BEGIN
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ActualizarProducto` (IN `p_id` INT, IN `p_nombre` VARCHAR(100), IN `p_precio` DECIMAL(10,2), IN `p_cantidad` INT)   BEGIN
    UPDATE productos 
    SET nombre = p_nombre, precio = p_precio, cantidad = p_cantidad 
    WHERE id = p_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `EliminarCliente` (IN `c_id` INT)   BEGIN
    DELETE FROM clientes WHERE id = c_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `EliminarDetallesPorPedido` (IN `p_id_pedido` INT)   BEGIN
    DELETE FROM pedidosDetalles WHERE id_pedido = p_id_pedido;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `EliminarPedidoPorIDYMesa` (IN `p_id` INT, IN `p_id_mesa` INT)   BEGIN
    DELETE FROM pedidos WHERE id = p_id AND id_mesa = p_id_mesa;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `EliminarProducto` (IN `p_id` INT)   BEGIN
    DELETE FROM productos WHERE id = p_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertarCliente` (IN `c_nombre` VARCHAR(100), IN `c_direccion` VARCHAR(255), IN `c_telefono` VARCHAR(20))   BEGIN
    INSERT INTO clientes (nombre, direccion, telefono) 
    VALUES (c_nombre, c_direccion, c_telefono);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertarPedido` (IN `p_id_mesa` INT, IN `p_id_estado_servicio` INT, IN `p_id_estado_delivery` INT, IN `p_fecha` DATE, IN `p_hora` TIME, IN `p_estado` INT, IN `p_id_mesero` INT)   BEGIN
    DECLARE idx INT;

    -- Buscar el último pedido en estado 1 para la mesa indicada
    SELECT id INTO idx  -- Cambiar 'id' a 'pedido_id' si es necesario
    FROM pedidos
    WHERE id_mesa = p_id_mesa AND estado = 1
    ORDER BY id DESC  -- Asegúrate de usar el mismo nombre aquí
    LIMIT 1;

    -- Si ya existe un pedido activo, devolver su ID y no insertar un nuevo registro
    IF idx IS NOT NULL THEN
        SELECT idx AS id_pedido_existente;
    ELSE
        -- Inserta el nuevo pedido
        INSERT INTO pedidos (id_mesa, id_estado_servicio, id_estado_delivery, fecha, hora, estado, id_mesero)
        VALUES (p_id_mesa, p_id_estado_servicio, p_id_estado_delivery, p_fecha, p_hora, p_estado, p_id_mesero);

        -- Devuelve el ID del pedido insertado
        SELECT LAST_INSERT_ID();
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertarPedidoDetalles` (IN `p_id_pedido` INT, IN `p_id_producto` INT, IN `p_cantidad` INT, IN `p_precio` DECIMAL(10,2), OUT `p_subtotal` DECIMAL(10,2))   BEGIN
    -- Calcular el subtotal
    SET p_subtotal = p_precio * p_cantidad;
    
    -- Insertar un nuevo detalle en la tabla 'pedidosDetalles'
    INSERT INTO pedidosDetalles (id_pedido, id_producto, cantidad, precio, subtotal)
    VALUES (p_id_pedido, p_id_producto, p_cantidad, p_precio, p_subtotal);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `InsertarProducto` (IN `p_nombre` VARCHAR(100), IN `p_precio` DECIMAL(10,2), IN `p_cantidad` INT, IN `p_productotipoid` INT, IN `p_productocategoriaid` INT)   BEGIN
    INSERT INTO productos (nombre, precio, cantidad, productotipoid, productocategoriaid) 
    VALUES (p_nombre, p_precio, p_cantidad, p_productotipoid, p_productocategoriaid);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerClientePorID` (IN `c_id` INT)   BEGIN
    SELECT * FROM clientes WHERE id = c_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerClientes` ()   BEGIN
    SELECT * FROM clientes;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerDetallesPorPedido` (IN `p_id_pedido` INT)   BEGIN
    SELECT * FROM pedidosDetalles WHERE id_pedido = p_id_pedido;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerPedidosPorMesa` (IN `p_id_mesa` INT)   BEGIN
    SELECT * FROM pedidos WHERE id_mesa = p_id_mesa;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerPedidosYDetallesPorMesa` (IN `p_id_mesa` INT)   BEGIN
 SELECT  pd.id, pd.id_producto, pd.cantidad, pro.nombre, pd.precio, pd.subtotal
    FROM pedidos p
    JOIN pedidosDetalles pd ON p.id = pd.id_pedido
    JOIN productos pro ON pro.id = pd.id_producto
    WHERE p.id_mesa = p_id_mesa;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerProductoCategorias` ()   BEGIN
    SELECT id, productotipoId, descripcion FROM productocategoria;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerProductoCategoriasPorTipo` (IN `tipoId` INT)   BEGIN
    SELECT id, descripcion
    FROM productocategoria
    WHERE productotipoId = tipoId;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerProductoPorID` (IN `p_id` INT)   BEGIN
    SELECT * FROM productos WHERE id = p_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerProductos` ()   BEGIN
    SELECT * FROM productos;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerProductosDetalles` ()   BEGIN
    SELECT 
        p.id, 
        p.nombre, 
        p.precio, 
        p.cantidad, 
        p.productotipoid,
        p.productocategoriaid,
        ptipo.descripcion AS tipo, 
        pcate.descripcion AS categoria
    FROM productos AS p
    INNER JOIN productotipo AS ptipo ON ptipo.id = p.productotipoid
    INNER JOIN productocategoria AS pcate ON pcate.id = p.productocategoriaid;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerProductosPorTipoYCategoria` (IN `p_tipo_id` INT, IN `p_categoria_id` INT)   BEGIN
    SELECT * FROM productos 
    WHERE productotipoid = p_tipo_id 
      AND productocategoriaid = p_categoria_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `ObtenerProductoTipos` ()   BEGIN
    SELECT id, descripcion FROM productotipo;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `clientes`
--

CREATE TABLE `clientes` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `direccion` varchar(255) NOT NULL,
  `telefono` varchar(20) NOT NULL,
  `correo` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `clientes`
--

INSERT INTO `clientes` (`id`, `nombre`, `direccion`, `telefono`, `correo`) VALUES
(1, 'Carlos Pérez', 'Av. Central 123, Managua', '505-8888-1234', ''),
(2, 'Ana López', 'Calle 10 #45, León', '505-7777-5678', ''),
(3, 'José Martínez', 'Colonia Centro, Granada', '505-9999-8765', ''),
(4, 'María Fernández', 'Barrio San Juan, Masaya', '505-6666-3456', ''),
(5, 'Pedro Ramírez', 'Km 5 Carretera Norte, Managua', '505-5555-7890', ''),
(6, 'Danilo', 'Berroteran Vivas', '89065522', '');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estados_delivery`
--

CREATE TABLE `estados_delivery` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Volcado de datos para la tabla `estados_delivery`
--

INSERT INTO `estados_delivery` (`id`, `descripcion`) VALUES
(1, 'Sin delivery'),
(2, 'Con delivery');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estados_servicio`
--

CREATE TABLE `estados_servicio` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Volcado de datos para la tabla `estados_servicio`
--

INSERT INTO `estados_servicio` (`id`, `descripcion`) VALUES
(1, 'Sin servicio'),
(2, 'Con servicio');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `mesas`
--

CREATE TABLE `mesas` (
  `id` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `capacidad` int(11) NOT NULL,
  `estado` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Volcado de datos para la tabla `mesas`
--

INSERT INTO `mesas` (`id`, `nombre`, `capacidad`, `estado`) VALUES
(1, 'mesa 1', 6, 1),
(2, 'mesa 2', 6, 1),
(3, 'mesa 3', 6, 1),
(4, 'mesa 4', 6, 1),
(5, 'mesa 5', 6, 1),
(6, 'mesa 6', 6, 1),
(7, 'mesa 7', 6, 1),
(8, 'mesa 8', 6, 1),
(9, 'mesa 9', 6, 1),
(10, 'mesa 10', 6, 1),
(11, 'mesa 11', 6, 1),
(12, 'mesa 12', 6, 1),
(13, 'mesa 13', 6, 1),
(14, 'mesa 14', 6, 1),
(15, 'mesa 15', 6, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedidos`
--

CREATE TABLE `pedidos` (
  `id` int(11) NOT NULL,
  `estado` int(11) NOT NULL,
  `id_mesa` int(11) NOT NULL,
  `id_estado_servicio` int(11) NOT NULL,
  `id_estado_delivery` int(11) NOT NULL,
  `fecha` date NOT NULL,
  `hora` time NOT NULL,
  `id_mesero` int(11) DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Volcado de datos para la tabla `pedidos`
--

INSERT INTO `pedidos` (`id`, `estado`, `id_mesa`, `id_estado_servicio`, `id_estado_delivery`, `fecha`, `hora`, `id_mesero`) VALUES
(1, 1, 3, 1, 1, '2025-03-30', '22:32:42', 1),
(2, 1, 2, 1, 1, '2025-03-31', '00:33:28', 1),
(3, 1, 1, 1, 1, '2025-04-03', '15:32:38', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pedidosdetalles`
--

CREATE TABLE `pedidosdetalles` (
  `id` int(11) NOT NULL,
  `id_pedido` int(11) NOT NULL,
  `id_producto` int(11) NOT NULL,
  `cantidad` int(11) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `subtotal` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Volcado de datos para la tabla `pedidosdetalles`
--

INSERT INTO `pedidosdetalles` (`id`, `id_pedido`, `id_producto`, `cantidad`, `precio`, `subtotal`) VALUES
(1, 1, 7, 3, 250.00, 750.00),
(2, 1, 1, 3, 65.00, 195.00),
(3, 1, 5, 3, 35.00, 105.00),
(4, 1, 2, 1, 66.40, 66.40),
(5, 1, 7, 1, 250.00, 250.00),
(6, 2, 2, 3, 66.40, 199.20),
(7, 2, 4, 2, 80.00, 160.00),
(8, 2, 5, 5, 35.00, 175.00),
(9, 2, 7, 2, 250.00, 500.00),
(10, 3, 2, 4, 66.40, 265.60);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `productocategoria`
--

CREATE TABLE `productocategoria` (
  `id` int(11) NOT NULL,
  `productotipoId` int(11) NOT NULL,
  `descripcion` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Volcado de datos para la tabla `productocategoria`
--

INSERT INTO `productocategoria` (`id`, `productotipoId`, `descripcion`) VALUES
(1, 2, 'CON ALCOHOL'),
(2, 2, 'SIN ALCOHOL'),
(3, 1, 'PLATILLO'),
(4, 1, 'POSTRE');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `productos`
--

CREATE TABLE `productos` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `cantidad` int(11) NOT NULL,
  `productotipoid` int(11) NOT NULL,
  `productocategoriaid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `productos`
--

INSERT INTO `productos` (`id`, `nombre`, `precio`, `cantidad`, `productotipoid`, `productocategoriaid`) VALUES
(1, 'TOÑA', 65.00, 100, 2, 1),
(2, 'VICTORIA', 66.40, 60, 2, 1),
(3, 'CUBETAZO', 180.00, 1000, 2, 1),
(4, 'SMIRNOFF', 80.00, 50, 2, 1),
(5, 'GASEOSA', 35.00, 90, 2, 2),
(6, 'COLA CHALER', 33.00, 33, 2, 2),
(7, 'CARNE ASADA', 250.00, 1000, 1, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `productotipo`
--

CREATE TABLE `productotipo` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Volcado de datos para la tabla `productotipo`
--

INSERT INTO `productotipo` (`id`, `descripcion`) VALUES
(1, 'COMIDA'),
(2, 'BEBIDA');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `clientes`
--
ALTER TABLE `clientes`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `estados_delivery`
--
ALTER TABLE `estados_delivery`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `estados_servicio`
--
ALTER TABLE `estados_servicio`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `mesas`
--
ALTER TABLE `mesas`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `pedidos`
--
ALTER TABLE `pedidos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_mesa` (`id_mesa`),
  ADD KEY `id_estado_servicio` (`id_estado_servicio`),
  ADD KEY `id_estado_delivery` (`id_estado_delivery`);

--
-- Indices de la tabla `pedidosdetalles`
--
ALTER TABLE `pedidosdetalles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_pedido` (`id_pedido`),
  ADD KEY `id_producto` (`id_producto`);

--
-- Indices de la tabla `productocategoria`
--
ALTER TABLE `productocategoria`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `productos`
--
ALTER TABLE `productos`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `productotipo`
--
ALTER TABLE `productotipo`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `clientes`
--
ALTER TABLE `clientes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `estados_delivery`
--
ALTER TABLE `estados_delivery`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `estados_servicio`
--
ALTER TABLE `estados_servicio`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `mesas`
--
ALTER TABLE `mesas`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `pedidos`
--
ALTER TABLE `pedidos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `pedidosdetalles`
--
ALTER TABLE `pedidosdetalles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `productocategoria`
--
ALTER TABLE `productocategoria`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `productos`
--
ALTER TABLE `productos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `productotipo`
--
ALTER TABLE `productotipo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `pedidos`
--
ALTER TABLE `pedidos`
  ADD CONSTRAINT `pedidos_ibfk_1` FOREIGN KEY (`id_mesa`) REFERENCES `mesas` (`id`),
  ADD CONSTRAINT `pedidos_ibfk_2` FOREIGN KEY (`id_estado_servicio`) REFERENCES `estados_servicio` (`id`),
  ADD CONSTRAINT `pedidos_ibfk_3` FOREIGN KEY (`id_estado_delivery`) REFERENCES `estados_delivery` (`id`);

--
-- Filtros para la tabla `pedidosdetalles`
--
ALTER TABLE `pedidosdetalles`
  ADD CONSTRAINT `pedidosdetalles_ibfk_1` FOREIGN KEY (`id_pedido`) REFERENCES `pedidos` (`id`),
  ADD CONSTRAINT `pedidosdetalles_ibfk_2` FOREIGN KEY (`id_producto`) REFERENCES `productos` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
