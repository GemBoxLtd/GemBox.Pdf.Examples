using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfCorePages.Models;
using GemBox.Pdf;
using GemBox.Pdf.Content;

namespace PdfCorePages.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public new FileModel File { get; set; }

        public IndexModel()
        {
            this.File = new FileModel();

            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public void OnGet() { }

        public FileContentResult OnPost()
        {
            // Create new document.
            var document = new PdfDocument();

            // Add page.
            var page = document.Pages.Add();

            // Write text.
            using (var formattedText = new PdfFormattedText())
            {
                // Write header.
                formattedText.TextAlignment = PdfTextAlignment.Center;
                formattedText.FontSize = 18;
                formattedText.MaxTextWidth = 400;

                formattedText.Append(this.File.Header);
                page.Content.DrawText(formattedText, new PdfPoint(90, 750));

                // Write body.
                formattedText.Clear();
                formattedText.TextAlignment = PdfTextAlignment.Justify;
                formattedText.FontSize = 14;

                formattedText.Append(this.File.Body);
                page.Content.DrawText(formattedText, new PdfPoint(90, 400));

                // Write footer.
                formattedText.Clear();
                formattedText.TextAlignment = PdfTextAlignment.Right;
                formattedText.FontSize = 10;
                formattedText.MaxTextWidth = 100;

                formattedText.Append(this.File.Footer);
                page.Content.DrawText(formattedText, new PdfPoint(450, 40));
            }

            // Save PDF file.
            var stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            // Download file.
            return File(stream.ToArray(), "application/pdf", "OutputFromPage.pdf");
        }
    }
}

namespace PdfCorePages.Models
{
    public class FileModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Header { get; set; } = "Header text from ASP.NET Core Razor Pages";
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Body { get; set; } = string.Join(" ", Enumerable.Repeat("Lorem ipsum dolor sit amet, consectetuer adipiscing elit.", 4));
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Footer { get; set; } = "Page 1 of 1";
    }
}
