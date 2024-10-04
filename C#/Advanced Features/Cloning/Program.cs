using System;
using GemBox.Pdf;

namespace Cloning;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Invoice.pdf");
        const int pageCount = 5;

        // Load a source document.
        using (var source = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Get the number of pages to clone.
            var cloneCount = Math.Min(pageCount, source.Pages.Count);

            // Clone the requested number of pages from the source document
            // and add them to the destination document.
            for (var i = 0; i < cloneCount; i++)
            {
                document.Pages.AddClone(source.Pages[i]);
            }
        }

        document.Save("Cloning.pdf");
    }
}