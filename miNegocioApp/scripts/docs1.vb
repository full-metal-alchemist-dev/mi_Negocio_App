
CREATE TABLE `productotipo` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `productotipo`
--

INSERT INTO `productotipo` (`id`, `descripcion`) VALUES
(1, 'comida'),
(2, 'bebida');



CREATE TABLE `productocategoria` (
  `id` int(11) NOT NULL,
  `productotipoId` int(11) NOT NULL,
  `descripcion` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `productocategoria`
--

INSERT INTO `productocategoria` (`id`, `productotipoId`, `descripcion`) VALUES
(1, 2, 'con alcohol'),
(2, 2, 'sin alcohol'),
(3, 1, 'platillo'),
(4, 1, 'postre');



CREATE TABLE `productos` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `precio` decimal(10,2) NOT NULL,
  `cantidad` int(11) NOT NULL,
  `productotipoid` int(11) NOT NULL,
  `productocategoriaid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `productos`
--

INSERT INTO `productos` (`id`, `nombre`, `precio`, `cantidad`, `productotipoid`, `productocategoriaid`) VALUES
(1, 'TOÑA', 65.00, 100, 2, 1),
(2, 'VICTORIA', 66.40, 60, 2, 1),
(3, 'CUBETAZO', 180.00, 1000, 2, 1),
(4, 'SMIRNOFF', 80.00, 50, 2, 1),
(5, 'GASEOSA', 35.00, 90, 2, 2);


tengo procedimientos almacenados
InsertarProducto
BEGIN
    INSERT INTO productos (nombre, precio, cantidad) 
    VALUES (p_nombre, p_precio, p_cantidad);
END

    Private Sub Productos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarProductos()
    End Sub

    ' Método para cargar productos en el DataGridView
    Private Sub CargarProductos()
        Dim productos As List(Of Producto) = productoDataAccess.ObtenerProductos()
        dgvProductos.DataSource = Nothing ' Limpia el DataGridView
        dgvProductos.DataSource = productos
        dgvProductos.Columns(0).Visible = False
    End Sub

    ' Botón para agregar un nuevo producto
    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtNombre.Text = "" OrElse txtPrecio.Text = "" OrElse txtCantidad.Text = "" Then
            MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

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

        If productoDataAccess.InsertarProducto(nombre, precio, cantidad) Then
            MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarProductos()
        Else
            MessageBox.Show("Error al agregar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub


modificar mi formulario de ser necesario 
modificar data access

Imports MySql.Data.MySqlClient
Imports MiNegocioApp.Entities

Namespace DataAccess
    Public Class ProductoDataAccess
        Private connection As DbConnection

        Public Sub New()
            connection = New DbConnection()
        End Sub

        Public Function ObtenerProductos() As List(Of Producto)
            Dim productos As New List(Of Producto)()

            'Dim query As String = "SELECT * FROM productos"
            Dim query As String = "CALL ObtenerProductos()" ' Llamada al procedimiento almacenado

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            productos.Add(New Producto With {
                                .Id = reader.GetInt32("id"),
                                .Nombre = reader.GetString("nombre"),
                                .Precio = reader.GetDecimal("precio"),
                                .Cantidad = reader.GetInt32("cantidad")
                            })
                        End While
                    End Using
                End Using
            End Using

            Return productos
        End Function


y crear los procedimientos almacenados que recuperen  productocategoria y con productotipo

como modificar el procedimiento almacenado para recibir productoId y categoriaId pero primero debo  llanar dos combobox con productocategoria y con productotipo

como agrego los combobox y como los alimnentos 


cboTipo y cboCategoria 
