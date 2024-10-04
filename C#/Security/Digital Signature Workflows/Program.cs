using GemBox.Pdf;
using GemBox.Pdf.Forms;
using GemBox.Pdf.Security;

namespace DigitalSignatureWorkflows;

static class Program
{
    static void Main()
    {
        Example1();
        Example2();
        Example3();
    }

    static void Example1()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Create signed document with author permission.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            PdfTextField textField = document.Form.Fields.AddText(document.Pages[0], 50, 530, 200, 20);
            textField.Name = "Field1";
            textField.Value = "Value before signing";

            PdfSignatureField signatureField = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);
            signatureField.Name = "Signature1";

            var digitalId = new PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword");
            var signer = new PdfSigner(digitalId)
            {
                // Specify a certification signature with actions that are permitted after certifying the document.
                AuthorPermission = PdfUserAccessPermissions.FillForm,

                // Adobe Acrobat Reader currently doesn't download the certificate chain
                // so we will also embed a certificate of intermediate Certificate Authority in the signature.
                // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
                ValidationInfo = new PdfSignatureValidationInfo([new("GemBoxECDsa.crt")], null, null)
            };

            signatureField.Sign(signer);

            document.Save("SignatureWithFillFormAccess.pdf");
        }

        // We're modifying the field's value of the signed document,
        // but the signature will remain valid because of the specified PdfUserAccessPermissions.FillForm.
        using (var document = PdfDocument.Load("SignatureWithFillFormAccess.pdf"))
        {
            var textField = (PdfTextField)document.Form.Fields["Field1"];
            textField.Value = "Value after signing";
            document.Save();
        }
    }

    static void Example2()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Reading.pdf");
        PdfTextField textField1 = document.Form.Fields.AddText(document.Pages[0], 50, 530, 200, 20);
        textField1.Name = "Text1";
        textField1.Value = "If changed signature is invalid";

        PdfTextField textField2 = document.Form.Fields.AddText(document.Pages[0], 50, 480, 200, 20);
        textField2.Name = "Text2";
        textField2.Value = "If changed signature is still valid";

        PdfSignatureField signatureField = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);
        signatureField.Name = "Signature1";
        signatureField.SetLockedFields(textField1);

        var digitalId = new PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword");
        var signer = new PdfSigner(digitalId)
        {
            // Adobe Acrobat Reader currently doesn't download the certificate chain
            // so we will also embed a certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            ValidationInfo = new PdfSignatureValidationInfo([new("GemBoxECDsa.crt")], null, null)
        };

        signatureField.Sign(signer);

        document.Save("SignatureWithLockedFields.pdf");
    }

    static void Example3()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Reading.pdf");
        PdfTextField textField = document.Form.Fields.AddText(document.Pages[0], 50, 530, 200, 20);
        textField.Name = "Field1";
        textField.Value = "Should be filled by the signer";

        // Signature field that is signed with the author permission.
        PdfSignatureField authorSignatureField = document.Form.Fields.AddSignature();
        authorSignatureField.Name = "AuthorSignature";

        // Signature field that will be signed by another signer.
        PdfSignatureField signatureField = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);
        signatureField.Name = "Signature1";
        signatureField.SetLockedFields(textField);
        // After this signature field is signed, the document is final.
        signatureField.LockedFields.Permission = PdfUserAccessPermissions.None;

        var certifyingDigitalId = new PdfDigitalId("GemBoxRSA1024.pfx", "GemBoxPassword");
        var authorSigner = new PdfSigner(certifyingDigitalId)
        {
            // Specify a certification signature with actions that are permitted after certifying the document.
            AuthorPermission = PdfUserAccessPermissions.FillForm,

            // Adobe Acrobat Reader currently doesn't download the certificate chain
            // so we will also embed a certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            ValidationInfo = new PdfSignatureValidationInfo([new("GemBoxRSA.crt")], null, null)
        };

        authorSignatureField.Sign(authorSigner);

        // Finish first signing of a PDF file.
        document.Save("CertificateAndApprovalSignaturesWorkflow.pdf");

        // Another signer fills its text field.
        textField.Value = "Filled by another signer";

        // And signs on its signature field thus making its text field locked.
        var approvalDigitalId = new PdfDigitalId("GemBoxECDsa521.pfx", "GemBoxPassword");
        var signer = new PdfSigner(approvalDigitalId)
        {
            // Adobe Acrobat Reader currently doesn't download the certificate chain
            // so we will also embed a certificate of intermediate Certificate Authority in the signature.
            // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
            ValidationInfo = new PdfSignatureValidationInfo([new("GemBoxECDsa.crt")], null, null)
        };
        signatureField.Sign(signer);

        // Finish second signing of the same PDF file.
        document.Save();
    }
}
