using GemBox.Pdf;
using GemBox.Pdf.Content.Marked;
using GemBox.Pdf.Objects;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            var markStart = page.Content.Elements.AddMarkStart(new PdfContentMarkTag(PdfContentMarkTagRole.Span));

            // Surround the path with marked content start and marked content end elements.
            var markedProperties = markStart.GetEditableProperties().GetDictionary();

            // Set replacement text for a path, as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=623
            markedProperties[PdfName.Create("ActualText")] = PdfString.Create("H");

            // Add the path that is a visual representation of a letter 'H'.
            var path = page.Content.Elements.AddPath();
            path.
                BeginSubpath(100, 600).LineTo(100, 800).
                BeginSubpath(100, 700).LineTo(200, 700).
                BeginSubpath(200, 600).LineTo(200, 800);
            var format = path.Format;
            format.Stroke.IsApplied = true;
            format.Stroke.Width = 10;

            page.Content.Elements.AddMarkEnd();

            document.Save("MarkedContent.pdf");
        }
    }
}