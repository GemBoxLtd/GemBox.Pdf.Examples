using GemBox.Pdf;

namespace IncrementalUpdateCs
{
    class Program
    {
        static void Main(string[] args)
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // Load a PDF document from a file.
            using (var document = PdfDocument.Load("Hello World.pdf"))
            {
                // Add a new empty page.
                document.Pages.Add();

                // Save all the changes made to the current PDF document using an incremental update.
                document.Save();
            }
        }
    }
}
