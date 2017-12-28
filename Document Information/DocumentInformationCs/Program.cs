using System;
using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        PdfDocument document = PdfDocument.Load("Reading.pdf");

        // Get document information.
        PdfDocumentInformation info = document.Info;

        // Modify document information.
        info.Title = "Document Information Example";
        info.Author = "GemBox.Pdf";
        info.Subject = "Introduction to GemBox.Pdf";
        info.Keywords = "GemBox, Pdf, Examples";

        document.SaveOptions.CloseOutput = true;
        document.Save("Document Information.pdf");
    }
}
