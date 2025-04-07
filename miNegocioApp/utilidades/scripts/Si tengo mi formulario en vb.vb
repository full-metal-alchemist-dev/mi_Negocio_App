Si tengo mi formulario en vb.net

Imports DataAccess
Imports Entities
Imports MiNegocioApp.DataAccess
Imports MiNegocioApp.Entities

Public Class Clientes
    Private clienteDataAccess As ClienteDataAccess

    Public Sub New()
        InitializeComponent()
        clienteDataAccess = New ClienteDataAccess()
    End Sub

    Private Sub Clientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarClientes()
    End Sub

    ' Método para cargar clientes en el DataGridView
    Private Sub CargarClientes()
        Dim clientes As List(Of Cliente) = clienteDataAccess.ObtenerClientes()
        dgvClientes.DataSource = Nothing ' Limpia el DataGridView
        dgvClientes.DataSource = clientes
    End Sub

    ' Botón para agregar un nuevo cliente
    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtNombre.Text = "" OrElse txtTelefono.Text = "" OrElse txtDireccion.Text = "" Then
            MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim nombre As String = txtNombre.Text
        Dim telefono As String = txtTelefono.Text
        Dim direccion As String = txtDireccion.Text

        If clienteDataAccess.InsertarCliente(nombre, direccion, telefono) Then
            MessageBox.Show("Cliente agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarClientes()
        Else
            MessageBox.Show("Error al agregar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Botón para actualizar un cliente
    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvClientes.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un cliente para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim id As Integer = Convert.ToInt32(dgvClientes.SelectedRows(0).Cells("Id").Value)
        Dim nombre As String = txtNombre.Text
        Dim telefono As String = txtTelefono.Text
        Dim direccion As String = txtDireccion.Text

        If clienteDataAccess.ActualizarCliente(id, nombre, direccion, telefono) Then
            MessageBox.Show("Cliente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            CargarClientes()
        Else
            MessageBox.Show("Error al actualizar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    ' Botón para eliminar un cliente
    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvClientes.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione un cliente para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim id As Integer = Convert.ToInt32(dgvClientes.SelectedRows(0).Cells("Id").Value)

        Dim resultado As DialogResult = MessageBox.Show("¿Está seguro de eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If resultado = DialogResult.Yes Then
            If clienteDataAccess.EliminarCliente(id) Then
                MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CargarClientes()
            Else
                MessageBox.Show("Error al eliminar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    ' Seleccionar cliente en el DataGridView y cargar datos en los campos de texto
    Private Sub dgvClientes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvClientes.SelectionChanged
        If dgvClientes.SelectedRows.Count > 0 Then
            txtNombre.Text = dgvClientes.SelectedRows(0).Cells("Nombre").Value.ToString()
            txtTelefono.Text = dgvClientes.SelectedRows(0).Cells("Telefono").Value.ToString()
            txtDireccion.Text = dgvClientes.SelectedRows(0).Cells("Direccion").Value.ToString()
        End If
    End Sub
End Class


mi entidad.

Namespace Entities
    Public Class Cliente
        Public Property Id As Integer
        Public Property Nombre As String
        Public Property Direccion As String
        Public Property Telefono As String
    End Class
End Namespace

mi data access 


Imports MySql.Data.MySqlClient
Imports Entities
Imports MiNegocioApp.Entities

Namespace DataAccess
    Public Class ClienteDataAccess
        Private connection As DbConnection

        Public Sub New()
            connection = New DbConnection()
        End Sub

        Public Function ObtenerClientes() As List(Of Cliente)
            Dim clientes As New List(Of Cliente)()

            'Dim query As String = "SELECT * FROM clientes"
            Dim query As String = "CALL ObtenerClientes()" ' Llamada al procedimiento almacenado

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            clientes.Add(New Cliente With {
                                .Id = reader.GetInt32("id"),
                                .Nombre = reader.GetString("nombre"),
                                .Direccion = reader.GetString("direccion"),
                                .Telefono = reader.GetString("telefono")
                            })
                        End While
                    End Using
                End Using
            End Using

            Return clientes
        End Function




        ' Insertar cliente
        Public Function InsertarCliente(nombre As String, direccion As String, telefono As String) As Boolean
            Dim query As String = "CALL InsertarCliente(@nombre, @direccion, @telefono)"

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@nombre", nombre)
                    cmd.Parameters.AddWithValue("@direccion", direccion)
                    cmd.Parameters.AddWithValue("@telefono", telefono)
                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        End Function

        ' Actualizar cliente
        Public Function ActualizarCliente(id As Integer, nombre As String, direccion As String, telefono As String) As Boolean
            Dim query As String = "CALL ActualizarCliente(@id, @nombre, @direccion, @telefono)"

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", id)
                    cmd.Parameters.AddWithValue("@nombre", nombre)
                    cmd.Parameters.AddWithValue("@direccion", direccion)
                    cmd.Parameters.AddWithValue("@telefono", telefono)

                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        End Function


        ' Eliminar cliente
        Public Function EliminarCliente(id As Integer) As Boolean
            Dim query As String = "CALL EliminarCliente(@id)"

            Using conn As MySqlConnection = connection.GetConnection()
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@id", id)

                    Return cmd.ExecuteNonQuery() > 0
                End Using
            End Using
        End Function




    End Class
End Namespace


dime paso a paso como puedo agregar un reporte de clientes personalizado con rdcl

