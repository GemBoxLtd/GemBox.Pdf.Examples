using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // Add a first empty page.
            document.Pages.Add();

            // Add a second empty page.
            document.Pages.Add();

            document.Save("Hello World.pdf");
        }
    }
}
