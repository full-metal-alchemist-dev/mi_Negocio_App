    Private Sub btnRegistarPedido_Click(sender As Object, e As EventArgs) Handles btnRegistarPedido.Click
        ' 1. Obtener la información del pedido
        Dim pedido As New Pedido With {
            .Estado = 1, ' Suponiendo que 1 es el estado de un nuevo pedido
            .IdMesa = Convert.ToInt32(lblMesa.Text.Replace("MESA #", "").Trim()), ' Asignamos el número de mesa
            .IdEstadoServicio = If(ckServicio.Checked, 1, 0), ' 1 para servicio, 0 si no
            .IdEstadoDelivery = If(ckDelivery.Checked, 1, 0), ' 1 para delivery, 0 si no
            .Fecha = Date.Now, ' Fecha actual
            .Hora = Date.Now.TimeOfDay, ' Hora actual
            .IdMesero = 1 ' Suponiendo que el mesero tiene un ID 1, o puedes asignarlo dinámicamente
        }

        ' 2. Insertar el pedido
        Dim pedidoDataAccess As New PedidosDataAccess() ' Crea una instancia de acceso a datos
        Dim idPedido As Integer = pedidoDataAccess.InsertarPedido(pedido) ' Insertamos el pedido y obtenemos el ID generado

        ' 3. Insertar los detalles del pedido
        For Each row As DataGridViewRow In dgvProductos4.Rows
            If Not row.IsNewRow Then
                ' Obtener los detalles de cada producto
                Dim detalle As New PedidoDetalle With {
                    .IdPedido = idPedido, ' Usamos el ID del pedido recién insertado
                    .IdProducto = Convert.ToInt32(row.Cells("IdProducto").Value), ' Asignar ID de producto
                    .Cantidad = Convert.ToInt32(row.Cells("Cantidad").Value), ' Asignar cantidad
                    .Precio = Convert.ToDecimal(row.Cells("Precio").Value), ' Asignar precio
                    .Subtotal = Convert.ToDecimal(row.Cells("SubTotal").Value) ' Asignar subtotal
                }

                ' Insertamos el detalle del pedido
                pedidoDataAccess.InsertarPedidoDetalle(detalle)
            End If
        Next

        ' 4. Confirmación
        MessageBox.Show("Pedido registrado correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
        LimpiarFormulario() ' Puedes crear un método para limpiar los controles después de registrar el pedido
    End Sub



         Public Function InsertarPedido(pedido As Pedido) As Integer
            Dim query As String = "CALL InsertarPedido(@p_estado, @p_id_mesa, @p_id_estado_servicio, @p_id_estado_delivery, @p_fecha, @p_hora, @p_id_mesero, @p_id_pedido)"

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@p_estado", pedido.Estado)
                    cmd.Parameters.AddWithValue("@p_id_mesa", pedido.IdMesa)
                    cmd.Parameters.AddWithValue("@p_id_estado_servicio", pedido.IdEstadoServicio)
                    cmd.Parameters.AddWithValue("@p_id_estado_delivery", pedido.IdEstadoDelivery)
                    cmd.Parameters.AddWithValue("@p_fecha", pedido.Fecha)
                    cmd.Parameters.AddWithValue("@p_hora", pedido.Hora)
                    cmd.Parameters.AddWithValue("@p_id_mesero", pedido.IdMesero)

                    ' Parámetro de salida
                    Dim pIdPedido As New MySqlParameter("@p_id_pedido", MySqlDbType.Int32)
                    pIdPedido.Direction = ParameterDirection.Output
                    cmd.Parameters.Add(pIdPedido)

                    cmd.ExecuteNonQuery()

                    ' Obtener el ID del nuevo pedido insertado
                    Return Convert.ToInt32(pIdPedido.Value)
                End Using
            End Using
        End Function



        ' Insertar detalles de un pedido
        Public Sub InsertarPedidoDetalle(detalle As PedidoDetalle)
            Dim query As String = "CALL InsertarPedidoDetalles(@p_id_pedido, @p_id_producto, @p_cantidad, @p_precio, @p_subtotal)"

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@p_id_pedido", detalle.IdPedido)
                    cmd.Parameters.AddWithValue("@p_id_producto", detalle.IdProducto)
                    cmd.Parameters.AddWithValue("@p_cantidad", detalle.Cantidad)
                    cmd.Parameters.AddWithValue("@p_precio", detalle.Precio)
                    cmd.Parameters.AddWithValue("@p_subtotal", detalle.Subtotal)

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        End Sub