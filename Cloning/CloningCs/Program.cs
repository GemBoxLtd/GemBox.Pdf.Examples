using System;
using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Invoice.pdf"))
        {
            int pageCount = 5;

            // Load a source document.
            using (var source = PdfDocument.Load("LoremIpsum.pdf"))
            {
                // Get the number of pages to clone.
                int cloneCount = Math.Min(pageCount, source.Pages.Count);

                // Clone the requested number of pages from the source document
                // and add them to the destination document.
                for (int i = 0; i < cloneCount; i++)
                    document.Pages.AddClone(source.Pages[i]);
            }

            document.Save("Cloning.pdf");
        }
    }
}
