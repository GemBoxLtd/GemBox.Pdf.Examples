using GemBox.Pdf;

namespace ExportForm;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("FormFilled.pdf");
        document.Form.ExportData("Form Data.fdf");
    }
}
