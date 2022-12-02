using System.IO;
using System.IO.Compression;
using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        var fileNameWithoutExt = Path.GetFileNameWithoutExtension("LoremIpsum.pdf");

        // Open source PDF file and create a destination ZIP file.
        using (var source = PdfDocument.Load("LoremIpsum.pdf"))
        using (var archiveStream = File.OpenWrite($"{fileNameWithoutExt}.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            for (int index = 0; index < source.Pages.Count; index++)
            {
                // Create new ZIP entry for each source document page.
                var entry = archive.CreateEntry($"{fileNameWithoutExt}{index + 1}.pdf");

                // Open ZIP entry stream.
                using (var entryStream = entry.Open())
                // Create destination document.
                using (var destination = new PdfDocument())
                {
                    // Clone source document page to destination document.
                    destination.Pages.AddClone(source.Pages[index]);

                    // Save destination document to ZIP entry stream.
                    destination.Save(entryStream);
                }
            }
    }
}
