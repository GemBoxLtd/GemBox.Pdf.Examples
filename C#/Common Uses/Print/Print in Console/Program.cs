using GemBox.Pdf;

namespace PrintInConsole;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Print.pdf");
        // Print PDF document to default printer (e.g. 'Microsoft Print to Pdf').
        const string printerName = null;
        document.Print(printerName);
    }
}
