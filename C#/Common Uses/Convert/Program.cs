using System.IO;
using System.IO.Compression;
using GemBox.Pdf;
using GemBox.Pdf.Content;

namespace Convert;

static class Program
{
    static void Main()
    {
        Example1();
        Example2();
        Example3();
    }

    static void Example1()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Load a PDF document.
        using var document = PdfDocument.Load("Input.pdf");
        // Create image save options.
        var imageOptions = new ImageSaveOptions(ImageSaveFormat.Jpeg)
        {
            PageNumber = 0, // Select the first PDF page.
            Width = 1240 // Set the image width and keep the aspect ratio.
        };

        // Save a PDF document to a JPEG file.
        document.Save("Output.jpg", imageOptions);
    }

    static void Example2()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Load a PDF document.
        using var document = PdfDocument.Load("Input.pdf");
        var imageOptions = new ImageSaveOptions(ImageSaveFormat.Png);

        // Create a ZIP file for storing PNG files.
        using FileStream archiveStream = File.OpenWrite("Output.zip");
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create);
        // Iterate through the PDF pages.
        for (var pageIndex = 0; pageIndex < document.Pages.Count; pageIndex++)
        {
            // Add a white background color to the page.
            PdfPage page = document.Pages[pageIndex];
            PdfContentElementCollection elements = page.Content.Elements;
            PdfPathContent background = elements.AddPath(elements.First);
            background.AddRectangle(0, 0, page.Size.Width, page.Size.Height);
            background.Format.Fill.IsApplied = true;
            background.Format.Fill.Color = PdfColor.FromRgb(1, 1, 1);

            // Create a ZIP entry for each page.
            ZipArchiveEntry entry = archive.CreateEntry($"Page {pageIndex + 1}.png");

            // Save each page as a PNG image to the ZIP entry.
            using var imageStream = new MemoryStream();
            using Stream entryStream = entry.Open();
            imageOptions.PageNumber = pageIndex;
            document.Save(imageStream, imageOptions);

            imageStream.Position = 0;
            imageStream.CopyTo(entryStream);
        }
    }

    static void Example3()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Load a PDF document.
        using var document = PdfDocument.Load("Input.pdf");
        // Max integer value indicates that all document pages should be saved.
        var imageOptions = new ImageSaveOptions(ImageSaveFormat.Tiff)
        {
            PageCount = int.MaxValue
        };

        // Save the TIFF file with multiple frames, each frame represents a single PDF page.
        document.Save("Output.tiff", imageOptions);
    }
}