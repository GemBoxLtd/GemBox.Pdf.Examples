using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // List of source file names.
        var fileNames = new string[]
        {
            "MergeFile01.pdf",
            "MergeFile02.pdf",
            "MergeFile03.pdf"
        };

        using (var document = new PdfDocument())
        {
            foreach (var fileName in fileNames)
                // Load a source document from the specified path.
                using (var source = PdfDocument.Load(fileName))
                    // Clone all pages from the source document and add them to the destination document.
                    foreach (var page in source.Pages)
                        document.Pages.AddClone(page);

            document.Save("Merge Files.pdf");
        }
    }
}