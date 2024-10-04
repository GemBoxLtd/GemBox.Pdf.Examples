using GemBox.Pdf;

namespace FillinForm;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Form.pdf");
        document.Form.Fields["FullName"].Value = "Jane Doe";
        document.Form.Fields["ID"].Value = "0123456789";
        document.Form.Fields["Gender"].Value = "Female";
        document.Form.Fields["Married"].Value = "Yes";
        document.Form.Fields["City"].Value = "Berlin";
        document.Form.Fields["Language"].Value = new string[] { "German", "Italian" };
        document.Form.Fields["Notes"].Value = "Notes first line\rNotes second line\rNotes third line";

        document.Save("FormFilled.pdf");
    }
}