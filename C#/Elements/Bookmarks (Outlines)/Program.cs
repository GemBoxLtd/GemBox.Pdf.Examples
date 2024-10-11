using GemBox.Pdf;
using System;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Remove all bookmarks.
            document.Outlines.Clear();

            // Get the number of pages.
            int numberOfPages = document.Pages.Count;

            for (int i = 0; i < numberOfPages; i += 10)
            {
                // Add a new outline item (bookmark) at the end of the document outline collection.
                var bookmark = document.Outlines.AddLast(string.Format("PAGES {0}-{1}", i + 1, Math.Min(i + 10, numberOfPages)));

                // Set the explicit destination on the new outline item (bookmark).
                bookmark.SetDestination(document.Pages[i], PdfDestinationViewType.FitRectangle, 0, 0, 100, 100);

                for (int j = 0; j < Math.Min(10, numberOfPages - i); j++)
                    // Add a new outline item (bookmark) at the end of parent outline item (bookmark) and set the explicit destination.
                    bookmark.Outlines.AddLast(string.Format("PAGE {0}", i + j + 1)).SetDestination(document.Pages[i + j], PdfDestinationViewType.FitPage);
            }

            document.PageMode = PdfPageMode.UseOutlines;

            document.Save("Outlines.pdf");
        }
    }
}
