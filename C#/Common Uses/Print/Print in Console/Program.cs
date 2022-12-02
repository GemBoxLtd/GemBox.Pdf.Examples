using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (PdfDocument document = PdfDocument.Load("Print.pdf"))
        {
            // Print Word document to default printer.
            string printer = null;
            document.Print(printer);
        }
    }
}