using GemBox.Pdf;
using GemBox.Pdf.Content.Marked;
using GemBox.Pdf.Objects;

namespace MarkedContent;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = new PdfDocument();
        PdfPage page = document.Pages.Add();

        // Surround the path with the marked content start and marked content end elements.
        GemBox.Pdf.Content.PdfContentMark markStart = page.Content.Elements.AddMarkStart(new PdfContentMarkTag(PdfContentMarkTagRole.Span));

        PdfDictionary markedProperties = markStart.GetEditableProperties().GetDictionary();

        // Set replacement text for a path, as specified in https://opensource.adobe.com/dc-acrobat-sdk-docs/standards/pdfstandards/pdf/PDF32000_2008.pdf#page=623
        markedProperties[PdfName.Create("ActualText")] = PdfString.Create("H");

        // Add the path that is a visual representation of the letter 'H'.
        GemBox.Pdf.Content.PdfPathContent path = page.Content.Elements.AddPath()
            .BeginSubpath(100, 600).LineTo(100, 800)
            .BeginSubpath(100, 700).LineTo(200, 700)
            .BeginSubpath(200, 600).LineTo(200, 800);

        GemBox.Pdf.Content.PdfContentFormat format = path.Format;
        format.Stroke.IsApplied = true;
        format.Stroke.Width = 10;

        page.Content.Elements.AddMarkEnd();

        document.Save("MarkedContent.pdf");
    }
}
