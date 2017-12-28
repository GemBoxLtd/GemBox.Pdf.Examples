using System;
using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        PdfDocument document = PdfDocument.Load("Reading.pdf");

        // Get the document outline.
        PdfOutlineCollection documentOutlines = document.Outlines;

        // Remove all bookmarks.
        documentOutlines.Clear();

        // Get the number of pages.
        int numberOfPages = document.Pages.Count;

        for (int i = 0; i < numberOfPages; i += 10)
        {
            // Add a new outline item (bookmark) at the end of the document outline collection.
            PdfOutline bookmark = documentOutlines.AddLast(string.Format("PAGES {0}-{1}", i + 1, Math.Min(i + 10, numberOfPages)));

            // Set the explicit destination on the new outline item (bookmark).
            bookmark.SetDestination(document.Pages[i], PdfDestinationViewType.FitRectangle, 0, 0, 100, 100);

            for (int j = 0; j < Math.Min(10, numberOfPages - i); j++)
            {
                // Add a new outline item (bookmark) at the end of parent outline item (bookmark) and set the explicit destination.
                bookmark.Outlines.AddLast(string.Format("PAGE {0}", i + j + 1)).SetDestination(document.Pages[i + j], PdfDestinationViewType.FitPage);
            }
        }

        document.SaveOptions.CloseOutput = true;
        document.Save("Outlines.pdf");
    }
}
