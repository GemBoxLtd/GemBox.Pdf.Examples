using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using GemBox.Pdf;

namespace SplitFile;

static class Program
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
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Open a source PDF file and create a destination ZIP file.
        using var source = PdfDocument.Load("Chapters.pdf");
        using FileStream archiveStream = File.OpenWrite("Output.zip");
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create);
        // Iterate through the PDF pages.
        for (var pageIndex = 0; pageIndex < source.Pages.Count; pageIndex++)
        {
            // Create a ZIP entry for each source document page.
            ZipArchiveEntry entry = archive.CreateEntry($"Page {pageIndex + 1}.pdf");

            // Save each page as a separate destination document to the ZIP entry.
            using Stream entryStream = entry.Open();
            using var destination = new PdfDocument();
            destination.Pages.AddClone(source.Pages[pageIndex]);
            destination.Save(entryStream);
        }
    }

    static void Example2()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // List of page numbers used for splitting the PDF document.
        var pageRanges = new[]
        {
            new { FirstPageIndex = 0, LastPageIndex = 2 },
            new { FirstPageIndex = 3, LastPageIndex = 3 },
            new { FirstPageIndex = 4, LastPageIndex = 6 }
        };

        // Open a source PDF file and create a destination ZIP file.
        using var source = PdfDocument.Load("Chapters.pdf");
        using FileStream archiveStream = File.OpenWrite("OutputRanges.zip");
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create);
        // Iterate through page ranges.
        foreach (var pageRange in pageRanges)
        {
            var pageIndex = pageRange.FirstPageIndex;
            var pageCount = Math.Min(pageRange.LastPageIndex + 1, source.Pages.Count);

            ZipArchiveEntry entry = archive.CreateEntry($"Pages {pageIndex + 1}-{pageCount}.pdf");
            using Stream entryStream = entry.Open();
            using var destination = new PdfDocument();
            // Add range of source pages to destination document.
            while (pageIndex < pageCount)
            {
                destination.Pages.AddClone(source.Pages[pageIndex++]);
            }

            // Save destination document to the ZIP entry.
            destination.Save(entryStream);
        }
    }

    static void Example3()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var source = PdfDocument.Load("Chapters.pdf");
        using FileStream archiveStream = File.OpenWrite("OutputBookmarks.zip");
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create);
        var pageIndexes = source.Pages
            .Select((page, index) => new { page, index })
            .ToDictionary(item => item.page, item => item.index);

        // Iterate through document outlines.
        PdfOutlineCollection outlines = source.Outlines;
        for (var index = 0; index < outlines.Count; ++index)
        {
            PdfOutline currentOutline = outlines[index];
            PdfOutline nextOutline = index + 1 < outlines.Count ? outlines[index + 1] : null;

            var pageIndex = pageIndexes[currentOutline.Destination.Page];
            var pageCount = nextOutline == null ? source.Pages.Count : pageIndexes[nextOutline.Destination.Page];

            ZipArchiveEntry entry = archive.CreateEntry($"{currentOutline.Title}.pdf");
            using Stream entryStream = entry.Open();
            using var destination = new PdfDocument();
            // Add source pages from current bookmark till next bookmark to destination document.
            while (pageIndex < pageCount)
            {
                destination.Pages.AddClone(source.Pages[pageIndex++]);
            }

            // Save destination document to the ZIP entry.
            destination.Save(entryStream);
        }
    }

    static void Example4()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var source = PdfDocument.Load("lorem-ipsum-1000-pages.pdf");
        const int chunkSize = 220;

        var pageIndex = 0;
        var pageCount = source.Pages.Count;
        while (pageIndex < pageCount)
        {
            // Split large PDF file into multiple PDF files of specified chunk size.
            using (var destination = new PdfDocument())
            {
                var chunkCount = Math.Min(chunkSize + pageIndex, pageCount);
                var chunkName = $"Pages {pageIndex + 1}-{chunkCount}.pdf";

                while (pageIndex < chunkCount)
                {
                    destination.Pages.AddClone(source.Pages[pageIndex++]);
                }

                destination.Save(Path.Combine("Split Large Pdf", chunkName));
            }

            // Clear previously parsed pages and thus free memory necessary for reading additional pages.
            source.Unload();
        }
    }
}
