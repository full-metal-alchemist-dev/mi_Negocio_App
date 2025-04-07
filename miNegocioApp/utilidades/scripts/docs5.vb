utilizando  como referencia 

Namespace Entities
    Public Class Pedido
        Public Property Id As Integer
        Public Property Estado As Integer
        Public Property IdMesa As Integer
        Public Property IdEstadoServicio As Integer
        Public Property IdEstadoDelivery As Integer
        Public Property Fecha As Date
        Public Property Hora As TimeSpan
        Public Property IdMesero As Integer
    End Class
End Namespace

Namespace Entities
    Public Class PedidoDetalle
        Public Property Id As Integer
        Public Property IdPedido As Integer
        Public Property IdProducto As Integer
        Public Property Cantidad As Integer
        Public Property Precio As Decimal
        Public Property Subtotal As Decimal
    End Class
End Namespace

' Insertar un nuevo pedido
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



enseñame como al presionar el boton 

 Private Sub btnRegistarPedido_Click(sender As Object, e As EventArgs) Handles btnRegistarPedido.Click

    End Sub



    puedo insertar el pedido en la base de datos 


    para aprovechar los metos de insertar ocuparas la siguiente informacion

    Imports MiNegocioApp.DataAccess
Imports MiNegocioApp.Entities

Public Class Atencion
    Private sumaTotal As Decimal = 0

    Private Sub Atencion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarProductos()
    End Sub

    Private Sub Atencion_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ' Este código se ejecutará justo después de que el formulario haya sido mostrado en pantalla
        dgvProductos1.ClearSelection()
        dgvProductos2.ClearSelection()
        dgvProductos3.ClearSelection()
        ' Calcular la suma total de la columna Subtotal
        CalcularTotal()
        ActualizarEstadoDelivery()
    End Sub



    Private productoDataAccess As ProductoDataAccess

    Public Sub New(numero As Integer)
        InitializeComponent()
        lblMesa.Text = "MESA # " + numero.ToString() ' Asigna el número al label o a cualquier otra parte que necesites
        productoDataAccess = New ProductoDataAccess()
    End Sub

    Private Sub ConfigurarDataGridView(dgv As DataGridView, productos As List(Of Producto))
        With dgv
            .DataSource = Nothing ' Limpia el DataGridView
            .DataSource = productos ' Asigna la lista de productos
            .Columns(0).Visible = False ' Oculta la columna 0
            .Columns(3).Visible = False ' Oculta la columna 3
            .Columns(4).Visible = False ' Oculta la columna 4
            .Columns(5).Visible = False ' Oculta la columna 5
            .Columns(6).Visible = False ' Oculta la columna 6
            .Columns(7).Visible = False ' Oculta la columna 7
        End With
    End Sub

    ' Método para cargar productos en el DataGridView
    Private Sub CargarProductos()
        ' Usar la función para los diferentes tipos de productos
        ' Productos Platillos
        Dim productos1 As List(Of Producto) = productoDataAccess.ObtenerProductosPorTipoYCategoria(1, 3)
        ConfigurarDataGridView(dgvProductos1, productos1)

        ' Productos Bebidas con Alcohol
        Dim productos2 As List(Of Producto) = productoDataAccess.ObtenerProductosPorTipoYCategoria(2, 1)
        ConfigurarDataGridView(dgvProductos2, productos2)

        ' Productos Bebidas sin Alcohol
        Dim productos3 As List(Of Producto) = productoDataAccess.ObtenerProductosPorTipoYCategoria(2, 2)
        ConfigurarDataGridView(dgvProductos3, productos3)

        ' Asegurarse de que no haya filas seleccionadas después de cargar los productos
        dgvProductos1.ClearSelection()
        dgvProductos2.ClearSelection()
        dgvProductos3.ClearSelection()
    End Sub

    ' Este es el evento para el DataGridView de Platillos
    Private Sub dgvProductos1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos1.CellClick
        ' Deseleccionamos cualquier fila en los otros dos DataGridViews
        If e.RowIndex >= 0 Then
            dgvProductos2.ClearSelection()
            dgvProductos3.ClearSelection()
            lblProductoSelected.Text = dgvProductos1.Rows(e.RowIndex).Cells(1).Value.ToString()
        End If
    End Sub

    ' Este es el evento para el DataGridView de Bebidas con Alcohol
    Private Sub dgvProductos2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos2.CellClick
        ' Deseleccionamos cualquier fila en los otros dos DataGridViews
        If e.RowIndex >= 0 Then
            dgvProductos1.ClearSelection()
            dgvProductos3.ClearSelection()
            lblProductoSelected.Text = dgvProductos2.Rows(e.RowIndex).Cells(1).Value.ToString()
        End If
    End Sub

    ' Este es el evento para el DataGridView de Bebidas sin Alcohol
    Private Sub dgvProductos3_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos3.CellClick
        ' Deseleccionamos cualquier fila en los otros dos DataGridViews
        If e.RowIndex >= 0 Then
            dgvProductos1.ClearSelection()
            dgvProductos2.ClearSelection()
            lblProductoSelected.Text = dgvProductos3.Rows(e.RowIndex).Cells(1).Value.ToString()
        End If

    End Sub

    Private Sub btnAddPoducto_Click(sender As Object, e As EventArgs) Handles btnAddPoducto.Click
        Dim nombreProducto As String = Nothing
        Dim precioProducto As Decimal = 0
        Dim cantidadProducto As Integer = 0
        Dim subtotalProducto As Decimal = 0

        ' Verificar cuál de los 3 DataGridView tiene una fila seleccionada
        If dgvProductos1.SelectedRows.Count > 0 Then
            nombreProducto = dgvProductos1.SelectedRows(0).Cells(1).Value.ToString()
            precioProducto = Convert.ToDecimal(dgvProductos1.SelectedRows(0).Cells(2).Value) ' Convertir a Decimal
        ElseIf dgvProductos2.SelectedRows.Count > 0 Then
            nombreProducto = dgvProductos2.SelectedRows(0).Cells(1).Value.ToString()
            precioProducto = Convert.ToDecimal(dgvProductos2.SelectedRows(0).Cells(2).Value) ' Convertir a Decimal
        ElseIf dgvProductos3.SelectedRows.Count > 0 Then
            nombreProducto = dgvProductos3.SelectedRows(0).Cells(1).Value.ToString()
            precioProducto = Convert.ToDecimal(dgvProductos3.SelectedRows(0).Cells(2).Value) ' Convertir a Decimal
        End If

        ' Verificar si el campo de cantidad es válido
        If Not Integer.TryParse(txtCantidad.Text, cantidadProducto) Then
            MessageBox.Show("Ingrese una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ' Calcular el subtotal
        subtotalProducto = cantidadProducto * precioProducto

        ' Si se seleccionó un producto, agregarlo a dgvProductos4
        If Not String.IsNullOrEmpty(nombreProducto) AndAlso precioProducto > 0 Then
            ' Verificar si dgvProductos4 tiene columnas, si no, agregarlas
            If dgvProductos4.ColumnCount = 0 Then
                dgvProductos4.Columns.Add("Cantidad", "Cantidad")
                dgvProductos4.Columns.Add("Producto", "Producto")
                dgvProductos4.Columns.Add("Precio", "Precio")
                dgvProductos4.Columns.Add("SubTotal", "Subtotal") ' Corrección en el nombre de la columna
            End If

            ' Agregar una fila completa con cantidad, producto, precio y subtotal
            dgvProductos4.Rows.Add(cantidadProducto, nombreProducto, precioProducto.ToString("N2"), subtotalProducto.ToString("N2"))
            txtCantidad.Text = 1
            lblProductoSelected.Text = "......................................................................................"
        Else
            MessageBox.Show("Por favor, seleccione un producto en uno de los grids.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        ' Calcular la suma total de la columna Subtotal
        Dim sumaTotal As Decimal = 0

        For Each row As DataGridViewRow In dgvProductos4.Rows
            If Not row.IsNewRow Then ' Evita contar la fila nueva en blanco
                sumaTotal += Convert.ToDecimal(row.Cells("SubTotal").Value)
            End If
        Next

        ' Mostrar el total en el TextBox
        txtSubTotal.Text = sumaTotal.ToString("N2") ' Formato con 2 decimales


        ' Calcular la suma total de la columna Subtotal
        CalcularTotal()

    End Sub

    ' Evento que se ejecuta cuando el CheckBox cambia su estado
    Private Sub ckServicio_CheckedChanged(sender As Object, e As EventArgs) Handles ckServicio.CheckedChanged
        If ckServicio.Checked Then
            ckDelivery.Checked = False
        End If
        ActualizarEstadoDelivery()
        CalcularTotal()
    End Sub




    Private Sub ckDelivery_CheckedChanged(sender As Object, e As EventArgs) Handles ckDelivery.CheckedChanged
        ' Llamamos a la función para actualizar el estado de Delivery cuando el CheckBox cambie
        If ckDelivery.Checked Then
            ckServicio.Checked = False
        End If
        ActualizarEstadoDelivery()
        CalcularTotal()
    End Sub

    ' Función para actualizar el estado del Delivery
    Private Sub ActualizarEstadoDelivery()
        If ckDelivery.Checked Then
            ' Si el Delivery está marcado, asignamos un valor inicial de 20 a txtDelivery
            txtDelivery.Text = "20"
            ' Habilitamos los botones relacionados al Delivery
            btnUpDelivery.Enabled = True
            btnDownDelivery.Enabled = True
        Else
            ' Si el Delivery no está marcado, asignamos 0 a txtDelivery
            txtDelivery.Text = "0"
            ' Deshabilitamos los botones relacionados al Delivery
            btnUpDelivery.Enabled = False
            btnDownDelivery.Enabled = False
        End If
    End Sub

    ' Método para calcular el total y actualizar los valores
    Private Sub CalcularTotal()
        sumaTotal = 0 ' Reiniciar la suma total

        For Each row As DataGridViewRow In dgvProductos4.Rows
            If Not row.IsNewRow Then
                sumaTotal += Convert.ToDecimal(row.Cells("SubTotal").Value)
            End If
        Next

        ' Mostrar el total en el TextBox
        txtSubTotal.Text = sumaTotal.ToString("N2")

        ' Calcular el servicio si el CheckBox está marcado
        If ckServicio.Checked Then
            txtServicio.Text = (sumaTotal * 0.1).ToString("N2")
        Else
            txtServicio.Text = "0.00"
        End If

        ' Verificar si txtDelivery tiene un valor válido
        Dim delivery As Decimal = 0
        If Decimal.TryParse(txtDelivery.Text, delivery) Then
            ' Si es válido, usar el valor de txtDelivery
        End If

        ' Sumar el total a pagar (SubTotal + Servicio + Delivery)
        Dim netoAPagar As Decimal = sumaTotal + Convert.ToDecimal(txtServicio.Text) + delivery

        ' Mostrar el total a pagar en el Label
        lblNetoAPagar.Text = netoAPagar.ToString("N2")
    End Sub





    ' Función para obtener el producto desde una fila
    Private Function ObtenerProductoDesdeFila(fila As DataGridViewRow) As Producto
        Return New Producto() With {
            .Id = Convert.ToInt32(fila.Cells("Id").Value),
            .Nombre = fila.Cells("Nombre").Value.ToString(),
            .Precio = Convert.ToDecimal(fila.Cells("Precio").Value),
            .Cantidad = Convert.ToInt32(fila.Cells("Cantidad").Value),
            .ProductotipoId = Convert.ToInt32(fila.Cells("ProductotipoId").Value),
            .ProductocategoriaId = Convert.ToInt32(fila.Cells("ProductocategoriaId").Value)
        }
    End Function

    Private Sub dgvProductos1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos1.CellContentClick

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Verificar si txtCantidad tiene un valor válido, si no, asignar 1 como predeterminado
        Dim cantidad As Integer

        If Integer.TryParse(txtCantidad.Text, cantidad) Then
            cantidad += 1 ' Incrementar en 1
        Else
            cantidad = 1 ' Si el valor no es válido, inicializar en 1
        End If

        txtCantidad.Text = cantidad.ToString() ' Asignar el nuevo valor al TextBox
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ' Verificar si txtCantidad tiene un valor válido, si no, asignar 1 como predeterminado
        Dim cantidad As Integer

        If Integer.TryParse(txtCantidad.Text, cantidad) And cantidad > 1 Then
            cantidad -= 1 ' Incrementar en 1
        Else
            cantidad = 1 ' Si el valor no es válido, inicializar en 1
        End If

        txtCantidad.Text = cantidad.ToString() ' Asignar el nuevo valor al TextBox
    End Sub

    Private Sub btnQuitar_Click(sender As Object, e As EventArgs) Handles btnQuitar.Click
        ' Verificar si hay una fila seleccionada en dgvProductos4
        If dgvProductos4.SelectedRows.Count > 0 Then
            ' Eliminar la fila seleccionada
            dgvProductos4.Rows.Remove(dgvProductos4.SelectedRows(0))

            ' Recalcular los totales
            CalcularTotal()
        Else
            MessageBox.Show("Seleccione un producto para quitar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnDownDelivery_Click(sender As Object, e As EventArgs) Handles btnDownDelivery.Click
        ' Verificar si txtCantidad tiene un valor válido
        Dim delivery As Integer

        If Integer.TryParse(txtDelivery.Text, delivery) Then
            ' Si la delivery es mayor o igual a 30, disminuir 10
            If delivery >= 30 Then
                delivery -= 10
                ' Si la delivery es mayor a 20 pero menor a 30, disminuir 1
            ElseIf delivery > 20 Then
                delivery -= 1
            End If
        End If

        ' Asegurar que el valor nunca sea menor que 20
        If delivery < 20 Then
            delivery = 20
        End If

        txtDelivery.Text = delivery.ToString() ' Asignar el nuevo valor al TextBox
        CalcularTotal()
    End Sub


    Private Sub btnUpDelivery_Click(sender As Object, e As EventArgs) Handles btnUpDelivery.Click
        ' Verificar si txtCantidad tiene un valor válido, si no, asignar 20 como valor inicial
        Dim delivery As Integer

        ' Si el valor en el TextBox es válido, incrementarlo, sino, asignar 20
        If Integer.TryParse(txtDelivery.Text, delivery) Then
            delivery += 10 ' Incrementar en 10
        Else
            delivery = 20 ' Si el valor no es válido, inicializar en 20
        End If

        txtDelivery.Text = delivery.ToString() ' Asignar el nuevo valor al TextBox
        CalcularTotal()
    End Sub

    Private Sub btnRegistarPedido_Click(sender As Object, e As EventArgs) Handles btnRegistarPedido.Click

    End Sub
End Class
