using GemBox.Pdf;
using GemBox.Pdf.Forms;
using GemBox.Pdf.Security;
using System;
using System.IO;
using System.Linq;

namespace Namespace
{
    static class Program
    {
        static void Main()
        {
            // Extract GemBoxPkcs11SoftwareModule from ZIP archive and setup environment variable with path to configuration file.
            // Required only for SoftHSM device used in this example. Not required for yours PKCS#11/Cryptoki device.
            var pkcs11SoftwareModuleDirectory = "GemBoxPkcs11SoftwareModule";
            System.IO.Compression.ZipFile.ExtractToDirectory("GemBoxPkcs11SoftwareModule.zip", pkcs11SoftwareModuleDirectory);
            Environment.SetEnvironmentVariable("SOFTHSM2_CONF", Path.Combine(pkcs11SoftwareModuleDirectory, "softhsm2.conf"));

            // Specify path to PKCS#11/Cryptoki library, depending on the runtime architecture (64-bit or 32-bit).
            var libraryPath = Path.Combine(pkcs11SoftwareModuleDirectory, IntPtr.Size == 8 ? "softhsm2-x64.dll" : "softhsm2.dll");

            // If using the Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            using var pkcs11Module = new PdfPkcs11Module(libraryPath);
            // Get a token from PKCS#11/Cryptoki device.
            var token = pkcs11Module.Tokens.Single(t => t.TokenLabel == "GemBoxECDsaToken");

            // Login to the token to get access to protected cryptographic functions.
            token.Login("GemBoxECDsaPin");

            // Get a digital ID from PKCS#11/Cryptoki device token.
            var digitalId = token.DigitalIds.Single(id => id.Certificate.SubjectCommonName == "GemBoxECDsa521");

            using (var document = PdfDocument.Load("Reading.pdf"))
            {
                // Add a visible signature field to the first page of the PDF document.
                var signatureField = document.Form.Fields.AddSignature(document.Pages[0], 300, 500, 250, 50);

                // Create a PDF signer that will create the digital signature.
                var signer = new PdfSigner(digitalId);

                // Adobe Acrobat Reader currently doesn't download certificate chain
                // so we will also embed certificate of intermediate Certificate Authority in the signature.
                // (see https://community.adobe.com/t5/acrobat/signature-validation-using-aia-extension-not-enabled-by-default/td-p/10729647)
                var intermediateCA = token.DigitalIds.Single(id => id.Certificate.SubjectCommonName == "GemBoxECDsa").Certificate;
                signer.ValidationInfo = new PdfSignatureValidationInfo(new PdfCertificate[] { intermediateCA }, null, null);

                // Initiate signing of a PDF file with the specified signer.
                signatureField.Sign(signer);

                // Finish signing of a PDF file.
                document.Save("Digital Signature PKCS#11.pdf");
            }

            token.Logout();
        }
    }
}