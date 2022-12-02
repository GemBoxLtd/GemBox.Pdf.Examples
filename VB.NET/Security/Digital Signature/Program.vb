Imports System.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Annotations
Imports GemBox.Pdf.Content
Imports GemBox.Pdf.Forms
Imports GemBox.Pdf.Security

Module Program

    Sub Main()

        SimpleSignature()

        VisibleSignature()

        MultipleSignature()

        ExternalSignature()

        RemoveSignature()
    End Sub

    Sub SimpleSignature()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add an invisible signature field to the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature()

            ' Get a digital ID from PKCS#12/PFX file.
            Dim digitalId = New PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create the digital signature.
            Dim signer = New PdfSigner(digitalId)

            ' Adobe Acrobat Reader currently doesn't download certificate chain
            ' so we will also embed certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxRSA.crt")}, Nothing, Nothing)

            ' Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer)

            'Finish signing of a PDF file.
            document.Save("Digital Signature.pdf")
        End Using
    End Sub

    Sub VisibleSignature()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a visible signature field to the first page of the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 100)

            ' Retrieve the signature's appearance settings to customize it.
            Dim signatureAppearance = signatureField.Appearance

            ' Signature appearance will consist of a text above an image.
            signatureAppearance.TextPlacement = PdfTextPlacement.TextAboveIcon
            ' Text should occupy 40% of the annotation rectangle height. The rest will be occupied by the image.
            signatureAppearance.TextExtent = 0.4
            ' Text should be right aligned.
            signatureAppearance.TextAlignment = PdfTextAlignment.Right
            ' Set font. A zero value for font size means that the text is auto-sized to fit the annotation rectangle.
            signatureAppearance.Font = New PdfFont("Times New Roman", 0)
            ' Show a 'Reason' label and value.
            signatureAppearance.Reason = "Legal agreement between the seller and the buyer about the purchase"
            ' Show a 'Location' label and value.
            signatureAppearance.Location = "New York, USA"
            ' Do not show a 'Date' label nor value.
            signatureAppearance.DateFormat = String.Empty
            ' Set the signature image.
            signatureAppearance.Icon = PdfImage.Load("GemBoxSignature.png")
            ' The signature image should be scaled only if it is too big to fit.
            signatureAppearance.IconScaleCondition = PdfScaleCondition.ContentTooBig
            ' The signature image should dock to the bottom (y = 0) right (x = 1) corner.
            signatureAppearance.IconAlignment = New PdfPoint(1, 0)

            ' Get a digital ID from PKCS#12/PFX file.
            Dim digitalId = New PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create the digital signature.
            Dim signer = New PdfSigner(digitalId)

            ' Adobe Acrobat Reader currently doesn't download the certificate chain
            ' so we will also embed a certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxRSA.crt")}, Nothing, Nothing)

            ' Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer)

            'Finish signing of a PDF file.
            document.Save("Visible Digital Signature.pdf")
        End Using
    End Sub

    Sub MultipleSignature()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a first signature field to the first page of the PDF document.
            Dim signatureField1 = document.Form.Fields.AddSignature(document.Pages(0), 100, 500, 200, 50)

            ' Get a first digital ID from PKCS#12/PFX file.
            Dim digitalId1 = New PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create the first signature.
            Dim signer1 = New PdfSigner(digitalId1)

            ' Adobe Acrobat Reader currently doesn't download certificate chain
            ' so we will also embed certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer1.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxRSA.crt")}, Nothing, Nothing)

            ' Initiate first signing of a PDF file with the specified signer.
            signatureField1.Sign(signer1)

            ' Finish first signing of a PDF file.
            document.Save("Multiple Digital Signature.pdf")

            ' Add a second signature field to the first page of the PDF document.
            Dim signatureField2 = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Get a second digital ID from PKCS#12/PFX file.
            Dim digitalId2 = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create the second signature.
            Dim signer2 = New PdfSigner(digitalId2)

            ' Adobe Acrobat Reader currently doesn't download certificate chain
            ' so we will also embed certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer2.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxECDsa.crt")}, Nothing, Nothing)

            ' Initiate second signing of a PDF file with the specified signer.
            signatureField2.Sign(signer2)

            ' Finish second signing of a same PDF file.
            document.Save()
        End Using
    End Sub

    Sub ExternalSignature()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a visible signature field to the first page of the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Get a digital ID from XML (private key) and certificate files.
            Dim digitalId = New RSAXmlDigitalId("GemBoxRSA1024PrivateKey.xml", "GemBoxRSA1024.crt")

            ' Create a PDF signer that will create the digital signature.
            Dim signer = New PdfSigner(digitalId)

            ' Adobe Acrobat Reader currently doesn't download certificate chain
            ' so we will also embed certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxRSA.crt")}, Nothing, Nothing)

            ' Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer)

            ' Finish signing of a PDF file.
            document.Save("External Digital Signature.pdf")
        End Using
    End Sub

    Sub RemoveSignature()

        ' If using the Professional version, put your serial key below.
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
