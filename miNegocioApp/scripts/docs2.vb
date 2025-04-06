store procedure 

ObtenerProductos

BEGIN
    SELECT * FROM productos;
END


modificar el procedimeinto almacenado anterior para que se compatible con 


' Evento para el doble clic sobre una fila
Private Sub dgvProductos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos.CellDoubleClick
    ' Verificar si se ha hecho clic en una fila válida
    If e.RowIndex >= 0 Then
        ' Obtener los datos de la fila seleccionada
        Dim selectedRow As DataGridViewRow = dgvProductos.Rows(e.RowIndex)
        Dim id As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)
        Dim nombre As String = selectedRow.Cells("Nombre").Value.ToString()
        Dim precio As Decimal = Convert.ToDecimal(selectedRow.Cells("Precio").Value)
        Dim cantidad As Integer = Convert.ToInt32(selectedRow.Cells("Cantidad").Value)
        Dim productotipoId As Integer = Convert.ToInt32(selectedRow.Cells("ProductotipoId").Value)
        Dim productocategoriaId As Integer = Convert.ToInt32(selectedRow.Cells("ProductocategoriaId").Value)

        ' Cargar los datos en los controles correspondientes
        txtNombre.Text = nombre
        txtPrecio.Text = precio.ToString()
        txtCantidad.Text = cantidad.ToString()

        ' Seleccionar el tipo y categoría en los ComboBox
        cboTipo.SelectedValue = productotipoId
        cboCategoria.SelectedValue = productocategoriaId
    End If
End Sub



en base a 

   ' Seleccionar producto en el DataGridView y cargar datos en los campos de texto
    ' Evento para el doble clic sobre una fila
    Private Sub dgvProductos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos.CellDoubleClick
        ' Verificar si se ha hecho clic en una fila válida
        If e.RowIndex >= 0 Then
            ' Obtener los datos de la fila seleccionada
            Dim selectedRow As DataGridViewRow = dgvProductos.Rows(e.RowIndex)
            Dim id As Integer = Convert.ToInt32(selectedRow.Cells("Id").Value)
            Dim nombre As String = selectedRow.Cells("Nombre").Value.ToString()
            Dim precio As Decimal = Convert.ToDecimal(selectedRow.Cells("Precio").Value)
            Dim cantidad As Integer = Convert.ToInt32(selectedRow.Cells("Cantidad").Value)
            Dim productotipoId As Integer = Convert.ToInt32(selectedRow.Cells("ProductotipoId").Value)
            Dim productocategoriaId As Integer = Convert.ToInt32(selectedRow.Cells("ProductocategoriaId").Value)

            ' Cargar los datos en los controles correspondientes
            txtNombre.Text = nombre
            txtPrecio.Text = precio.ToString()
            txtCantidad.Text = cantidad.ToString()

            ' Seleccionar el tipo y categoría en los ComboBox
            cboTipo.SelectedValue = productotipoId
            cboCategoria.SelectedValue = productocategoriaId
        End If
    End Sub


    al parecer no detecta o no persive que quiero modificar la fila

  Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvProductos.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un producto para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim id As Integer = Convert.ToInt32(dgvProductos.SelectedRows(0).Cells("Id").Value)
        Dim nombre As String = txtNombre.Text
        Dim precio As Decimal
        Dim cantidad As Integer

        If Not Decimal.TryParse(txtPrecio.Text, precio) Then
            MessageBox.Show("Ingrese un precio válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not Integer.TryParse(txtCantidad.Text, cantidad) Then
            MessageBox.Show("Ingrese una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If productoDataAccess.ActualizarProducto(id, nombre, precio, cantidad) Then
            MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarProductos()
        Else
            MessageBox.Show("Error al actualizar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub



    Tengo 2 windows forms 

    menu principal 

Public Class MenuPrincipal
    Private Sub btnProductos_Click(sender As Object, e As EventArgs) Handles btnProductos.Click
        Dim productosForm As New Productos()
        productosForm.Show()
    End Sub



    Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        Dim clientesForm As New Clientes()
        clientesForm.Show()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Dim atencionForm As New Atencion()
        quiero pasarle a atencionForm  un numero como si fuera un argumento ejemplo numero 1
        atencionForm.Show()
    End Sub

    Private Sub MenuPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class


Imports MiNegocioApp.DataAccess
Imports MiNegocioApp.Entities

Public Class Atencion
    Private Sub Atencion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarProductos()
    End Sub

    Private productoDataAccess As ProductoDataAccess

    Public Sub New()
        InitializeComponent()
        lblMesa.Text = quiero obtener el valor proveniente de menu principal ejemplo el 1
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



    End Sub


End Class




tengo 4 gridview

dgvProductos1
dgvProductos1
dgvProductos3
y
dgvProductos4

como crear un funcion clik en el boton btnAddPoducto

detecte de los 3 primer gridview
cual es el row seleccionado 

y de esta forma pasarle ese registro al 
grid 4

Imports MiNegocioApp.DataAccess
Imports MiNegocioApp.Entities

Public Class Atencion
    Private Sub Atencion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarProductos()
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
    End Sub

    ' Este es el evento para el DataGridView de Platillos
    Private Sub dgvProductos1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos1.CellClick
        ' Deseleccionamos cualquier fila en los otros dos DataGridViews
        dgvProductos2.ClearSelection()
        dgvProductos3.ClearSelection()
    End Sub

    ' Este es el evento para el DataGridView de Bebidas con Alcohol
    Private Sub dgvProductos2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos2.CellClick
        ' Deseleccionamos cualquier fila en los otros dos DataGridViews
        dgvProductos1.ClearSelection()
        dgvProductos3.ClearSelection()
    End Sub

    ' Este es el evento para el DataGridView de Bebidas sin Alcohol
    Private Sub dgvProductos3_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProductos3.CellClick
        ' Deseleccionamos cualquier fila en los otros dos DataGridViews
        dgvProductos1.ClearSelection()
        dgvProductos2.ClearSelection()
    End Sub


End Class




utilizando la informacion de 

 lblMesa.Text = "MESA # " + numero.ToString() 


 sobre todo enfocado en usar numero (la variable numero representa el id de la mesa)
    


    utilizando la informacion  de 


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

los estados de los checkboxs

- ckServicio
- ckDelivery


quiero que crees que contoda la informacion anterior

la tabla para la base de datos  llamada pedidos (estado del pedido, 1 pendiente de pagar, 2 pagado, id mesa, , id estado servicio, id estado delivery, fecha, hora, id mesero por default 1,  )
la tabla para la base de datos  llamada pedidosDetalles (todos los detalles pertinentes)
procedimientos almacenados para consultar por medio del id mesa y id estado pedido
al hacer click en btnRegistarPedido 
  Private Sub btnRegistarPedido_Click(sender As Object, e As EventArgs) Handles btnRegistarPedido.Click

    End Sub

    se guarde el pedido 






ademas   
