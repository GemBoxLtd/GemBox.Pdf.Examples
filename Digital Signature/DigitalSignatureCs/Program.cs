using System;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using GemBox.Pdf;
using GemBox.Pdf.Forms;

class Program
{
    static void Main()
    {
#if NET40
        Example1();

        Example2();

        Example3();
#endif

        Example4();

        Example5();
    }

#if NET40
    static void Example1()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add an invisible signature field to the PDF document.
            var signatureField = document.Form.Fields.AddSignature();

            // Initiate signing of a PDF file with the specified digital ID file and the password.
            signatureField.Sign("GemBoxExampleExplorer.pfx", "GemBoxPassword");

            // Finish signing of a PDF file.
            document.Save("Digital Signature.pdf");
        }
    }

    static void Example2()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add a visible signature field to the first page of the PDF document.
            var signatureField = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);

            // Retrieve signature appearance settings to customize it.
            var signatureAppearance = signatureField.Appearance;

            // Show 'Reason' label and value.
            signatureAppearance.Reason = "Legal agreement";
            // Show 'Location' label and value.
            signatureAppearance.Location = "New York, USA";
            // Do not show 'Date' label nor value.
            signatureAppearance.DateFormat = string.Empty;

            // Initiate signing of a PDF file with the specified digital ID file and the password.
            signatureField.Sign("GemBoxExampleExplorer.pfx", "GemBoxPassword");

            // Finish signing of a PDF file.
            document.Save("Visible Digital Signature.pdf");
        }
    }

    static void Example3()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add a first signature field to the first page of the PDF document.
            var signatureField1 = document.Form.Fields.AddSignature(document.Pages[0], 100, 500, 200, 50);

            // Initiate first signing of a PDF file with the specified digital ID file and the password.
            signatureField1.Sign("JohnDoe.pfx", "JohnDoePassword");

            // Finish first signing of a PDF file.
            document.Save("Multiple Digital Signature.pdf");

            // Add a second signature field to the first page of the PDF document.
            var signatureField2 = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);

            // Initiate second signing of a PDF file with the specified digital ID file and the password.
            signatureField2.Sign("GemBoxExampleExplorer.pfx", "GemBoxPassword");

            // Finish second signing of a same PDF file.
            document.Save();
        }
    }
#endif

    static void Example4()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Add an invisible signature field to the PDF document.
            var signatureField = document.Form.Fields.AddSignature();

            // Initiate signing of a PDF file with the specified signer delegate.
            signatureField.Sign(pdfFileStream =>
            {
                    // Create a signed CMS object using the content that should be signed,
                    // but not included in the signed CMS object (detached: true).
                    var content = new byte[pdfFileStream.Length];
                pdfFileStream.Read(content, 0, content.Length);
                var signedCms = new SignedCms(new ContentInfo(content), detached: true);

                X509Certificate2 certificate = null;
                try
                {
                        // Compute the signature using the specified digital ID file and the password.
                        certificate = new X509Certificate2("GemBoxExampleExplorer.pfx", "GemBoxPassword");
                    signedCms.ComputeSignature(new CmsSigner(certificate));
                }
                finally
                {
                        // Starting with the .NET Framework 4.6, this type implements the IDisposable interface.
                        (certificate as IDisposable)?.Dispose();
                }

                    // Return the signature encoded into a CMS/PKCS #7 message.
                    return signedCms.Encode();

            }, PdfSignatureFormat.PKCS7, 2199);

            // Finish signing of a PDF file.
            document.Save("External Digital Signature.pdf");
        }
    }

    static void Example5()
    {
        // If using Professional version, put your serial key below.
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
