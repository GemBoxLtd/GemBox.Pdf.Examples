Imports GemBox.Pdf
Imports GemBox.Pdf.Forms
Imports GemBox.Pdf.Security

Module Program

    Sub Main()
        Example1()
        Example2()
        Example3()
    End Sub

    Sub Example1()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Create signed document with author permission.
        Using document = PdfDocument.Load("Reading.pdf")
            Dim textField = document.Form.Fields.AddText(document.Pages(0), 50, 530, 200, 20)
            textField.Name = "Field1"
            textField.Value = "Value before signing"

            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)
            signatureField.Name = "Signature1"

            Dim digitalId = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")
            Dim signer = New PdfSigner(digitalId)

            ' Specify a certification signature with actions that are permitted after certifying the document.
            signer.AuthorPermission = PdfUserAccessPermissions.FillForm

            ' Adobe Acrobat Reader currently doesn't download the certificate chain
            ' so we will also embed a certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxECDsa.crt")}, Nothing, Nothing)

            signatureField.Sign(signer)

            document.Save("SignatureWithFillFormAccess.pdf")
        End Using

        ' We're modifying the field's value of the signed document,
        ' but the signature will remain valid because of the specified PdfUserAccessPermissions.FillForm.
        Using document = PdfDocument.Load("SignatureWithFillFormAccess.pdf")
            Dim textField = DirectCast(document.Form.Fields("Field1"), PdfTextField)
            textField.Value = "Value after signing"
            document.Save()
        End Using
    End Sub

    Sub Example2()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            Dim textField1 = document.Form.Fields.AddText(document.Pages(0), 50, 530, 200, 20)
            textField1.Name = "Text1"
            textField1.Value = "If changed signature is invalid"

            Dim textField2 = document.Form.Fields.AddText(document.Pages(0), 50, 480, 200, 20)
            textField2.Name = "Text2"
            textField2.Value = "If changed signature is still valid"

            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)
            signatureField.Name = "Signature1"
            signatureField.SetLockedFields(textField1)

            Dim digitalId = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")
            Dim signer = New PdfSigner(digitalId)

            ' Adobe Acrobat Reader currently doesn't download the certificate chain
            ' so we will also embed a certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxECDsa.crt")}, Nothing, Nothing)

            signatureField.Sign(signer)

            document.Save("SignatureWithLockedFields.pdf")

        End Using
    End Sub

    Sub Example3()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")
            Dim textField = document.Form.Fields.AddText(document.Pages(0), 50, 530, 200, 20)
            textField.Name = "Field1"
            textField.Value = "Should be filled by the signer"

            ' Signature field that is signed with the author permission.
            Dim authorSignatureField = document.Form.Fields.AddSignature()
            authorSignatureField.Name = "AuthorSignature"

            ' Signature field that will be signed by another signer.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)
            signatureField.Name = "Signature1"
            signatureField.SetLockedFields(textField)
            ' After this signature field is signed, the document is final.
            signatureField.LockedFields.Permission = PdfUserAccessPermissions.None

            Dim certifyingDigitalId = New PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword")
            Dim authorSigner = New PdfSigner(certifyingDigitalId)

            ' Adobe Acrobat Reader currently doesn't download the certificate chain
            ' so we will also embed a certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            authorSigner.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxRSA.crt")}, Nothing, Nothing)

            authorSignatureField.Sign(authorSigner)

            ' Finish first signing of a PDF file.
            document.Save("CertificateAndApprovalSignaturesWorkflow.pdf")

            ' Another signer fills its text field.
            textField.Value = "Filled by another signer"

            ' And signs on its signature field thus making its text field locked.
            Dim approvalDigitalId = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")
            Dim signer = New PdfSigner(approvalDigitalId)
            ' Adobe Acrobat Reader currently doesn't download the certificate chain
            ' so we will also embed a certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxECDsa.crt")}, Nothing, Nothing)
            signatureField.Sign(signer)

            ' Finish second signing of the same PDF file.
            document.Save()
        End Using
    End Sub

End Module
