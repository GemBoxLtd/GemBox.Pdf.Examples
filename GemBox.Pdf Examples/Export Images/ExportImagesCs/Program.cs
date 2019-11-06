using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("ExportImages.pdf"))
            // Iterate through PDF pages and through each page's content elements.
            foreach (var page in document.Pages)
                foreach (var contentElement in page.Content.Elements.All())
                    if (contentElement.ElementType == PdfContentElementType.Image)
                    {
                        // Export an image content element to selected image format.
                        var imageContent = (PdfImageContent)contentElement;
                        imageContent.Save("ExportImages.jpg");
                        return;
                    }
    }
}
