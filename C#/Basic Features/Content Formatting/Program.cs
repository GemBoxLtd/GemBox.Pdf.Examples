using System;
using GemBox.Pdf;
using GemBox.Pdf.Content;
using GemBox.Pdf.Content.Colors;
using GemBox.Pdf.Content.Patterns;
using GemBox.Pdf.Objects;
using GemBox.Pdf.Text;

class Program
{
    static void Main()
    {
        Example1();

        Example2();

        Example3();

        Example4();

        Example5();

        Example6();
    }

    static void Example1()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // PdfFormattedText currently supports just Device color spaces (DeviceGray, DeviceRGB, and DeviceCMYK).
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 100);

                // Make the text fill black (in DeviceGray color space) and 50% opaque.
                formattedText.Color = PdfColor.FromGray(0);
                // In PDF, opacity is defined separately from the color.
                formattedText.Opacity = 0.5;
                formattedText.Append("Hello world!");

                page.Content.DrawText(formattedText, new PdfPoint(50, 700));
            }

            // Path filled with non-zero winding number rule.
            var path = page.Content.Elements.AddPath();
            var center = new PdfPoint(300, 500);
            double radius = 150, cos1 = Math.Cos(Math.PI / 10), sin1 = Math.Sin(Math.PI / 10), cos2 = Math.Cos(Math.PI / 5), sin2 = Math.Sin(Math.PI / 5);
            // Create a five-point star.
            path.
                BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius). // Start from the point in the bottom-left corner.
                LineTo(center.X + cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-right corner.
                LineTo(center.X - cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-left corner.
                LineTo(center.X + sin2 * radius, center.Y - cos2 * radius). // Continue to the point in the bottom-right corner.
                LineTo(center.X, center.Y + radius). // Continue to the point in the upper-center.
                CloseSubpath(); // End with the starting point.
            var format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Rule = PdfFillRule.NonzeroWindingNumber;
            // Make the path fill red (in DeviceRGB color space) and 40% opaque.
            format.Fill.Color = PdfColor.FromRgb(1, 0, 0);
            format.Fill.Opacity = 0.4;

            // Path filled with even-odd rule.
            path = page.Content.Elements.AddClone(path);
            path.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -300));
            format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Rule = PdfFillRule.EvenOdd;
            // Make the path fill yellow (in DeviceCMYK color space) and 60% opaque.
            format.Fill.Color = PdfColor.FromCmyk(0, 0, 1, 0);
            format.Fill.Opacity = 0.6;

            document.Save("Filling.pdf");
        }
    }

    static void Example2()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // PdfFormattedText currently doesn't support stroking, so we will stroke its drawn output.
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 200);

                formattedText.Append("Hello!");

                page.Content.DrawText(formattedText, new PdfPoint(50, 600));
            }

            // Draw lines with different line joins.
            var path = page.Content.Elements.AddPath();
            path.BeginSubpath(50, 350).LineTo(100, 550).LineTo(150, 350);
            path.Format.Stroke.LineJoin = PdfLineJoin.Miter;

            path = page.Content.Elements.AddPath();
            path.BeginSubpath(200, 350).LineTo(250, 550).LineTo(300, 350);
            path.Format.Stroke.LineJoin = PdfLineJoin.Round;

            path = page.Content.Elements.AddPath();
            path.BeginSubpath(350, 350).LineTo(400, 550).LineTo(450, 350);
            path.Format.Stroke.LineJoin = PdfLineJoin.Bevel;

            // Create a dash pattern with 20 point dashes and 10 point gaps.
            var dashPattern = new PdfLineDashPattern(0, 20, 10);

            // Draw lines with different line caps.
            // Notice how the line cap is applied to each dash.
            path = page.Content.Elements.AddPath();
            path.BeginSubpath(50, 100).LineTo(100, 300).LineTo(150, 100);
            var format = path.Format;
            format.Stroke.LineCap = PdfLineCap.Butt;
            format.Stroke.DashPattern = dashPattern;

            path = page.Content.Elements.AddPath();
            path.BeginSubpath(200, 100).LineTo(250, 300).LineTo(300, 100);
            format = path.Format;
            format.Stroke.LineCap = PdfLineCap.Round;
            format.Stroke.DashPattern = dashPattern;

            path = page.Content.Elements.AddPath();
            path.BeginSubpath(350, 100).LineTo(400, 300).LineTo(450, 100);
            format = path.Format;
            format.Stroke.LineCap = PdfLineCap.Square;
            format.Stroke.DashPattern = dashPattern;

            // Do not fill any content and stroke all content with a 10 point width red outline that is 50% opaque.
            format = page.Content.Format;
            format.Fill.IsApplied = false;
            format.Stroke.IsApplied = true;
            format.Stroke.Width = 10;
            format.Stroke.Color = PdfColor.FromRgb(1, 0, 0);
            format.Stroke.Opacity = 0.5;

            // Add a line to visualize the differences between line joins.
            var line = page.Content.Elements.AddPath().BeginSubpath(25, 550).LineTo(475, 550);
            format = line.Format;
            format.Stroke.IsApplied = true;
            // A line width of 0 denotes the thinnest line that can be rendered at device resolution: 1 device pixel wide.
            format.Stroke.Width = 0;

            // Add a line to visualize the differences between line caps.
            line = page.Content.Elements.AddPath().BeginSubpath(25, 100).LineTo(475, 100);
            format = line.Format;
            format.Stroke.IsApplied = true;
            format.Stroke.Width = 0;

            document.Save("Stroking.pdf");
        }
    }

    static void Example3()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // Add a new content group. Clipping is localized to the content group.
            var textGroup = page.Content.Elements.AddGroup();
            // Draw text in the content group.
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 96);

                formattedText.Append("Hello world!");

                textGroup.DrawText(formattedText, new PdfPoint(50, 700));
            }
            // Stroke all text elements in the group (to visualize their bounds) and set them as a clipping path.
            var format = textGroup.Format;
            format.Fill.IsApplied = false;
            format.Stroke.IsApplied = true;
            format.Clip.IsApplied = true;
            // Draw an image in the same content group as the text.
            // The image will be clipped to the text.
            var image = PdfImage.Load("Acme.png");
            textGroup.DrawImage(image, new PdfPoint(50, 700), new PdfSize(500, 100));

            // Add a new content group. Clipping is localized to the content group.
            var pathGroup = page.Content.Elements.AddGroup();
            // Add a diamond-like path to the content group.
            pathGroup.Elements.AddPath().BeginSubpath(50, 550).LineTo(300, 500).LineTo(550, 550).LineTo(300, 600).CloseSubpath();
            // Stroke all path elements in the group (to visualize their bounds) and set them as a clipping path.
            format = pathGroup.Format;
            format.Fill.IsApplied = false;
            format.Stroke.IsApplied = true;
            format.Clip.IsApplied = true;
            // Draw an image in the same content group as the diamond-like path.
            // The image will be clipped to the diamond-like path.
            pathGroup.DrawImage(image, new PdfPoint(50, 500), new PdfSize(500, 100));

            // Add a new content group. Clipping is localized to the content group.
            pathGroup = page.Content.Elements.AddGroup();
            // Add a star-like path to the content group.
            var path = pathGroup.Elements.AddPath();
            var center = new PdfPoint(150, 300);
            double radius = 100, cos1 = Math.Cos(Math.PI / 10), sin1 = Math.Sin(Math.PI / 10), cos2 = Math.Cos(Math.PI / 5), sin2 = Math.Sin(Math.PI / 5);
            // Create a five-point star.
            path.
                BeginSubpath(center.X - sin2 * radius, center.Y - cos2 * radius). // Start from the point in the bottom-left corner.
                LineTo(center.X + cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-right corner.
                LineTo(center.X - cos1 * radius, center.Y + sin1 * radius). // Continue to the point in the upper-left corner.
                LineTo(center.X + sin2 * radius, center.Y - cos2 * radius). // Continue to the point in the bottom-right corner.
                LineTo(center.X, center.Y + radius). // Continue to the point in the upper-center.
                CloseSubpath(); // End with the starting point.
            // Stroke a path (to visualize its bounds) and set it as a clipping path using non-zero winding number rule.
            format = path.Format;
            format.Fill.IsApplied = false;
            format.Stroke.IsApplied = true;
            format.Clip.IsApplied = true;
            format.Clip.Rule = PdfFillRule.NonzeroWindingNumber;
            // Draw an image in the same content group as the star-like path.
            // The image will be clipped to the star-like path using non-zero winding number rule.
            pathGroup.DrawImage(image, new PdfPoint(50, 200), new PdfSize(200, 200));

            // Add a new content group. Clipping is localized to the content group.
            pathGroup = page.Content.Elements.AddGroup();
            // Clone a star-like path to the content group and move it down.
            path = pathGroup.Elements.AddClone(path);
            path.Subpaths.Transform(PdfMatrix.CreateTranslation(250, 0));
            // Set the clipping rule to even-odd.
            path.Format.Clip.Rule = PdfFillRule.EvenOdd;
            // Draw an image in the same content group as the star-like path.
            // The image will be clipped to the star-like path using the even-odd rule.
            pathGroup.DrawImage(image, new PdfPoint(300, 200), new PdfSize(200, 200));

            document.Save("Clipping.pdf");
        }
    }

    static void Example4()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // PdfFormattedText currently supports just Device color spaces (DeviceGray, DeviceRGB, and DeviceCMYK).
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 24);

                // Three different ways to specify gray color in the DeviceGray color space:
                formattedText.Color = PdfColors.Gray;
                formattedText.Append("Hello world! ");
                formattedText.Color = PdfColor.FromGray(0.5);
                formattedText.Append("Hello world! ");
                formattedText.Color = new PdfColor(PdfColorSpace.DeviceGray, 0.5);
                formattedText.AppendLine("Hello world!");

                // Three different ways to specify red color in the DeviceRGB color space:
                formattedText.Color = PdfColors.Red;
                formattedText.Append("Hello world! ");
                formattedText.Color = PdfColor.FromRgb(1, 0, 0);
                formattedText.Append("Hello world! ");
                formattedText.Color = new PdfColor(PdfColorSpace.DeviceRGB, 1, 0, 0);
                formattedText.AppendLine("Hello world!");

                // Three different ways to specify yellow color in the DeviceCMYK color space:
                formattedText.Color = PdfColors.Yellow;
                formattedText.Append("Hello world! ");
                formattedText.Color = PdfColor.FromCmyk(0, 0, 1, 0);
                formattedText.Append("Hello world! ");
                formattedText.Color = new PdfColor(PdfColorSpace.DeviceCMYK, 0, 0, 1, 0);
                formattedText.Append("Hello world!");

                page.Content.DrawText(formattedText, new PdfPoint(100, 500));
            }

            // Create an Indexed color space (which is currently not supported by GemBox.Pdf)
            // as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=164
            // Base color space is DeviceRGB and the created Indexed color space consists of two colors:
            // at index 0: green color (0x00FF00)
            // at index 1: blue color (0x0000FF)
            var indexedColorSpaceArray = PdfArray.Create(4);
            indexedColorSpaceArray.Add(PdfName.Create("Indexed"));
            indexedColorSpaceArray.Add(PdfName.Create("DeviceRGB"));
            indexedColorSpaceArray.Add(PdfInteger.Create(1));
            indexedColorSpaceArray.Add(PdfString.Create("\x00\xFF\x00\x00\x00\xFF", PdfEncoding.Byte, PdfStringForm.Hexadecimal));
            var indexedColorSpace = PdfColorSpace.FromArray(indexedColorSpaceArray);

            // Add a rectangle.
            // Fill it with color at index 0 (green) of the Indexed color space.
            // Stroke it with color at index 1 (blue) of the Indexed color space.
            var path = page.Content.Elements.AddPath();
            path.AddRectangle(100, 300, 200, 100);
            var format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = new PdfColor(indexedColorSpace, 0);
            format.Stroke.IsApplied = true;
            format.Stroke.Color = new PdfColor(indexedColorSpace, 1);
            format.Stroke.Width = 5;

            document.Save("Colors.pdf");
        }
    }

    static void Example5()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // The uncolored tiling pattern should not specify the color of its content, instead the outer element that uses the uncolored tiling pattern will specify the color of the tiling pattern content.
            var uncoloredTilingPattern = new PdfTilingPattern(document, new PdfSize(100, 100)) { IsColored = false };
            // Begin editing the pattern cell.
            uncoloredTilingPattern.Content.BeginEdit();
            // The tiling pattern cell contains two triangles that are filled with color specified by the outer element that uses the uncolored tiling pattern.
            var path = uncoloredTilingPattern.Content.Elements.AddPath();
            path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath();
            path.Format.Fill.IsApplied = true;
            path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath();
            path.Format.Fill.IsApplied = true;
            // End editing the pattern cell.
            uncoloredTilingPattern.Content.EndEdit();

            // Create an uncolored tiling Pattern color space (which is currently not supported by GemBox.Pdf)
            // as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=186.
            // The underlying color space is DeviceRGB and colorants will be specified in DeviceRGB.
            var uncoloredTilingPatternColorSpaceArray = PdfArray.Create(2);
            uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("Pattern"));
            uncoloredTilingPatternColorSpaceArray.Add(PdfName.Create("DeviceRGB"));
            var uncoloredTilingPatternColorSpace = PdfColorSpace.FromArray(uncoloredTilingPatternColorSpaceArray);

            var page = document.Pages.Add();

            // Add a background rectangle over the entire page that shows how the tiling pattern, by default, starts from the bottom-left corner of the page.
            var mediaBox = page.MediaBox;
            var backgroundRect = page.Content.Elements.AddPath();
            backgroundRect.AddRectangle(mediaBox.Left, mediaBox.Bottom, mediaBox.Width, mediaBox.Height);
            var format = backgroundRect.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 0);
            format.Fill.Opacity = 0.2;

            // Add a rectangle that is filled with the red (red = 1, green = 0, blue = 0) pattern.
            var redRect = page.Content.Elements.AddPath();
            redRect.AddRectangle(75, 575, 200, 100);
            format = redRect.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 1, 0, 0);
            format.Stroke.IsApplied = true;

            // Add a rectangle that is filled with the same pattern, but this time the pattern's color is green (red = 0, green = 1, blue = 0).
            var greenRect = page.Content.Elements.AddClone(redRect);
            greenRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150));
            greenRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 1, 0);

            // Add a rectangle that is filled with the same pattern, but this time the pattern's color is blue (red = 0, green = 0, blue = 1).
            var blueRect = page.Content.Elements.AddClone(greenRect);
            blueRect.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -150));
            blueRect.Format.Fill.Color = PdfColor.FromPattern(uncoloredTilingPatternColorSpace, uncoloredTilingPattern, 0, 0, 1);

            // The colored tiling pattern specifies the color of its content.
            var coloredTilingPattern = new PdfTilingPattern(document, new PdfSize(100, 100));
            // Begin editing the pattern cell.
            coloredTilingPattern.Content.BeginEdit();
            // The tiling pattern cell contains two triangles. The first one is filled with the red color and the second one is filled with the green color.
            path = coloredTilingPattern.Content.Elements.AddPath();
            path.BeginSubpath(0, 0).LineTo(50, 0).LineTo(50, 100).CloseSubpath();
            format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColors.Red;
            path = coloredTilingPattern.Content.Elements.AddPath();
            path.BeginSubpath(50, 0).LineTo(100, 0).LineTo(100, 100).CloseSubpath();
            format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColors.Green;
            // End editing the pattern cell.
            coloredTilingPattern.Content.EndEdit();

            // Add a rectangle that is filled with the colored (red-green) tiling pattern.
            var redGreenRect = page.Content.Elements.AddPath();
            redGreenRect.AddRectangle(325, 275, 200, 400);
            format = redGreenRect.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColor.FromPattern(coloredTilingPattern);
            format.Stroke.IsApplied = true;

            document.Save("Patterns.pdf");
        }
    }

    static void Example6()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // Create axial shading (which is currently not supported by GemBox.Pdf)
            // as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=193
            var shadingDictionary = PdfDictionary.Create();
            PdfIndirectObject.Create(shadingDictionary);
            shadingDictionary[PdfName.Create("ShadingType")] = PdfInteger.Create(2);
            // Color values of the shading will be expressed in DeviceRGB color space.
            shadingDictionary[PdfName.Create("ColorSpace")] = PdfName.Create("DeviceRGB");
            // Shading transitions the colors in the axis from location (0, 0) to location (250, 250).
            shadingDictionary[PdfName.Create("Coords")] = PdfArray.Create(PdfNumber.Create(0), PdfNumber.Create(0), PdfNumber.Create(250), PdfNumber.Create(250));
            var functionDictionary = PdfDictionary.Create();
            functionDictionary[PdfName.Create("FunctionType")] = PdfInteger.Create(2);
            functionDictionary[PdfName.Create("Domain")] = PdfArray.Create(PdfNumber.Create(0), PdfNumber.Create(1));
            functionDictionary[PdfName.Create("N")] = PdfNumber.Create(1);
            // Red color transitions from 1 (C0[0]) to 0 (C1[0]).
            // Green color transitions from 0 (C0[1]) to 1 (C1[1]).
            // Blue color is always 0 (C0[2] and C1[2] are 0).
            functionDictionary[PdfName.Create("C0")] = PdfArray.Create(PdfNumber.Create(1), PdfNumber.Create(0), PdfNumber.Create(0));
            functionDictionary[PdfName.Create("C1")] = PdfArray.Create(PdfNumber.Create(0), PdfNumber.Create(1), PdfNumber.Create(0));
            shadingDictionary[PdfName.Create("Function")] = functionDictionary;
            var shading = PdfShading.FromDictionary(shadingDictionary);

            var shadingPattern = new PdfShadingPattern(document, shading);

            var page = document.Pages.Add();

            // Add a background rectangle over the entire page that shows how the shading pattern, by default, starts from the bottom-left corner of the page.
            var mediaBox = page.MediaBox;
            var backgroundRect = page.Content.Elements.AddPath();
            backgroundRect.AddRectangle(mediaBox.Left, mediaBox.Bottom, mediaBox.Width, mediaBox.Height);
            var format = backgroundRect.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColor.FromPattern(shadingPattern);
            format.Fill.Opacity = 0.2;

            // Add a square that is filled with the shading pattern.
            var square = page.Content.Elements.AddPath();
            square.AddRectangle(25, 25, 200, 200);
            format = square.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColor.FromPattern(shadingPattern);
            format.Stroke.IsApplied = true;

            // Add a text group inside another group because it is recommended to change the Transform only for a single element in a group.
            var textGroup = page.Content.Elements.AddGroup().Elements.AddGroup();
            textGroup.Transform = PdfMatrix.CreateTranslation(25, 550);
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 96);
                formattedText.AppendLine("Hello ").Append("world!");

                // Draw the formatted text in the bottom-left corner of the group.
                textGroup.DrawText(formattedText, new PdfPoint(0, 0));
            }
            format = textGroup.Format;
            // Don't fill the text, but make it a clipping region for next content - shading.
            format.Fill.IsApplied = false;
            format.Clip.IsApplied = true;
            // Add a bounding rectangle before (because it would not be visible otherwise because all following content is clipped by the text) text elements.
            var path = textGroup.Elements.AddPath(textGroup.Elements.First);
            path.AddRectangle(0, 0, 250, 250);
            path.Format.Stroke.IsApplied = true;
            // Add shading content that is clipped by the text content.
            // In this case shading doesn't start from the bottom-left corner of the page, but from the bottom-left corner of the group.
            textGroup.Elements.AddShading(shading);

            // Add a path group inside another group because it is recommended to change the Transform only for a single element in a group.
            var pathGroup = page.Content.Elements.AddGroup().Elements.AddGroup();
            pathGroup.Transform = PdfMatrix.CreateTranslation(325, 550);
            path = pathGroup.Elements.AddPath();
            path.AddRectangle(0, 0, 250, 250);
            // Make path a clipping region for next content - shading.
            path.Format.Clip.IsApplied = true;
            // Add shading content that is clipped by the path content.
            // In this case shading doesn't start from the bottom-left corner of the page, but from the bottom-left corner of the group.
            var shadingContent = pathGroup.Elements.AddShading(shading);
            // Make the shading 50% opaque.
            shadingContent.Format.Fill.Opacity = 0.5;

            document.Save("Shadings.pdf");
        }
    }
}
