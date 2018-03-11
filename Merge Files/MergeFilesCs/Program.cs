using System;
using System.IO;
using GemBox.Pdf;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        PdfDocument document = new PdfDocument();

        string pathToResources = "Resources";

        // List of source file names.
        string[] fileNames = new string[] {
            "MergeFile01.pdf",
            "MergeFile02.pdf",
            "MergeFile03.pdf"
        };

        foreach (string fileName in fileNames)
            // Load a source document from the specified path.
            using (PdfDocument source = PdfDocument.Load(Path.Combine(pathToResources, fileName)))
                // Clone all pages from the source document and add them to the destination document.
                foreach (PdfPage page in source.Pages)
                    document.Pages.AddClone(page);

        document.Save("Merge Files.pdf");
        document.Close();
    }
}