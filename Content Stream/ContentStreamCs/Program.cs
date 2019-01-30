using System.Text;
using GemBox.Pdf;
using GemBox.Pdf.Filters;
using GemBox.Pdf.Objects;
using GemBox.Pdf.Text;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // Specify content stream's content as a sequence of content stream operands and operators.
            var content = new StringBuilder();
            // Begin a text object.
            content.AppendLine("BT");
            // Set the font and font size to use, installing them as parameters in the text state.
            // In this case, the font resource identified by the name F1 specifies the font externally known as Helvetica.
            content.AppendLine("/F1 12 Tf");
            // Specify a starting position on the page, setting parameters in the text object.
            content.AppendLine("70 760 Td");
            // Paint the glyphs for a string of characters at that position.
            content.AppendLine("(GemBox.Pdf) Tj");
            // End the text object.
            content.AppendLine("ET");

            // Create content stream and write content to it.
            var contentStream = PdfStream.Create();
            contentStream.Filters.AddFilter(PdfFilterType.FlateDecode);

            using (var stream = contentStream.Open(PdfStreamDataMode.Write, PdfStreamDataState.Decoded))
            {
                var contentBytes = PdfEncoding.Byte.GetBytes(content.ToString());
                stream.Write(contentBytes, 0, contentBytes.Length);
            }

            // Create font dictionary for Standard Type 1 'Helvetica' font.
            var font = PdfDictionary.Create();
            font[PdfName.Create("Type")] = PdfName.Create("Font");
            font[PdfName.Create("Subtype")] = PdfName.Create("Type1");
            font[PdfName.Create("BaseFont")] = PdfName.Create("Helvetica");

            // Add font dictionary to resources.
            var fontResources = PdfDictionary.Create();
            fontResources[PdfName.Create("F1")] = PdfIndirectObject.Create(font);

            var resources = PdfDictionary.Create();
            resources[PdfName.Create("Font")] = fontResources;

            // Create new empty A4 page.
            var page = document.Pages.Add();

            // Set contents and resources of a page.
            var pageDictionary = page.GetDictionary();
            pageDictionary[PdfName.Create("Contents")] = PdfIndirectObject.Create(contentStream);
            pageDictionary[PdfName.Create("Resources")] = resources;

            document.Save("Content Stream.pdf");
        }
    }
}
