Imports System.Security.Cryptography.Pkcs
Imports System.Security.Cryptography.X509Certificates
Imports GemBox.Pdf
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

#If NET40 Then
        Example1()

        Example2()

        Example3()
#End If

        Example4()

        Example5()
    End Sub

#If NET40 Then
    Sub Example1()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add an invisible signature field to the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature()

            ' Initiate signing of a PDF file with the specified digital ID file and the password.
            signatureField.Sign("GemBoxExampleExplorer.pfx", "GemBoxPassword")

            'Finish signing of a PDF file.
            document.Save("Digital Signature.pdf")
        End Using
    End Sub

    Sub Example2()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a visible signature field to the first page of the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Retrieve signature appearance settings to customize it.
            Dim signatureAppearance = signatureField.Appearance

            ' Show 'Reason' label and value.
            signatureAppearance.Reason = "Legal agreement"
            ' Show 'Location' label and value.
            signatureAppearance.Location = "New York, USA"
            ' Do not show 'Date' label nor value.
            signatureAppearance.DateFormat = String.Empty

            ' Initiate signing of a PDF file with the specified digital ID file and the password.
            signatureField.Sign("GemBoxExampleExplorer.pfx", "GemBoxPassword")

            'Finish signing of a PDF file.
            document.Save("Visible Digital Signature.pdf")
        End Using
    End Sub

    Sub Example3()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a first signature field to the first page of the PDF document.
            Dim signatureField1 = document.Form.Fields.AddSignature(document.Pages(0), 100, 500, 200, 50)

            ' Initiate first signing of a PDF file with the specified digital ID file and the password.
            signatureField1.Sign("JohnDoe.pfx", "JohnDoePassword")

            ' Finish first signing of a PDF file.
            document.Save("Multiple Digital Signature.pdf")

            ' Add a second signature field to the first page of the PDF document.
            Dim signatureField2 = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Initiate second signing of a PDF file with the specified digital ID file and the password.
            signatureField2.Sign("GemBoxExampleExplorer.pfx", "GemBoxPassword")

            ' Finish second signing of a same PDF file.
            document.Save()
        End Using
    End Sub
#End If

    Sub Example4()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add an invisible signature field to the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature()

            ' Initiate signing of a PDF file with the specified signer delegate.
            signatureField.Sign(
                Function(pdfFileStream)
                    ' Create a signed CMS object using the content that should be signed,
                    ' but not included in the signed CMS object (detached: true).
                    Dim content = New Byte(CInt(pdfFileStream.Length) - 1) {}
                    pdfFileStream.Read(content, 0, content.Length)
                    Dim signedCms = New SignedCms(New ContentInfo(content), detached:=True)

                    Dim certificate As X509Certificate2 = Nothing
                    Try

                        ' Compute the signature using the specified digital ID file and the password.
                        certificate = New X509Certificate2("GemBoxExampleExplorer.pfx", "GemBoxPassword")
                        signedCms.ComputeSignature(New CmsSigner(certificate))

                    Finally

                        ' Starting with the .NET Framework 4.6, this type implements the IDisposable interface.
                        TryCast(certificate, IDisposable)?.Dispose()
                    End Try

                    ' Return the signature encoded into a CMS/PKCS #7 message.
                    Return signedCms.Encode()

                End Function, PdfSignatureFormat.PKCS7, 2199)

            ' Finish signing of a PDF file.
            document.Save("External Digital Signature.pdf")
        End Using
    End Sub

    Sub Example5()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Multiple Digital Signature.pdf")

            ' Get a list of all signature fields in the document.
            Dim signatureFields = document.Form.Fields.
                Where(Function(f) f.FieldType = PdfFieldType.Signature).
                Cast(Of PdfSignatureField)().
                ToList()

            ' Either remove the signature or the signature field.
            For i As Integer = 0 To signatureFields.Count - 1
                If i Mod 2 = 0 Then
                    signatureFields(i).Value = Nothing
                Else
                    document.Form.Fields.Remove(signatureFields(i))
                End If
            Next

            document.Save("Remove Digital Signature.pdf")
        End Using
    End Sub
End Module
