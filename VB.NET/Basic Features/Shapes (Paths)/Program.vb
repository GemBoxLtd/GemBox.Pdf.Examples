Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            ' Add a page.
            Dim page = document.Pages.Add()

            ' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
            ' and the positive y axis extends vertically upward.
            Dim pageBounds = page.CropBox

            ' Add a thick red line at the top of the page.
            Dim line = page.Content.Elements.AddPath()
            line.BeginSubpath(New PdfPoint(100, pageBounds.Top - 100)).
                LineTo(New PdfPoint(pageBounds.Right - 100, pageBounds.Top - 200))
            Dim lineFormat = line.Format
            lineFormat.Stroke.IsApplied = True
            lineFormat.Stroke.Width = 5
            lineFormat.Stroke.Color = PdfColor.FromRgb(1, 0, 0)

            ' Add a filled and stroked rectangle in the middle of the page.
            Dim rectangle = page.Content.Elements.AddPath()
            ' NOTE: The start point of the rectangle is the bottom left corner of the rectangle.
            rectangle.AddRectangle(New PdfPoint(100, pageBounds.Top - 400),
                                   New PdfSize(pageBounds.Width - 200, 100))
            Dim rectangleFormat = rectangle.Format
            rectangleFormat.Fill.IsApplied = True
            rectangleFormat.Fill.Color = PdfColor.FromRgb(0, 1, 0)
            rectangleFormat.Stroke.IsApplied = True
            rectangleFormat.Stroke.Width = 10
            rectangleFormat.Stroke.Color = PdfColor.FromRgb(0, 0, 1)

            ' Add a more complex semi-transparent filled and dashed path at the bottom of the page.
            Dim shape = page.Content.Elements.AddPath()
            shape.BeginSubpath(New PdfPoint(100, 100)).
                BezierTo(New PdfPoint(100 + pageBounds.Width / 4, 200),
                         New PdfPoint(pageBounds.Right - 100 - pageBounds.Width / 4, 0),
                         New PdfPoint(pageBounds.Right - 100, 100)).
                LineTo(New PdfPoint(pageBounds.Right - 100, 300)).
                BezierTo(New PdfPoint(pageBounds.Right - 100 - pageBounds.Width / 4, 200),
                         New PdfPoint(100 + pageBounds.Width / 4, 400),
                         New PdfPoint(100, 300)).
                CloseSubpath()
            Dim shapeFormat = shape.Format
            shapeFormat.Fill.IsApplied = True
            shapeFormat.Fill.Color = PdfColor.FromRgb(0, 1, 0)
            shapeFormat.Fill.Opacity = 0.5
            shapeFormat.Stroke.IsApplied = True
            shapeFormat.Stroke.Width = 4
            shapeFormat.Stroke.Color = PdfColor.FromRgb(0, 0, 1)
            shapeFormat.Stroke.Opacity = 0.5
            shapeFormat.Stroke.DashPattern = PdfLineDashPatterns.DashDot

            ' Add a grid to visualize the bounds of each drawn shape.
            Dim grid = page.Content.Elements.AddPath()
            grid.AddRectangle(New PdfPoint(100, 100),
                              New PdfSize(pageBounds.Width - 200, pageBounds.Height - 200))
            grid.BeginSubpath(New PdfPoint(100, pageBounds.Top - 200)).
                LineTo(New PdfPoint(pageBounds.Right - 100, pageBounds.Top - 200)).
                BeginSubpath(New PdfPoint(100, pageBounds.Top - 300)).
                LineTo(New PdfPoint(pageBounds.Right - 100, pageBounds.Top - 300)).
                BeginSubpath(New PdfPoint(100, pageBounds.Top - 400)).
                LineTo(New PdfPoint(pageBounds.Right - 100, pageBounds.Top - 400)).
                BeginSubpath(New PdfPoint(100, 300)).
                LineTo(New PdfPoint(pageBounds.Right - 100, 300))
            grid.Format.Stroke.IsApplied = True
            ' A line width of 0 denotes the thinnest line that can be rendered at device resolution: 1 device pixel wide.
            grid.Format.Stroke.Width = 0

            document.Save("Paths.pdf")
        End Using
    End Sub
End Module