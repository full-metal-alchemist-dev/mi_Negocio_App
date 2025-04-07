-- IdProducto
-- Cantidad
-- Producto
-- Precio
-- Subtotal
 
 SELECT p.*, pd.*
    FROM pedidos p
    JOIN pedidosDetalles pd ON p.id = pd.id_pedido
    WHERE p.id_mesa = 3;



    -- IdProducto
-- Cantidad
-- Producto
-- Precio
-- Subtotal
 
-- IdProducto
-- Cantidad
-- Producto
-- Precio
-- Subtotal
 
 SELECT  pd.id, pd.id_producto, pd.cantidad, pro.nombre, pd.precio, pd.subtotal
    FROM pedidos p
    JOIN pedidosDetalles pd ON p.id = pd.id_pedido
    JOIN productos pro ON pro.id = pd.id_producto
    WHERE p.id_mesa = 3;


DROP PROCEDURE IF EXISTS `ObtenerPedidosYDetallesPorMesa`;
DELIMITER $$

CREATE PROCEDURE `ObtenerPedidosYDetallesPorMesa`(IN `p_id_mesa` INT)
BEGIN
 SELECT  pd.id, pd.id_producto, pd.cantidad, pro.nombre, pd.precio, pd.subtotal
    FROM pedidos p
    JOIN pedidosDetalles pd ON p.id = pd.id_pedido
    JOIN productos pro ON pro.id = pd.id_producto
    WHERE p.id_mesa = p_id_mesa;
END $$

DELIMITER ;