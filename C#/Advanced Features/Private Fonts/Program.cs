using System.Linq;
using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.FontSize = 48;
                formattedText.LineHeight = 72;

                // Use the font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
                formattedText.FontFamily = new PdfFontFamily("Resources", "Almonte Snow");
                formattedText.AppendLine("Hello World 1!");

                // Use the font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
                formattedText.FontFamily = new PdfFontFamily(null, "Resources", "Almonte Woodgrain");
                formattedText.AppendLine("Hello World 2!");

                // Another way to use the font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
                formattedText.FontFamily = PdfFonts.GetFontFamilies("Resources").First(ff => ff.Name == "Almonte Snow");
                formattedText.AppendLine("Hello World 3!");

                // Another way to use the font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
                formattedText.FontFamily = PdfFonts.GetFontFamilies(null, "Resources").First(ff => ff.Name == "Almonte Woodgrain");
                formattedText.Append("Hello World 4!");

                page.Content.DrawText(formattedText, new PdfPoint(100, 500));
            }

            document.Save("Private Fonts.pdf");
        }
    }
}
