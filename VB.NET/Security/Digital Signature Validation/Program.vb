Imports System
Imports GemBox.Pdf
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Multiple Digital Signature.pdf")

            For Each field In document.Form.Fields
                If field.FieldType = PdfFieldType.Signature Then

                    Dim signatureField = CType(field, PdfSignatureField)

                    Dim signature = signatureField.Value

                    If signature IsNot Nothing Then

                        Dim signatureValidationResult = signature.Validate()

                        If signatureValidationResult.IsValid Then

                            Console.Write("Signature '{0}' is VALID, signed by '{1}'. ", signatureField.Name, signature.Content.SignerCertificate.SubjectCommonName)
                            Console.WriteLine("The document has not been modified since this signature was applied.")

                        Else

                            Console.Write("Signature '{0}' is INVALID. ", signatureField.Name)
                            Console.WriteLine("The document has been altered or corrupted since the signature was applied.")
                        End If
                    End If
                End If
            Next
        End Using
    End Sub
End Module
