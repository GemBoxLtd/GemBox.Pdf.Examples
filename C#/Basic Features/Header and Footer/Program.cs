using System;
using System.Globalization;
using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        ComponentInfo.FreeLimitReached += (sender, e) => e.FreeLimitReachedAction = FreeLimitReachedAction.Stop;

        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            double marginLeft = 20, marginTop = 10, marginRight = 20, marginBottom = 10;

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));

                // Add a header with the current date and time to all pages.
                foreach (var page in document.Pages)
                {
                    // Set the location of the bottom-left corner of the text.
                    // We want the top-left corner of the text to be at location (marginLeft, marginTop)
                    // from the top-left corner of the page.
                    // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                    // and the positive y axis extends vertically upward.
                    double x = marginLeft, y = page.CropBox.Top - marginTop - formattedText.Height;

                    page.Content.DrawText(formattedText, new PdfPoint(x, y));
                }

                // Add a footer with the current page number to all pages.
                int pageCount = document.Pages.Count, pageNumber = 0;
                foreach (var page in document.Pages)
                {
                    ++pageNumber;

                    formattedText.Clear();
                    formattedText.Append(string.Format("Page {0} of {1}", pageNumber, pageCount));

                    // Set the location of the bottom-left corner of the text.
                    double x = page.CropBox.Width - marginRight - formattedText.Width, y = marginBottom;

                    page.Content.DrawText(formattedText, new PdfPoint(x, y));
                }
            }

            document.Save("Header and Footer.pdf");
        }
    }
}