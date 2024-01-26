Imports System
Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports GemBox.Pdf.Content.Colors
Imports GemBox.Pdf.Content.Patterns
Imports GemBox.Pdf.Objects
Imports GemBox.Pdf.Text

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Example1()
        Example2()
        Example3()
        Example4()
        Example5()
        Example6()

    End Sub

    Sub Example1()
        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            ' PdfFormattedText currently supports just Device color spaces (DeviceGray, DeviceRGB, and DeviceCMYK).
            Using formattedText = New PdfFormattedText()

                formattedText.Font = New PdfFont("Helvetica", 100)

                ' Make the text fill black (in DeviceGray color space) and 50% opaque.
                formattedText.Color = PdfColor.FromGray(0)
                ' In PDF, opacity is defined separately from the color.
                formattedText.Opacity = 0.5
                formattedText.Append("Hello world!")

                page.Content.DrawText(formattedText, New PdfPoint(50, 700))
            End Using

            ' Path filled with non-zero winding number rule.
            Dim path = page.Content.Elements.AddPath()
            Dim center = New PdfPoint(300, 500)
            Dim radius As Double = 150, cos1 As Double = Math.Cos(Math.PI / 10), sin1 As Double = Math.Sin(Math.PI / 10), cos2 As Double = Math.Cos(Math.PI / 5), sin2 As Double = Math.Sin(Math.PI / 5)
            ' Create a five-point star.
            path.
                BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius). ' Start from the point in the bottom-left corner.
                LineTo(center.X + cos1 * radius, center.Y + sin1 * radius). ' Continue to the point in the upper-right corner.
                LineTo(center.X - cos1 * radius, center.Y + sin1 * radius). ' Continue to the point in the upper-left corner.
                LineTo(center.X + sin2 * radius, center.Y - cos2 * radius). ' Continue to the point in the bottom-right corner.
                LineTo(center.X, center.Y + radius). ' Continue to the point in the upper-center.
                CloseSubpath() ' End with the starting point.
            Dim format = path.Format
            format.Fill.IsApplied = True
            format.Fill.Rule = PdfFillRule.NonzeroWindingNumber
            ' Make the path fill red (in DeviceRGB color space) and 40% opaque.
            format.Fill.Color = PdfColor.FromRgb(1, 0, 0)
            format.Fill.Opacity = 0.4

            ' Path filled with even-odd rule.
            path = page.Content.Elements.AddClone(path)
            path.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -300))
            format = path.Format
            format.Fill.IsApplied = True
            format.Fill.Rule = PdfFillRule.EvenOdd
            ' Make the path fill yellow (in DeviceCMYK color space) and 60% opaque.
            format.Fill.Color = PdfColor.FromCmyk(0, 0, 1, 0)
            format.Fill.Opacity = 0.6

            document.Save("Filling.pdf")
        End Using
    End Sub

    Sub Example2()
        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            ' PdfFormattedText currently doesn't support stroking, so we will stroke its drawn output.
            Using formattedText = New PdfFormattedText()

                formattedText.Font = New PdfFont("Helvetica", 200)

                formattedText.Append("Hello!")

                page.Content.DrawText(formattedText, New PdfPoint(50, 600))
            End Using

            ' Draw lines with different line joins.
            Dim path = page.Content.Elements.AddPath()
            path.BeginSubpath(50, 350).LineTo(100, 550).LineTo(150, 350)
            path.Format.Stroke.LineJoin = PdfLineJoin.Miter

            path = page.Content.Elements.AddPath()
            path.BeginSubpath(200, 350).LineTo(250, 550).LineTo(300, 350)
            path.Format.Stroke.LineJoin = PdfLineJoin.Round

            path = page.Content.Elements.AddPath()
            path.BeginSubpath(350, 350).LineTo(400, 550).LineTo(450, 350)
            path.Format.Stroke.LineJoin = PdfLineJoin.Bevel

            ' Create a dash pattern with 20 point dashes and 10 point gaps.
            Dim dashPattern = New PdfLineDashPattern(0, 20, 10)

            ' Draw lines with different line caps.
            ' Notice how the line cap is applied to each dash.
            path = page.Content.Elements.AddPath()
            path.BeginSubpath(50, 100).LineTo(100, 300).LineTo(150, 100)
            Dim format = path.Format
            format.Stroke.LineCap = PdfLineCap.Butt
            format.Stroke.DashPattern = dashPattern

            path = page.Content.Elements.AddPath()
            path.BeginSubpath(200, 100).LineTo(250, 300).LineTo(300, 100)
            format = path.Format
            format.Stroke.LineCap = PdfLineCap.Round
            format.Stroke.DashPattern = dashPattern

            path = page.Content.Elements.AddPath()
            path.BeginSubpath(350, 100).LineTo(400, 300).LineTo(450, 100)
            format = path.Format
            format.Stroke.LineCap = PdfLineCap.Square
            format.Stroke.DashPattern = dashPattern

            ' Do not fill any content and stroke all content with a 10 point width red outline that is 50% opaque.
            format = page.Content.Format
            format.Fill.IsApplied = False
            format.Stroke.IsApplied = True
            format.Stroke.Width = 10
            format.Stroke.Color = PdfColor.FromRgb(1, 0, 0)
            format.Stroke.Opacity = 0.5

            ' Add a line to visualize the differences between line joins.
            Dim line = page.Content.Elements.AddPath().BeginSubpath(25, 550).LineTo(475, 550)
            format = line.Format
            format.Stroke.IsApplied = True
            ' A line width of 0 denotes the thinnest line that can be rendered at device resolution: 1 device pixel wide.
            format.Stroke.Width = 0

            ' Add a line to visualize the differences between line caps.
            line = page.Content.Elements.AddPath().BeginSubpath(25, 100).LineTo(475, 100)
            format = line.Format
            format.Stroke.IsApplied = True
            format.Stroke.Width = 0

            document.Save("Stroking.pdf")
        End Using
    End Sub

    Sub Example3()
        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            ' Add a new content group. Clipping is localized to the content group.
            Dim textGroup = page.Content.Elements.AddGroup()
            ' Draw text in the content group.
            Using formattedText = New PdfFormattedText()

                formattedText.Font = New PdfFont("Helvetica", 96)

                formattedText.Append("Hello world!")

                textGroup.DrawText(formattedText, New PdfPoint(50, 700))
            End Using
            ' Stroke all text elements in the group (to visualize their bounds) and set them as a clipping path.
            Dim format = textGroup.Format
            format.Fill.IsApplied = False
            format.Stroke.IsApplied = True
            format.Clip.IsApplied = True
            ' Draw an image in the same content group as the text.
            ' The image will be clipped to the text.
            Dim image = PdfImage.Load("Acme.png")
            textGroup.DrawImage(image, New PdfPoint(50, 700), New PdfSize(500, 100))

            ' Add a new content group. Clipping is localized to the content group.
            Dim pathGroup = page.Content.Elements.AddGroup()
            ' Add a diamond-like path to the content group.
            pathGroup.Elements.AddPath().BeginSubpath(50, 550).LineTo(300, 500).LineTo(550, 550).LineTo(300, 600).CloseSubpath()
            ' Stroke all path elements in the group (to visualize their bounds) and set them as a clipping path.
            format = pathGroup.Format
            format.Fill.IsApplied = False
            format.Stroke.IsApplied = True
            format.Clip.IsApplied = True
            ' Draw an image in the same content group as the diamond-like path.
            ' The image will be clipped to the diamond-like path.
            pathGroup.DrawImage(image, New PdfPoint(50, 500), New PdfSize(500, 100))

            ' Add a new content group. Clipping is localized to the content group.
            pathGroup = page.Content.Elements.AddGroup()
            ' Add a star-like path to the content group.
            Dim path = pathGroup.Elements.AddPath()
            Dim center = New PdfPoint(150, 300)
            Dim radius As Double = 100, cos1 As Double = Math.Cos(Math.PI / 10), sin1 As Double = Math.Sin(Math.PI / 10), cos2 As Double = Math.Cos(Math.PI / 5), sin2 As Double = Math.Sin(Math.PI / 5)
            ' Create a five-point star.
            path.
                BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius). ' Start from the point in the bottom-left corner.
                LineTo(center.X + cos1 * radius, center.Y + sin1 * radius). ' Continue to the point in the upper-right corner.
                LineTo(center.X - cos1 * radius, center.Y + sin1 * radius). ' Continue to the point in the upper-left corner.
                LineTo(center.X + sin2 * radius, center.Y - cos2 * radius). ' Continue to the point in the bottom-right corner.
                LineTo(center.X, center.Y + radius). ' Continue to the point in the upper-center.
                CloseSubpath() ' End with the starting point.
            ' Stroke a path (to visualize its bounds) and set it as a clipping path using non-zero winding number rule.
            format = path.Format
            format.Fill.IsApplied = False
            format.Stroke.IsApplied = True
            format.Clip.IsApplied = True
            format.Clip.Rule = PdfFillRule.NonzeroWindingNumber
            ' Draw an image in the same content group as the star-like path.
            ' The image will be clipped to the star-like path using non-zero winding number rule.
            pathGroup.DrawImage(image, New PdfPoint(50, 200), New PdfSize(200, 200))

            ' Add a new content group. Clipping is localized to the content group.
            pathGroup = page.Content.Elements.AddGroup()
            ' Clone a star-like path to the content group and move it down.
            path = pathGroup.Elements.AddClone(path)
            path.Subpaths.Transform(PdfMatrix.CreateTranslation(250, 0))
            ' Set the clipping rule to even-odd.
            path.Format.Clip.Rule = PdfFillRule.EvenOdd
            ' Draw an image in the same content group as the star-like path.
            ' The image will be clipped to the star-like path using the even-odd rule.
            pathGroup.DrawImage(image, New PdfPoint(300, 200), New PdfSize(200, 200))

            document.Save("Clipping.pdf")
        End Using
    End Sub

    Sub Example4()
        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            ' PdfFormattedText currently supports just Device color spaces (DeviceGray, DeviceRGB, and DeviceCMYK).
            Using formattedText = New PdfFormattedText()

                formattedText.Font = New PdfFont("Helvetica", 24)

                ' Three different ways to specify gray color in the DeviceGray color space:
                formattedText.Color = PdfColors.Gray
                formattedText.Append("Hello world! ")
                formattedText.Color = PdfColor.FromGray(0.5)
                formattedText.Append("Hello world! ")
                formattedText.Color = New PdfColor(PdfColorSpace.DeviceGray, 0.5)
                formattedText.AppendLine("Hello world!")

                ' Three different ways to specify red color in the DeviceRGB color space:
                formattedText.Color = PdfColors.Red
                formattedText.Append("Hello world! ")
                formattedText.Color = PdfColor.FromRgb(1, 0, 0)
                formattedText.Append("Hello world! ")
                formattedText.Color = New PdfColor(PdfColorSpace.DeviceRGB, 1, 0, 0)
                formattedText.AppendLine("Hello world!")

                ' Three different ways to specify yellow color in the DeviceCMYK color space:
                formattedText.Color = PdfColors.Yellow
                formattedText.Append("Hello world! ")
                formattedText.Color = PdfColor.FromCmyk(0, 0, 1, 0)
                formattedText.Append("Hello world! ")
                formattedText.Color = New PdfColor(PdfColorSpace.DeviceCMYK, 0, 0, 1, 0)
                formattedText.Append("Hello world!")

                page.Content.DrawText(formattedText, New PdfPoint(100, 500))
            End Using

            ' Create an Indexed color space (which is currently not supported by GemBox.Pdf)
            ' as specified in https://opensource.adobe.com/dc-acrobat-sdk-docs/standards/pdfstandards/pdf/PDF32000_2008.pdf#page=164
            ' Base color space is DeviceRGB and the created Indexed color space consists of two colors:
            ' at index 0: green color (0x00FF00)
            ' at index 1: blue color (0x0000FF)
            Dim indexedColorSpaceArray = PdfArray.Create(4)
            indexedColorSpaceArray.Add(PdfName.Create("Indexed"))
            indexedColorSpaceArray.Add(PdfName.Create("DeviceRGB"))
            indexedColorSpaceArray.Add(PdfInteger.Create(1))
            indexedColorSpaceArray.Add(PdfString.Create(ChrW(0) & ChrW(&HFF) & ChrW(0) & ChrW(0) & ChrW(0) & ChrW(&HFF), PdfEncoding.Byte, PdfStringForm.Hexadecimal))
            Dim indexedColorSpace = PdfColorSpace.FromArray(indexedColorSpaceArray)

            ' Add a rectangle.
            ' Fill it with color at index 0 (green) of the Indexed color space.
            ' Stroke it with color at index 1 (blue) of the Indexed color space.
            Dim path = page.Content.Elements.AddPath()
            path.AddRectangle(100, 300, 200, 100)
            Dim format = path.Format
            format.Fill.IsApplied = True
            format.Fill.Color = New PdfColor(indexedColorSpace, 0)
            format.Stroke.IsApplied = True
            format.Stroke.Color = New PdfColor(indexedColorSpace, 1)
            format.Stroke.Width = 5

            document.Save("Colors.pdf")
        End Using
    End Sub

    Sub Example5()
        Using document = New PdfDocument()

            ' The uncolored tiling pattern should not specify the color of its content, instead the outer element that uses the uncolored tiling pattern will specify the color of the tiling pattern content.
            Dim uncoloredTilingPattern = New PdfTilingPattern(document, New PdfSize(100, 100)) With {.IsColored = False}
            ' Begin editing the pattern cell.
            uncoloredTilingPattern.Content.BeginEdit()
            ' The tiling pattern cell contains two triangles that are filled with color specified by the outer element that uses the uncolored tiling pattern.
            Dim path = uncoloredTilingPattern.Content.Elements.AddPath()
            path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath()
            path.Format.Fill.IsApplied = True
            path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath()
            path.Format.Fill.IsApplied = True
            ' End editing the pattern cell.
            uncoloredTilingPattern.Content.EndEdit()

            ' Create an uncolored tiling Pattern color space (which is currently not supported by GemBox.Pdf)
            ' as specified in https://opensource.adobe.com/dc-acrobat-sdk-docs/standards/pdfstandards/pdf/PDF32000_2008.pdf#page=186.
            ' The underlying color space is DeviceRGB and colorants will be specified in DeviceRGB.
            Dim uncoloredTilingPatternColorSpaceArray = PdfArray.Create(2)
            uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("Pattern"))
            uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("DeviceRGB"))
            Dim uncoloredTilingPatternColorSpace = PdfColorSpace.FromArray(uncoloredTilingPatternColorSpaceArray)

            Dim page = document.Pages.Add()

            ' Add a background rectangle over the entire page that shows how the tiling pattern, by default, starts from the bottom-left corner of the page.
            Dim mediaBox = page.MediaBox
            Dim backgroundRect = page.Content.Elements.AddPath()
            backgroundRect.AddRectangle(mediaBox.Left, mediaBox.Bottom, mediaBox.Width, mediaBox.Height)
            Dim format = backgroundRect.Format
            format.Fill.IsApplied = True
            format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 0)
            format.Fill.Opacity = 0.2

            ' Add a rectangle that is filled with the red (red = 1, green = 0, blue = 0) pattern.
            Dim redRect = page.Content.Elements.AddPath()
            redRect.AddRectangle(75, 575, 200, 100)
            format = redRect.Format
            format.Fill.IsApplied = True
            format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 1, 0, 0)
            format.Stroke.IsApplied = True

            ' Add a rectangle that is filled with the same pattern, but this time the pattern's color is green (red = 0, green = 1, blue = 0).
            Dim greenRect = page.Content.Elements.AddClone(redRect)
            greenRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150))
            greenRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 1, 0)

            ' Add a rectangle that is filled with the same pattern, but this time the pattern's color is blue (red = 0, green = 0, blue = 1).
            Dim blueRect = page.Content.Elements.AddClone(greenRect)
            blueRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150))
            blueRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 1)

            ' The colored tiling pattern specifies the color of its content.
            Dim coloredTilingPattern = New PdfTilingPattern(document, New PdfSize(100, 100))
            ' Begin editing the pattern cell.
            coloredTilingPattern.Content.BeginEdit()
            ' The tiling pattern cell contains two triangles. The first one is filled with the red color and the second one is filled with the green color.
            path = coloredTilingPattern.Content.Elements.AddPath()
            path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath()
            format = path.Format
            format.Fill.IsApplied = True
            format.Fill.Color = PdfColors.Red
            path = coloredTilingPattern.Content.Elements.AddPath()
            path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath()
            format = path.Format
            format.Fill.IsApplied = True
            format.Fill.Color = PdfColors.Green
            ' End editing the pattern cell.
            coloredTilingPattern.Content.EndEdit()

            ' Add a rectangle that is filled with the colored (red-green) tiling pattern.
            Dim redGreenRect = page.Content.Elements.AddPath()
            redGreenRect.AddRectangle(325, 275, 200, 400)
            format = redGreenRect.Format
            format.Fill.IsApplied = True
            format.Fill.Color = PdfColor.FromPattern(coloredTilingPattern)
            format.Stroke.IsApplied = True

            document.Save("Patterns.pdf")
        End Using
    End Sub

    Sub Example6()
        Using document = New PdfDocument()

            ' Shading transitions the colors in the axis from location (0, 0) to location (250, 250).
            Dim startPoint = New PdfPoint(0, 0)
            Dim size = New PdfSize(250, 250)

            ' Create axial shading as specified in https://opensource.adobe.com/dc-acrobat-sdk-docs/standards/pdfstandards/pdf/PDF32000_2008.pdf#page=193
            Dim shading = New PdfAxialShading(startPoint, New PdfPoint(startPoint.X + size.Width, startPoint.Y + size.Height), PdfColors.Red, PdfColors.Green)

            Dim shadingPattern = New PdfShadingPattern(document, shading)

            Dim page = document.Pages.Add()

            ' Add a background rectangle over the entire page that shows how the shading pattern, by default, starts from the bottom-left corner of the page.
            Dim mediaBox = page.MediaBox
            Dim backgroundRect = page.Content.Elements.AddPath()
            backgroundRect.AddRectangle(mediaBox.Left, mediaBox.Bottom, mediaBox.Width, mediaBox.Height)
            Dim format = backgroundRect.Format
            format.Fill.IsApplied = True
            format.Fill.Color = PdfColor.FromPattern(shadingPattern)
            format.Fill.Opacity = 0.2

            ' Add a square that is filled with the shading pattern.
            Dim square = page.Content.Elements.AddPath()
            square.AddRectangle(25, 25, 200, 200)
            format = square.Format
            format.Fill.IsApplied = True
            format.Fill.Color = PdfColor.FromPattern(shadingPattern)
            format.Stroke.IsApplied = True

            ' Add a text group inside another group because it is recommended to change the Transform only for a single element in a group.
            Dim textGroup = page.Content.Elements.AddGroup().Elements.AddGroup()
            textGroup.Transform = PdfMatrix.CreateTranslation(25, 550)
            Using formattedText = New PdfFormattedText()

                formattedText.Font = New PdfFont("Helvetica", 96)
                formattedText.AppendLine("Hello ").Append("world!")

                ' Draw the formatted text in the bottom-left corner of the group.
                textGroup.DrawText(formattedText, New PdfPoint(0, 0))
            End Using
            format = textGroup.Format
            ' Don't fill the text, but make it a clipping region for next content - shading.
            format.Fill.IsApplied = False
            format.Clip.IsApplied = True
            ' Add a bounding rectangle before (because it would not be visible otherwise because all following content is clipped by the text) text elements.
            Dim path = textGroup.Elements.AddPath(textGroup.Elements.First)
            path.AddRectangle(0, 0, 250, 250)
            path.Format.Stroke.IsApplied = True
            ' Add shading content that is clipped by the text content.
            ' In this case shading doesn't start from the bottom-left corner of the page, but from the bottom-left corner of the group.
            textGroup.Elements.AddShading(shading)

            ' Add a path group inside another group because it is recommended to change the Transform only for a single element in a group.
            Dim pathGroup = page.Content.Elements.AddGroup().Elements.AddGroup()
            pathGroup.Transform = PdfMatrix.CreateTranslation(325, 550)
            path = pathGroup.Elements.AddPath()
            path.AddRectangle(0, 0, 250, 250)
            ' Make path a clipping region for next content - shading.
            path.Format.Clip.IsApplied = True
            ' Add shading content that is clipped by the path content.
            ' In this case shading doesn't start from the bottom-left corner of the page, but from the bottom-left corner of the group.
            Dim shadingContent = pathGroup.Elements.AddShading(shading)
            ' Make the shading 50% opaque.
            shadingContent.Format.Fill.Opacity = 0.5

            document.Save("Shadings.pdf")
        End Using
    End Sub
End Module