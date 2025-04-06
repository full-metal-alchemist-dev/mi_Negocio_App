por la logica de la base de DataSource

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

-- --------------------------------------------------------

--
-- Table structure for table `productos`
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


excelente el combobox de categorias  deberia depender primero de la opcion seleccionada  por el combox tipo 

Private Sub CargarCombos()
    ' Crear listas de tipos y categorías con la opción predeterminada
    Dim tipos As New List(Of KeyValuePair(Of Integer, String)) From {
        New KeyValuePair(Of Integer, String)(-1, "Selecciona una opción") ' Opción predeterminada
    }
    tipos.AddRange(productoDataAccess.ObtenerProductoTipos()) ' Agregar los tipos reales

    Dim categorias As New List(Of KeyValuePair(Of Integer, String)) From {
        New KeyValuePair(Of Integer, String)(-1, "Selecciona una opción") ' Opción predeterminada
    }
    categorias.AddRange(productoDataAccess.ObtenerProductoCategorias()) ' Agregar las categorías reales

    ' Llenar ComboBox de Tipo
    cboTipo.DataSource = New BindingSource(tipos, Nothing)
    cboTipo.DisplayMember = "Value" ' Mostrar la descripción
    cboTipo.ValueMember = "Key" ' Guardar el ID

    ' Llenar ComboBox de Categoría
    cboCategoria.DataSource = New BindingSource(categorias, Nothing)
    cboCategoria.DisplayMember = "Value" ' Mostrar la descripción
    cboCategoria.ValueMember = "Key" ' Guardar el ID
End Sub


crear procedimientos almacenados y modificar el dataaccess producto 

Imports MySql.Data.MySqlClient
Imports MiNegocioApp.Entities

Namespace DataAccess
    Public Class ProductoDataAccess
        Private connection As DbConnection

        Public Sub New()
            connection = New DbConnection()
        End Sub


                Public Function ObtenerProductoTipos() As List(Of KeyValuePair(Of Integer, String))
            Dim tipos As New List(Of KeyValuePair(Of Integer, String))()

            Dim query As String = "CALL ObtenerProductoTipos()"

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            tipos.Add(New KeyValuePair(Of Integer, String)(reader.GetInt32("id"), reader.GetString("descripcion")))
                        End While
                    End Using
                End Using
            End Using

            Return tipos
        End Function

        Public Function ObtenerProductoCategorias() As List(Of KeyValuePair(Of Integer, String))
            Dim categorias As New List(Of KeyValuePair(Of Integer, String))()

            Dim query As String = "CALL ObtenerProductoCategorias()"

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            categorias.Add(New KeyValuePair(Of Integer, String)(reader.GetInt32("id"), reader.GetString("descripcion")))
                        End While
                    End Using
                End Using
            End Using

            Return categorias
        End Function






        para que al momento de  de seleccionar un tipo de producto me aparezcan las categorias que corresponden 



        -- Crear procedimiento almacenado para obtener categorías por tipo
DELIMITER $$

CREATE PROCEDURE ObtenerProductoCategoriasPorTipo(IN tipoId INT)
BEGIN
    SELECT id, descripcion
    FROM productocategoria
    WHERE productotipoId = tipoId;
END $$

DELIMITER ;
