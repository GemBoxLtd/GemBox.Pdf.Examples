Imports GemBox.Pdf.Security

''' <summary>
''' Represents a digital ID that reads an RSA private key from an XML file.
''' </summary>
Class RSAXmlDigitalId
    Inherits PdfDigitalId
    Private ReadOnly privateKeyXmlString As String

    Public Sub New(ByVal privateKeyXmlFileName As String, ByVal certificateFileName As String)
        MyBase.New(New PdfCertificate(certificateFileName))
        Me.privateKeyXmlString = System.IO.File.ReadAllText(privateKeyXmlFileName)
    End Sub

    Protected Overrides Function SignHash(ByVal hash As Byte(), ByVal hashAlgorithm As PdfHashAlgorithm, ByVal rsaSignaturePadding As PdfRSASignaturePadding) As Byte()

        Using rsa = System.Security.Cryptography.RSA.Create()

            rsa.FromXmlString(Me.privateKeyXmlString)

            Return rsa.SignHash(
                hash,
                New System.Security.Cryptography.HashAlgorithmName(hashAlgorithm.ToString()),
                If(rsaSignaturePadding = PdfRSASignaturePadding.Pss, System.Security.Cryptography.RSASignaturePadding.Pss, System.Security.Cryptography.RSASignaturePadding.Pkcs1))
        End Using
    End Function
End Class
