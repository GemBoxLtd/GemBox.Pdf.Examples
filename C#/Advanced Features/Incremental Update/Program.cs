using GemBox.Pdf;
using GemBox.Pdf.Content;

namespace IncrementalUpdate;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Load a PDF document from a file.
        using var document = PdfDocument.Load("Hello World.pdf");
        // Add a page.
        var page = document.Pages.Add();

        // Write a text.
        using (var formattedText = new PdfFormattedText())
        {
            formattedText.Append("Hello World again!");

            page.Content.DrawText(formattedText, new PdfPoint(100, 700));
        }

        // Save all the changes made to the current PDF document using an incremental update.
        document.Save();
    }
}