using System;
using GemBox.Pdf;
using GemBox.Pdf.Forms;

namespace DigitalSignatureValidation;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Multiple Digital Signature.pdf");
        foreach (PdfField field in document.Form.Fields)
        {
            if (field.FieldType == PdfFieldType.Signature)
            {
                var signatureField = (PdfSignatureField)field;

                PdfSignature signature = signatureField.Value;

                if (signature != null)
                {
                    PdfSignatureValidationResult signatureValidationResult = signature.Validate();

                    if (signatureValidationResult.IsValid)
                    {
                        Console.Write("Signature '{0}' is VALID, signed by '{1}'. ", signatureField.Name, signature.Content.SignerCertificate.SubjectCommonName);
                        Console.WriteLine("The document has not been modified since this signature was applied.");
                    }
                    else
                    {
                        Console.Write("Signature '{0}' is INVALID. ", signatureField.Name);
                        Console.WriteLine("The document has been altered or corrupted since the signature was applied.");
                    }
                }
            }
        }
    }
}
