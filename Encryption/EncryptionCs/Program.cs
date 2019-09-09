using System;
using GemBox.Pdf;
using GemBox.Pdf.Security;

class Program
{
    static void Main()
    {
        Example1();

        Example2();

        Example3();

        Example4();
    }

    static void Example1()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Load PDF document from an unencrypted PDF file.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Set password-based encryption with password required to open a PDF document.
            document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "user";

            // Save PDF document to an encrypted PDF file.
            document.Save("Encryption.pdf");
        }
    }

    static void Example2()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        try
        {
            // Load PDF document from a potentially encrypted PDF file.
            using (var document = PdfDocument.Load("Encryption.pdf",
                new PdfLoadOptions() { Password = "user" }))
            {
                // Remove encryption from an output PDF file.
                document.SaveOptions.Encryption = null;

                // Save PDF document to an unencrypted PDF file.
                document.Save("Decryption.pdf");
            }
        }
        catch (InvalidPdfPasswordException ex)
        {
            // Gracefully handle the case when input PDF file is encrypted 
            // and provided password is invalid.
            Console.WriteLine(ex.Message);
        }
    }

    static void Example3()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Load PDF document from an unencrypted PDF file.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Set password-based encryption to an output PDF file.
            var encryption = document.SaveOptions.SetPasswordEncryption();

            // Specify password required to edit a PDF document.
            encryption.PermissionsPassword = "owner";

            // User will be able to print PDF and fill-in PDF form 
            // without requiring a password.
            encryption.Permissions =
                PdfUserAccessPermissions.Print |
                PdfUserAccessPermissions.FillForm |
                PdfUserAccessPermissions.CopyContentForAccessibility |
                PdfUserAccessPermissions.PrintHighResolution;

            // Save PDF document to an encrypted PDF file.
            document.Save("Restrict Editing.pdf");
        }
    }

    static void Example4()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Load PDF document from an unencrypted PDF file.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Set password-based encryption to an output PDF file.
            var encryption = document.SaveOptions.SetPasswordEncryption();

            // Specify password required to open a PDF document.
            encryption.DocumentOpenPassword = "user";

            // Specify password required to edit a PDF document.
            encryption.PermissionsPassword = "owner";

            // User will be able to print PDF and fill-in PDF form 
            // without requiring a password.
            encryption.Permissions =
                PdfUserAccessPermissions.Print |
                PdfUserAccessPermissions.FillForm |
                PdfUserAccessPermissions.CopyContentForAccessibility |
                PdfUserAccessPermissions.PrintHighResolution;

            // Specify 256-bit AES encryption level (supported in Acrobat X and later).
            encryption.EncryptionLevel = new PdfEncryptionLevel(PdfEncryptionAlgorithm.AES, 256);

            // Encrypt content and embedded files but do not encrypt document's metadata.
            encryption.Options = PdfEncryptionOptions.EncryptContent | PdfEncryptionOptions.EncryptEmbeddedFiles;

            // Save PDF document to an encrypted PDF file.
            document.Save("Encryption Settings.pdf");
        }
    }
}
