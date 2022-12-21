using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        Example1();
        Example2();
        Example3();
        Example4();
    }

    static void Example1()
    {
        // Open a source PDF file and create a destination ZIP file.
        using (var source = PdfDocument.Load("Chapters.pdf"))
        using (var archiveStream = File.OpenWrite("Output.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
        {
            // Iterate through the PDF pages.
            for (int pageIndex = 0; pageIndex < source.Pages.Count; pageIndex++)
            {
                // Create a ZIP entry for each source document page.
                var entry = archive.CreateEntry($"Page {pageIndex + 1}.pdf");

                // Save each page as a separate destination document to the ZIP entry.
                using (var entryStream = entry.Open())
                using (var destination = new PdfDocument())
                {
                    destination.Pages.AddClone(source.Pages[pageIndex]);
                    destination.Save(entryStream);
                }
            }
        }
    }

    static void Example2()
    {
        // List of page numbers used for splitting the PDF document.
        var pageRanges = new[]
        {
            new { FirstPageIndex = 0, LastPageIndex = 2 },
            new { FirstPageIndex = 3, LastPageIndex = 3 },
            new { FirstPageIndex = 4, LastPageIndex = 6 }
        };

        // Open a source PDF file and create a destination ZIP file.
        using (var source = PdfDocument.Load("Chapters.pdf"))
        using (var archiveStream = File.OpenWrite("OutputRanges.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
        {
            // Iterate through page ranges.
            foreach (var pageRange in pageRanges)
            {
                int pageIndex = pageRange.FirstPageIndex;
                int pageCount = Math.Min(pageRange.LastPageIndex + 1, source.Pages.Count);

                var entry = archive.CreateEntry($"Pages {pageIndex + 1}-{pageCount}.pdf");
                using (var entryStream = entry.Open())
                using (var destination = new PdfDocument())
                {
                    // Add range of source pages to destination document.
                    while (pageIndex < pageCount)
                        destination.Pages.AddClone(source.Pages[pageIndex++]);

                    // Save destination document to the ZIP entry.
                    destination.Save(entryStream);
                }
            }
        }
    }

    static void Example3()
    {
        using (var source = PdfDocument.Load("Chapters.pdf"))
        using (var archiveStream = File.OpenWrite("OutputBookmarks.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create))
        {
            Dictionary<PdfPage, int> pageIndexes = source.Pages
                .Select((page, index) => new { page, index })
                .ToDictionary(item => item.page, item => item.index);

            // Iterate through document outlines.
            var outlines = source.Outlines;
            for (int index = 0; index < outlines.Count; ++index)
            {
                var currentOutline = outlines[index];
                var nextOutline = index + 1 < outlines.Count ? outlines[index + 1] : null;

                int pageIndex = pageIndexes[currentOutline.Destination.Page];
                int pageCount = nextOutline == null ? source.Pages.Count : pageIndexes[nextOutline.Destination.Page];

                var entry = archive.CreateEntry($"{currentOutline.Title}.pdf");
                using (var entryStream = entry.Open())
                using (var destination = new PdfDocument())
                {
                    // Add source pages from current bookmark till next bookmark to destination document.
                    while (pageIndex < pageCount)
                        destination.Pages.AddClone(source.Pages[pageIndex++]);

                    // Save destination document to the ZIP entry.
                    destination.Save(entryStream);
                }
            }
        }
    }

    static void Example4()
    {
        using (var source = PdfDocument.Load("lorem-ipsum-1000-pages.pdf"))
        {
            int chunkSize = 220;

            int pageIndex = 0;
            int pageCount = source.Pages.Count;
            while (pageIndex < pageCount)
            {
                // Split large PDF file into multiple PDF files of specified chunk size.
                using (var destination = new PdfDocument())
                {
                    int chunkCount = Math.Min(chunkSize + pageIndex, pageCount);
                    string chunkName = $"Pages {pageIndex + 1}-{chunkCount}.pdf";

                    while (pageIndex < chunkCount)
                        destination.Pages.AddClone(source.Pages[pageIndex++]);

                    destination.Save(Path.Combine("Split Large Pdf", chunkName));
                }

                // Clear previously parsed pages and thus free memory necessary for reading additional pages.
                source.Unload();
            }
        }
    }
}
