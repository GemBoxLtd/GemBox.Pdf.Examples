using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.FontSize = 48;
                formattedText.LineHeight = 72;

                // Use the font family 'Almonte Snow' whose font file is located in the 'MyFonts' directory.
                formattedText.FontFamily = new PdfFontFamily("MyFonts", "Almonte Snow");
                formattedText.AppendLine("Hello World!");

                page.Content.DrawText(formattedText, new PdfPoint(100, 500));
            }

            document.Save("Private Fonts.pdf");
        }
    }
}
