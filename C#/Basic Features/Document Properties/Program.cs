using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Get document properties.
            var info = document.Info;

            // Modify document properties.
            info.Title = "Document Properties Example";
            info.Author = "GemBox.Pdf";
            info.Subject = "Introduction to GemBox.Pdf";
            info.Keywords = "GemBox, Pdf, Examples";

            document.Save("Document Properties.pdf");
        }
    }
}
