using System;
using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        PdfDocument document = PdfDocument.Create();

        // Get a page tree root node.
        PdfPages rootNode = document.Pages;
        // Set page rotation for a whole set of pages.
        rootNode.Rotate = 90;

        // Create a left page tree node.
        PdfPages childNode = rootNode.Kids.AddPages();
        // Overwrite a parent tree node rotation value.
        childNode.Rotate = 0;

        // Create an empty page.
        childNode.Kids.AddPage();
        // Create an empty page and set a page media box value.
        childNode.Kids.AddPage().SetMediaBox(0, 0, 400, 400);

        // Create a right page tree node.
        childNode = rootNode.Kids.AddPages();
        // Set a media box value.
        childNode.SetMediaBox(0, 0, 200, 200);

        // Create an empty page.
        childNode.Kids.AddPage();
        // Create an empty page and overwrite a rotation value.
        childNode.Kids.AddPage().Rotate = 0;

        document.SaveOptions.CloseOutput = true;
        document.Save("Page Tree.pdf");
    }
}
