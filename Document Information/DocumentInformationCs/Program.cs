using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Get document information.
            var info = document.Info;

            // Modify document information.
            info.Title = "Document Information Example";
            info.Author = "GemBox.Pdf";
            info.Subject = "Introduction to GemBox.Pdf";
            info.Keywords = "GemBox, Pdf, Examples";

            document.Save("Document Information.pdf");
        }
    }
}
