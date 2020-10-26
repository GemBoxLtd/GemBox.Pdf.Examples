using System;
using GemBox.Pdf;
using GemBox.Pdf.Content;

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
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Iterate through PDF pages and extract each page's Unicode text content.
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            foreach (var page in document.Pages)
            {
                Console.WriteLine(page.Content.ToString());
            }
        }
    }

    static void Example2()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Iterate through all PDF pages and through each page's content elements,
        // and retrieve only the text content elements.
        using (var document = PdfDocument.Load("TextContent.pdf"))
        {
            foreach (var page in document.Pages)
            {
                var contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator();
                while (contentEnumerator.MoveNext())
                {
                    if (contentEnumerator.Current.ElementType == PdfContentElementType.Text)
                    {
                        var textElement = (PdfTextContent)contentEnumerator.Current;

                        var text = textElement.ToString();
                        var font = textElement.Format.Text.Font;
                        var color = textElement.Format.Fill.Color;
                        var bounds = textElement.Bounds;

                        contentEnumerator.Transform.Transform(ref bounds);

                        // Read the text content element's additional information.
                        Console.WriteLine($"Unicode text: {text}");
                        Console.WriteLine($"Font name: {font.Face.Family.Name}");
                        Console.WriteLine($"Font size: {font.Size}");
                        Console.WriteLine($"Font style: {font.Face.Style}");
                        Console.WriteLine($"Font weight: {font.Face.Weight}");

                        if (color.TryGetRgb(out double red, out double green, out double blue))
                            Console.WriteLine($"Color: Red={red}, Green={green}, Blue={blue}");

                        Console.WriteLine($"Bounds: Left={bounds.Left:0.00}, Bottom={bounds.Bottom:0.00}, Right={bounds.Right:0.00}, Top={bounds.Top:0.00}");
                        Console.WriteLine();
                    }
                }
            }
        }
    }

    static void Example3()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        var pageIndex = 0;
        double areaLeft = 400, areaRight = 550, areaBottom = 680, areaTop = 720;

        using (var document = PdfDocument.Load("TextContent.pdf"))
        {
            // Retrieve first page object.
            var page = document.Pages[pageIndex];

            // Retrieve text content elements that are inside specified area on the first page.
            var contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator();
            while (contentEnumerator.MoveNext())
            {
                if (contentEnumerator.Current.ElementType == PdfContentElementType.Text)
                {
                    var textElement = (PdfTextContent)contentEnumerator.Current;

                    var bounds = textElement.Bounds;

                    contentEnumerator.Transform.Transform(ref bounds);

                    if (bounds.Left > areaLeft && bounds.Right < areaRight &&
                        bounds.Bottom > areaBottom && bounds.Top < areaTop)
                    {
                        Console.Write(textElement.ToString());
                    }
                }
            }
        }
    }
}
