Imports GemBox.Pdf
Imports GemBox.Pdf.Forms
Imports GemBox.Pdf.Security

Module Program

    Sub Main()

        PAdES_B_B()

        PAdES_B_T()

        PAdES_B_LT()

        PAdES_B_LTA()
    End Sub

    Sub PAdES_B_B()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a visible signature field to the first page of the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Get a digital ID from PKCS#12/PFX file.
            Dim digitalId = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create PAdES B-B level signature.
            Dim signer = New PdfSigner(digitalId)

            ' PdfSigner should create CAdES-equivalent signature.
            signer.SignatureFormat = PdfSignatureFormat.CAdES

            ' Adobe Acrobat Reader currently doesn't download certificate chain
            ' so we will also embed certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxECDsa.crt")}, Nothing, Nothing)

            ' Make sure that all properties specified on PdfSigner are according to PAdES B-B level.
            signer.SignatureLevel = PdfSignatureLevel.PAdES_B_B

            ' Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer)

            ' Finish signing of a PDF file.
            document.Save("PAdES B-B.pdf")
        End Using
    End Sub

    Sub PAdES_B_T()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a visible signature field to the first page of the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Get a digital ID from PKCS#12/PFX file.
            Dim digitalId = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create PAdES B-T level signature.
            Dim signer = New PdfSigner(digitalId)

            ' PdfSigner should create CAdES-equivalent signature.
            signer.SignatureFormat = PdfSignatureFormat.CAdES

            ' PdfSigner will embed a timestamp created by freeTSA.org Time Stamp Authority in the signature.
            signer.Timestamper = New PdfTimestamper("https://freetsa.org/tsr")

            ' Adobe Acrobat Reader currently doesn't download certificate chain
            ' so we will also embed certificate of intermediate Certificate Authority in the signature.
            ' (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = New PdfSignatureValidationInfo(New PdfCertificate() {New PdfCertificate("GemBoxECDsa.crt")}, Nothing, Nothing)

            ' Make sure that all properties specified on PdfSigner are according to PAdES B-T level.
            signer.SignatureLevel = PdfSignatureLevel.PAdES_B_T

            ' Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer)

            ' Finish signing of a PDF file.
            document.Save("PAdES B-T.pdf")
        End Using
    End Sub

    Sub PAdES_B_LT()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a visible signature field to the first page of the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Get a digital ID from PKCS#12/PFX file.
            Dim digitalId = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create PAdES B-LT level signature.
            Dim signer = New PdfSigner(digitalId)

            ' PdfSigner should create CAdES-equivalent signature.
            signer.SignatureFormat = PdfSignatureFormat.CAdES

            ' PdfSigner will embed a timestamp created by freeTSA.org Time Stamp Authority in the signature.
            signer.Timestamper = New PdfTimestamper("https://freetsa.org/tsr")

            ' Make sure that all properties specified on PdfSigner are according to PAdES B-LT level.
            signer.SignatureLevel = PdfSignatureLevel.PAdES_B_LT

            ' Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer)

            ' Finish signing of a PDF file.
            document.Save("PAdES B-LT.pdf")

            ' Download validation-related information for the signer's certificate.
            Dim signerValidationInfo = document.SecurityStore.GetValidationInfo(digitalId.Certificate)

            ' Embed validation-related information for the signer's certificate in the PDF file.
            ' This will make the signature "LTV enabled".
            document.SecurityStore.AddValidationInfo(signerValidationInfo)

            ' Save any changes done to the PDF file that were done since the last time Save was called.
            document.Save()
        End Using
    End Sub

    Sub PAdES_B_LTA()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Add a visible signature field to the first page of the PDF document.
            Dim signatureField = document.Form.Fields.AddSignature(document.Pages(0), 300, 500, 250, 50)

            ' Get a digital ID from PKCS#12/PFX file.
            Dim digitalId = New PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword")

            ' Create a PDF signer that will create PAdES B-LTA level signature.
            Dim signer = New PdfSigner(digitalId)

            ' PdfSigner should create CAdES-equivalent signature.
            signer.SignatureFormat = PdfSignatureFormat.CAdES

            ' PdfSigner will embed a timestamp created by freeTSA.org Time Stamp Authority in the signature.
            signer.Timestamper = New PdfTimestamper("https://freetsa.org/tsr")

            ' Make sure that all properties specified on PdfSigner are according to PAdES B-LTA level.
            signer.SignatureLevel = PdfSignatureLevel.PAdES_B_LTA

            ' Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer)

            ' Finish signing of a PDF file.
            document.Save("PAdES B-LTA.pdf")

            ' Download validation-related information for the signature and the signature's timestamp and embed it in the PDF file.
            ' This will make the signature "LTV enabled".
            document.SecurityStore.AddValidationInfo(signatureField.Value)

            ' Add an invisible signature field to the PDF document that will hold the document timestamp.
            Dim timestampField = document.Form.Fields.AddSignature()

            ' Initiate timestamping of a PDF file with the specified timestamper.
            timestampField.Timestamp(signer.Timestamper)

            ' Save any changes done to the PDF file that were done since the last time Save was called and
            ' finish timestamping of a PDF file.
            document.Save()
        End Using
    End Sub
End Module
