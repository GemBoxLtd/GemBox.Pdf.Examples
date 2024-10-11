using GemBox.Document;
using GemBox.Pdf;
using GemBox.Pdf.Content;
using GemBox.Presentation;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Charts;
using System.IO;

class Program
{
    static void Main()
    {
        Example1();
        Example2();
        Example3();
    }

    static void Example1()
    {
        // If using the Professional version, put your GemBox.Pdf serial key below.
        GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // If using the Professional version, put your GemBox.Spreadsheet serial key below.
        GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();
            double x = 50;
            double y = page.Size.Height;

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Append("The following chart is imported from a PDF that was created with GemBox.Spreadsheet.");
                page.Content.DrawText(formattedText, new PdfPoint(x, y - 50));
            }

            // Create chart and save it as PDF stream.
            var chart = CreateChart(400, 200);
            var chartAsPdf = new MemoryStream();
            chart.Format().Save(chartAsPdf, GemBox.Spreadsheet.SaveOptions.PdfDefault);

            // Add chart to PDF page.
            using (var chartDocument = PdfDocument.Load(chartAsPdf))
                document.AppendPage(chartDocument, 0, 0, new PdfPoint(x, y - chart.Position.Height - 60));

            document.Save("Chart.pdf");
        }
    }

    static ExcelChart CreateChart(double width, double height)
    {
        var workbook = new ExcelFile();
        var worksheet = workbook.Worksheets.Add("Chart");

        worksheet.Cells["A1"].Value = "Name";
        worksheet.Cells["A2"].Value = "John Doe";
        worksheet.Cells["A3"].Value = "Fred Nurk";
        worksheet.Cells["A4"].Value = "Hans Meier";
        worksheet.Cells["A5"].Value = "Ivan Horvat";

        worksheet.Cells["B1"].Value = "Salary";
        worksheet.Cells["B2"].Value = 3600;
        worksheet.Cells["B3"].Value = 2580;
        worksheet.Cells["B4"].Value = 3200;
        worksheet.Cells["B5"].Value = 4100;

        worksheet.Columns[1].Style.NumberFormat = "\"$\"#,##0";

        var chart = worksheet.Charts.Add(GemBox.Spreadsheet.Charts.ChartType.Bar,
            new AnchorCell(worksheet.Cells["A1"], true), width, height, GemBox.Spreadsheet.LengthUnit.Point);

        chart.SelectData(worksheet.Cells.GetSubrangeAbsolute(0, 0, 4, 1), true);
        return chart;
    }

    static void Example2()
    {
        // If using the Professional version, put your GemBox.Pdf serial key below.
        GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // If using the Professional version, put your GemBox.Document serial key below.
        GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();
            double x = 50;
            double y = page.Size.Height;

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Append("The following QR code is imported from a PDF that was created with GemBox.Document.");
                page.Content.DrawText(formattedText, new PdfPoint(x, y - 50));
            }

            // Create barcode and save it as PDF stream.
            var barcode = CreateBarcode("1234567890");
            var barcodeAsPdf = new MemoryStream();
            barcode.FormatDrawing().Save(barcodeAsPdf, GemBox.Document.SaveOptions.PdfDefault);

            // Add barcode to PDF page.
            using (var barcodeDocument = PdfDocument.Load(barcodeAsPdf))
                document.AppendPage(barcodeDocument, 0, 0, new PdfPoint(x, y - barcode.Layout.Size.Height - 60));

            document.Save("Barcode.pdf");
        }
    }

    static GemBox.Document.TextBox CreateBarcode(string qrCode)
    {
        var document = new DocumentModel();
        document.DefaultParagraphFormat.SpaceAfter = 0;
        document.DefaultParagraphFormat.LineSpacing = 1;

        var textBox = new GemBox.Document.TextBox(document, Layout.Inline(0, 0, GemBox.Document.LengthUnit.Point),
            new Paragraph(document,
                new Field(document, FieldType.DisplayBarcode, $"{qrCode} QR")));

        document.Sections.Add(
            new GemBox.Document.Section(document,
                new Paragraph(document, textBox)));

        textBox.TextBoxFormat.InternalMargin = new Padding(0);
        textBox.TextBoxFormat.AutoFit = GemBox.Document.TextAutoFit.ResizeShapeToFitText;
        document.GetPaginator(new GemBox.Document.PaginatorOptions() { UpdateTextBoxHeights = true });

        double size = textBox.Layout.Size.Height;
        textBox.Layout.Size = new Size(size, size);

        return textBox;
    }

    static void Example3()
    {
        // If using the Professional version, put your GemBox.Pdf serial key below.
        GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // If using the Professional version, put your GemBox.Presentation serial key below.
        GemBox.Presentation.ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();
            double x = 50;
            double y = page.Size.Height;

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Append("The following shapes are imported from a PDF that was created with GemBox.Presentation.");
                page.Content.DrawText(formattedText, new PdfPoint(x, y - 50));
            }

            // Create shapes and save them as PDF stream.
            var shapes = CreateShapes();
            var shapesAsPdf = new MemoryStream();
            shapes.Save(shapesAsPdf, GemBox.Presentation.SaveOptions.Pdf);

            // Add shapes to PDF page.
            using (var shapesDocument = PdfDocument.Load(shapesAsPdf))
                document.AppendPage(shapesDocument, 0, 0, new PdfPoint(0, y - shapes.SlideSize.Height - 60));

            document.Save("Shapes.pdf");
        }
    }

    static PresentationDocument CreateShapes()
    {
        var presentation = new PresentationDocument();
        var slide = presentation.Slides.AddNew(SlideLayoutType.Custom);

        slide.Content.AddShape(ShapeGeometryType.RectangularCallout, 30, 30, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.AliceBlue));
        slide.Content.AddShape(ShapeGeometryType.RoundedRectangularCallout, 170, 30, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.BlueViolet));
        slide.Content.AddShape(ShapeGeometryType.OvalCallout, 310, 30, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.CadetBlue));
        slide.Content.AddShape(ShapeGeometryType.Pentagon, 450, 30, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.CornflowerBlue));

        slide.Content.AddShape(ShapeGeometryType.RoundedRectangle, 30, 150, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.DarkSeaGreen));
        slide.Content.AddShape(ShapeGeometryType.RegularPentagon, 170, 150, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.ForestGreen));
        slide.Content.AddShape(ShapeGeometryType.Hexagon, 310, 150, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.GreenYellow));
        slide.Content.AddShape(ShapeGeometryType.Octagon, 450, 150, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.LightSeaGreen));

        slide.Content.AddShape(ShapeGeometryType.UpArrow, 30, 270, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.DarkRed));
        slide.Content.AddShape(ShapeGeometryType.RightArrow, 170, 270, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.IndianRed));
        slide.Content.AddShape(ShapeGeometryType.UpDownArrow, 310, 270, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.OrangeRed));
        slide.Content.AddShape(ShapeGeometryType.LeftRightArrow, 450, 270, 130, 100, GemBox.Presentation.LengthUnit.Point).Format.Fill.SetSolid(GemBox.Presentation.Color.FromName(GemBox.Presentation.ColorName.MediumVioletRed));

        return presentation;
    }
}

public static class PdfDocumentExtension
{
    public static PdfFormContent AppendPage(this PdfDocument destination, PdfDocument source,
        int sourcePageIndex, int destinationPageIndex, PdfPoint destinationBottomLeft)
    {
        var form = source.Pages[sourcePageIndex].ConvertToForm(destination);
        var group = destination.Pages[destinationPageIndex].Content.Elements.AddGroup();

        var formContent = group.Elements.AddForm(form);
        formContent.Transform = PdfMatrix.CreateTranslation(destinationBottomLeft.X, destinationBottomLeft.Y);
        return formContent;
    }
}
