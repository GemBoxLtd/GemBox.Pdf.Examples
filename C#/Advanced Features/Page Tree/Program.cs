using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.Stop;

        using (var document = new PdfDocument())
        {
            using (var formattedText = new PdfFormattedText())
            {
                // Get a page tree root node.
                var rootNode = document.Pages;
                // Set page rotation for a whole set of pages.
                rootNode.Rotate = 90;

                // Create a left page tree node.
                var childNode = rootNode.Kids.AddPages();
                // Overwrite a parent tree node rotation value.
                childNode.Rotate = 0;

                // Create a first page.
                var page = childNode.Kids.AddPage();
                formattedText.Append("FIRST PAGE");
                page.Content.DrawText(formattedText, new PdfPoint(0, 0));

                // Create a second page and set a page media box value.
                page = childNode.Kids.AddPage();
                page.SetMediaBox(0, 0, 200, 400);
                formattedText.Clear();
                formattedText.Append("SECOND PAGE");
                page.Content.DrawText(formattedText, new PdfPoint(0, 0));

                // Create a right page tree node.
                childNode = rootNode.Kids.AddPages();
                // Set a media box value.
                childNode.SetMediaBox(0, 0, 100, 200);

                // Create a third page.
                page = childNode.Kids.AddPage();
                formattedText.Clear();
                formattedText.Append("THIRD PAGE");
                page.Content.DrawText(formattedText, new PdfPoint(0, 0));

                // Create a fourth page and overwrite a rotation value.
                page = childNode.Kids.AddPage();
                page.Rotate = 0;
                formattedText.Clear();
                formattedText.Append("FOURTH PAGE");
                page.Content.DrawText(formattedText, new PdfPoint(0, 0));
            }

            document.Save("Page Tree.pdf");
        }
    }
}
