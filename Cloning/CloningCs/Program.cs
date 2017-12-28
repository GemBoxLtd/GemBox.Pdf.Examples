using System;
using System.IO;
using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        PdfDocument document = PdfDocument.Load("Invoice.pdf");

        int pageCount = 5;
        string pathToResources = "Resources";

        // Load a source document.
        using (PdfDocument source = PdfDocument.Load(Path.Combine(pathToResources, "Reading.pdf")))
        {
            // Get the number of pages to clone.
            int cloneCount = Math.Min(pageCount, source.Pages.Count);

            // Clone the requested number of pages from the source document
            // and add them to the destination document.
            using (PdfCloneContext context = document.BeginClone(source))
            {
                for (int i = 0; i < cloneCount; i++)
                    document.Pages.AddClone(source.Pages[i]);
            }
        }

        document.SaveOptions.CloseOutput = true;
        document.Save("Cloning.pdf");
    }
}
