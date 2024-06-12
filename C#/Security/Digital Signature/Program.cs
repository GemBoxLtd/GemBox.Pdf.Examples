using GemBox.Pdf;
using GemBox.Pdf.Annotations;
using GemBox.Pdf.Content;
using GemBox.Pdf.Forms;
using GemBox.Pdf.Security;
using System.Linq;

class Program
{
    static void Main()
    {
        Example1();
        Example2();
        Example3();
        Example4();
        Example5();
    }

    static void Example1()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add an invisible signature field to the PDF document.
            var signatureField = document.Form.Fields.AddSignature();

            // Get a digital ID from PKCS#12/PFX file.
            var digitalId = new PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword");

            // Create a PDF signer that will create the digital signature.
            var signer = new PdfSigner(digitalId);

            // Adobe Acrobat Reader currently doesn't download certificate chain
            // so we will also embed certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = new PdfSignatureValidationInfo(new PdfCertificate[] { new PdfCertificate("GemBoxRSA.crt") }, null, null);

            // Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer);

            // Finish signing of a PDF file.
            document.Save("Digital Signature.pdf");
        }
    }

    static void Example2()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add a visible signature field to the first page of the PDF document.
            var signatureField = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 100);

            // Retrieve the signature's appearance settings to customize it.
            var signatureAppearance = signatureField.Appearance;

            // Signature appearance will consist of a text above an image.
            signatureAppearance.TextPlacement = PdfTextPlacement.TextAboveIcon;
            // Text should occupy 40% of the annotation rectangle height. The rest will be occupied by the image.
            signatureAppearance.TextExtent = 0.4;
            // Text should be right aligned.
            signatureAppearance.TextAlignment = PdfTextAlignment.Right;
            // Set font. A zero value for font size means that the text is auto-sized to fit the annotation rectangle.
            signatureAppearance.Font = new PdfFont("Times New Roman", 0);
            // Show a 'Reason' label and value.
            signatureAppearance.Reason = "Legal agreement between the seller and the buyer about the purchase";
            // Show a 'Location' label and value.
            signatureAppearance.Location = "New York, USA";
            // Do not show a 'Date' label nor value.
            signatureAppearance.DateFormat = string.Empty;
            // Set the signature image.
            signatureAppearance.Icon = PdfImage.Load("GemBoxSignature.png");
            // The signature image should be scaled only if it is too big to fit.
            signatureAppearance.IconScaleCondition = PdfScaleCondition.ContentTooBig;
            // The signature image should dock to the bottom (y = 0) right (x = 1) corner.
            signatureAppearance.IconAlignment = new PdfPoint(1, 0);

            // Get a digital ID from PKCS#12/PFX file.
            var digitalId = new PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword");

            // Create a PDF signer that will create the digital signature.
            var signer = new PdfSigner(digitalId);

            // Adobe Acrobat Reader currently doesn't download the certificate chain
            // so we will also embed a certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = new PdfSignatureValidationInfo(new PdfCertificate[] { new PdfCertificate("GemBoxRSA.crt") }, null, null);

            // Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer);

            // Finish signing of a PDF file.
            document.Save("Visible Digital Signature.pdf");
        }
    }

    static void Example3()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add a first signature field to the first page of the PDF document.
            var signatureField1 = document.Form.Fields.AddSignature(document.Pages[0], 100, 500, 200, 50);

            // Get a first digital ID from PKCS#12/PFX file.
            var digitalId1 = new PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword");

            // Create a PDF signer that will create the first signature.
            var signer1 = new PdfSigner(digitalId1);

            // Adobe Acrobat Reader currently doesn't download certificate chain
            // so we will also embed certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer1.ValidationInfo = new PdfSignatureValidationInfo(new PdfCertificate[] { new PdfCertificate("GemBoxRSA.crt") }, null, null);

            // Initiate first signing of a PDF file with the specified signer.
            signatureField1.Sign(signer1);

            // Finish first signing of a PDF file.
            document.Save("Multiple Digital Signature.pdf");

            // Add a second signature field to the first page of the PDF document.
            var signatureField2 = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);

            // Get a second digital ID from PKCS#12/PFX file.
            var digitalId2 = new PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword");

            // Create a PDF signer that will create the second signature.
            var signer2 = new PdfSigner(digitalId2);

            // Adobe Acrobat Reader currently doesn't download certificate chain
            // so we will also embed certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer2.ValidationInfo = new PdfSignatureValidationInfo(new PdfCertificate[] { new PdfCertificate("GemBoxECDsa.crt") }, null, null);

            // Initiate second signing of a PDF file with the specified signer.
            signatureField2.Sign(signer2);

            // Finish second signing of the same PDF file.
            document.Save();
        }
    }

    static void Example4()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add a visible signature field to the first page of the PDF document.
            var signatureField = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);

            // Get a digital ID from XML (private key) and certificate files.
            var digitalId = new RSAXmlDigitalId("GemBoxRSA1024PrivateKey.xml", "GemBoxRSA1024.crt");

            // Create a PDF signer that will create the digital signature.
            var signer = new PdfSigner(digitalId);

            // Adobe Acrobat Reader currently doesn't download certificate chain
            // so we will also embed certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            signer.ValidationInfo = new PdfSignatureValidationInfo(new PdfCertificate[] { new PdfCertificate("GemBoxRSA.crt") }, null, null);

            // Initiate signing of a PDF file with the specified signer.
            signatureField.Sign(signer);

            // Finish signing of a PDF file.
            document.Save("External Digital Signature.pdf");
        }
    }

    static void Example5()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Multiple Digital Signature.pdf"))
        {
            // Get a list of all signature fields in the document.
            var signatureFields = document.Form.Fields.
                Where(f => f.FieldType == PdfFieldType.Signature).
                Cast<PdfSignatureField>().
                ToList();

            // Either remove the signature or the signature field.
            for (int i = 0; i < signatureFields.Count; ++i)
                if (i % 2 == 0)
                    signatureFields[i].Value = null;
                else
                    document.Form.Fields.Remove(signatureFields[i]);

            document.Save("Remove Digital Signature.pdf");
        }
    }
}
