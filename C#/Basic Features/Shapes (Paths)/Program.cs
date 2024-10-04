using GemBox.Pdf;
using GemBox.Pdf.Content;

namespace Paths;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = new PdfDocument();
        // Add a page.
        PdfPage page = document.Pages.Add();

        // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
        // and the positive y axis extends vertically upward.
        PdfRectangle pageBounds = page.CropBox;

        // Add a thick red line at the top of the page.
        PdfPathContent line = page.Content.Elements.AddPath();
        line.BeginSubpath(new PdfPoint(100, pageBounds.Top - 100)).
            LineTo(new PdfPoint(pageBounds.Right - 100, pageBounds.Top - 200));
        PdfContentFormat lineFormat = line.Format;
        lineFormat.Stroke.IsApplied = true;
        lineFormat.Stroke.Width = 5;
        lineFormat.Stroke.Color = PdfColor.FromRgb(1, 0, 0);

        // Add a filled and stroked rectangle in the middle of the page.
        PdfPathContent rectangle = page.Content.Elements.AddPath();
        // NOTE: The start point of the rectangle is the bottom left corner of the rectangle.
        rectangle.AddRectangle(new PdfPoint(100, pageBounds.Top - 400),
            new PdfSize(pageBounds.Width - 200, 100));
        PdfContentFormat rectangleFormat = rectangle.Format;
        rectangleFormat.Fill.IsApplied = true;
        rectangleFormat.Fill.Color = PdfColor.FromRgb(0, 1, 0);
        rectangleFormat.Stroke.IsApplied = true;
        rectangleFormat.Stroke.Width = 10;
        rectangleFormat.Stroke.Color = PdfColor.FromRgb(0, 0, 1);

        // Add a more complex semi-transparent filled and stroked path at the bottom of the page.
        PdfPathContent shape = page.Content.Elements.AddPath();
        shape.BeginSubpath(new PdfPoint(100, 100)).
            BezierTo(new PdfPoint(100 + pageBounds.Width / 4, 200),
                new PdfPoint(pageBounds.Right - 100 - pageBounds.Width / 4, 0),
                new PdfPoint(pageBounds.Right - 100, 100)).
            LineTo(new PdfPoint(pageBounds.Right - 100, 300)).
            BezierTo(new PdfPoint(pageBounds.Right - 100 - pageBounds.Width / 4, 200),
                new PdfPoint(100 + pageBounds.Width / 4, 400),
                new PdfPoint(100, 300)).
            CloseSubpath();
        PdfContentFormat shapeFormat = shape.Format;
        shapeFormat.Fill.IsApplied = true;
        shapeFormat.Fill.Color = PdfColor.FromRgb(0, 1, 0);
        shapeFormat.Fill.Opacity = 0.5;
        shapeFormat.Stroke.IsApplied = true;
        shapeFormat.Stroke.Width = 4;
        shapeFormat.Stroke.Color = PdfColor.FromRgb(0, 0, 1);
        shapeFormat.Stroke.Opacity = 0.5;
        shapeFormat.Stroke.DashPattern = PdfLineDashPatterns.DashDot;

        // Add a grid to visualize the bounds of each drawn shape.
        PdfPathContent grid = page.Content.Elements.AddPath();
        grid.AddRectangle(new PdfPoint(100, 100),
            new PdfSize(pageBounds.Width - 200, pageBounds.Height - 200));
        grid.BeginSubpath(new PdfPoint(100, pageBounds.Top - 200)).
            LineTo(new PdfPoint(pageBounds.Right - 100, pageBounds.Top - 200)).
            BeginSubpath(new PdfPoint(100, pageBounds.Top - 300)).
            LineTo(new PdfPoint(pageBounds.Right - 100, pageBounds.Top - 300)).
            BeginSubpath(new PdfPoint(100, pageBounds.Top - 400)).
            LineTo(new PdfPoint(pageBounds.Right - 100, pageBounds.Top - 400)).
            BeginSubpath(new PdfPoint(100, 300)).
            LineTo(new PdfPoint(pageBounds.Right - 100, 300));
        grid.Format.Stroke.IsApplied = true;
        // A line width of 0 denotes the thinnest line that can be rendered at device resolution: 1 device pixel wide.
        grid.Format.Stroke.Width = 0;

        document.Save("Paths.pdf");
    }
}
