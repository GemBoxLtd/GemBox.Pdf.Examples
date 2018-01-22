using System;
using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        PdfDocument document = new PdfDocument();

        // Add a first empty page.
        document.Pages.Add();

        // Add a second empty page.
        document.Pages.Add();
        
        document.SaveOptions.CloseOutput = true;
        document.Save("Hello World.pdf");
    }
}
