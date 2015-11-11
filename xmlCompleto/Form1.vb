Imports System.IO

Public Class Form1

    Private clase As Class1
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click



    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "Ususario"
        Label2.Text = "Contraseña"
        Label3.Text = "Nombre de fichero"
        Button1.Text = "Aceptar"

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim nombreFic As String = TextBox3.Text

        Dim ruta As String = "C:\Users\Yourbroda\Documents\DA32\Acceso a datos\media\" & nombreFic & ".xml"
        Dim pass, usuario, encript As String

        usuario = TextBox1.Text
        pass = TextBox2.Text
        clase = New Class1(pass)
        encript = clase.EncryptData(pass)
        If File.Exists(ruta) Then
            If (clase.Buscar(ruta, usuario, encript)) Then
                MsgBox("Loged")

            Else
                clase.agregar(usuario, encript, ruta)
                MsgBox("Se registro el usuario")

            End If
        Else
            clase.crearXml(ruta, usuario, encript)
            MsgBox("Archivo no encontrado se creara uno nuevo")

        End If



    End Sub
End Class
