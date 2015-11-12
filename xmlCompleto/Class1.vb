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

    Public Function agregar(id As String, Ipassword As String, ruta As String) As Boolean 'programa que añade un id a un registro en XML
        Dim documento As New XmlDocument



        documento.Load(ruta)
        Dim usuario As XmlElement = documento.CreateElement("usuario") 'Crea u elemento llamado usuario.
        Dim atributoID As XmlAttribute = documento.CreateAttribute("id") 'Crea un atributo 
        atributoID.InnerText = id 'asigna el valor de id 
        usuario.SetAttributeNode(atributoID) 'se le asigna al nodo de usuario el id de atributoID
        Dim password As XmlElement = documento.CreateElement("password") 'Crea un elemento password
        password.InnerText = Ipassword 'Añade valor o texto entre las etiquetas password

        usuario.AppendChild(password) 'cierra el nodo password  hijo de el nodo usuario
        documento.DocumentElement.AppendChild(usuario) 'cierra el nodo usuario
        documento.Save(ruta) 'Guarda el archivo en la ruta otra vez.
        Return True

    End Function

    Function agregar() As Boolean
        Throw New NotImplementedException
    End Function
    Public Function Buscar(ruta As String, path As String, pass As String) As Boolean 'Busca Nodos en el archivo con la pass y el id


        Dim docNav As XPathDocument

        Dim nav As XPathNavigator

        Dim nodeIter As XPathNodeIterator

        Dim strExpression As String

        Try
            docNav = New XPathDocument(ruta)
            nav = docNav.CreateNavigator
            strExpression = "//usuario[@id='" & path & "' and password ='" & pass & "'] " 'XPath

            nodeIter = nav.Select(strExpression)
            If nodeIter.Count() > 0 Then ' si da mas de 0 es que existe
                Return True
            Else
                Return False
            End If
            Return False
        Catch ex As Exception

        End Try










        Return False
    End Function
    Public Sub crearXml(ruta As String, nombre As String, password As String) 'Crear un Xml en la ruta expecificada
        Try
            Dim xmldocument As New XmlTextWriter(ruta, System.Text.Encoding.UTF8)
            xmldocument.Formatting = Formatting.Indented '
            xmldocument.WriteStartDocument()
            xmldocument.WriteStartElement("Usuarios") 'Crea el elemento de apertura de usuarios
            xmldocument.WriteStartElement("usuario") 'Crea el elemento usuario dentro del elemento usuarios. 
            xmldocument.WriteAttributeString("id", nombre) 'Crea un atributo para usuario con el parametro pasado por referencia
            xmldocument.WriteElementString("password", password) 'Crea un nodo password con el strin password
            xmldocument.WriteEndElement() 'cierra elemento
            xmldocument.WriteEndElement() 'cierra elemento
            xmldocument.WriteEndDocument() 'cierra el docuto
            xmldocument.Close() 'Cerramos el documento para que se guarde.
        Catch ex As Exception
            MsgBox("error en la ruta")
        End Try
    End Sub
End Class
