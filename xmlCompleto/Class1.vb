Imports System.Security.Cryptography
Imports System.Xml
Imports System.Xml.XPath

Public Class Class1

    Private TripleDes As New TripleDESCryptoServiceProvider

    Private Function TruncateHash(
                                 ByVal key As String,
                                 ByVal length As Integer) As Byte()
        Dim sha1 As New SHA1CryptoServiceProvider

        Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ReDim Preserve hash(length - 1)
        Return hash

    End Function
    Sub New(ByVal key As String)
        ' Inicializa el proveedor de cryptografia

        TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)



    End Sub

    Public Function EncryptData(ByVal plainText As String) As String
        ' Comvierte el texto introducido a un array de byte
        Dim textoByte() As Byte =
            System.Text.Encoding.Unicode.GetBytes(plainText)

        'Crea el flujo
        Dim ms As New System.IO.MemoryStream

        Dim encStream As New CryptoStream(ms, TripleDes.CreateEncryptor(),
                                          System.Security.Cryptography.CryptoStreamMode.Write)

        encStream.Write(textoByte, 0, textoByte.Length)
        encStream.FlushFinalBlock()

        Return Convert.ToBase64String(ms.ToArray)
    End Function

    Public Function DecryptData(ByVal encryptedtext As String) As String

        Dim byteEncriptado() As Byte = Convert.FromBase64String(encryptedtext)

        Dim ms As New System.IO.MemoryStream

        Dim decStream As New CryptoStream(ms, TripleDes.CreateDecryptor(),
                                          System.Security.Cryptography.CryptoStreamMode.Write)

        decStream.Write(byteEncriptado, 0, byteEncriptado.Length)
        decStream.FlushFinalBlock()

        Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
    End Function

    Public Function agregar(id As String, Ipassword As String, ruta As String) As Boolean
        Dim documento As New XmlDocument



        documento.Load(ruta)
        Dim usuario As XmlElement = documento.CreateElement("usuario")
        Dim atributoID As XmlAttribute = documento.CreateAttribute("id")
        atributoID.InnerText = id
        usuario.SetAttributeNode(atributoID)
        Dim password As XmlElement = documento.CreateElement("password")
        password.InnerText = Ipassword

        usuario.AppendChild(password)
        documento.DocumentElement.AppendChild(usuario)
        documento.Save(ruta)
        Return True

    End Function

    Function agregar() As Boolean
        Throw New NotImplementedException
    End Function
    Public Function Buscar(ruta As String, path As String, pass As String) As Boolean


        Dim docNav As XPathDocument

        Dim nav As XPathNavigator

        Dim nodeIter As XPathNodeIterator

        Dim strExpression As String

        Try
            docNav = New XPathDocument(ruta)
            nav = docNav.CreateNavigator
            strExpression = "//usuario[@id='" & path & "' and password ='" & pass & "'] "

            nodeIter = nav.Select(strExpression)
            If nodeIter.Count() > 0 Then
                Return True
            Else
                Return False
            End If
            Return False
        Catch ex As Exception

        End Try










        Return False
    End Function
    Public Sub crearXml(ruta As String, nombre As String, password As String)
        Try
            Dim xmldocument As New XmlTextWriter(ruta, System.Text.Encoding.UTF8)
            xmldocument.Formatting = Formatting.Indented
            xmldocument.WriteStartDocument()
            xmldocument.WriteStartElement("Usuarios")
            xmldocument.WriteStartElement("usuario")
            xmldocument.WriteAttributeString("id", nombre)
            xmldocument.WriteElementString("password", password)
            xmldocument.WriteEndElement()
            xmldocument.WriteEndElement()
            xmldocument.WriteEndDocument()
            xmldocument.Close()
        Catch ex As Exception
            MsgBox("error en la ruta")
        End Try
    End Sub
End Class
