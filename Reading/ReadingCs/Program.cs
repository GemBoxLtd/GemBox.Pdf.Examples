using System;
using System.Drawing;
using System.Linq;
using GemBox.Pdf;
using GemBox.Pdf.Content;

namespace ReadingCs
{
    class Program
    {
        static void Main(string[] args)
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
                foreach (var page in document.Pages)
                    Console.WriteLine(page.Content.ToString());
        }

        static void Example2()
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            // Iterate through all PDF pages and through each page's content elements,
            // and retrieve only the text content elements.
            using (var document = PdfDocument.Load("TextContent.pdf"))
                foreach (var textElement in document.Pages
                    .SelectMany(page => page.Content.Elements.All())
                    .Where(element => element.ElementType == PdfContentElementType.Text)
                    .Cast<PdfTextContent>())
                {
                    var text = textElement.ToString();
                    var font = textElement.Format.Text.Font;
                    var color = textElement.Format.Fill.Color;
                    var location = textElement.Location;

                    // Read the text content element's additional information.
                    Console.WriteLine($"Unicode text: {text}");
                    Console.WriteLine($"Font name: {font.Face.Family.Name}");
                    Console.WriteLine($"Font size: {font.Size}");
                    Console.WriteLine($"Font style: {font.Face.Style}");
                    Console.WriteLine($"Font weight: {font.Face.Weight}");

                    if (color.TryGetRgb(out double red, out double green, out double blue))
                        Console.WriteLine($"Color: Red={red}, Green={green}, Blue={blue}");

                    Console.WriteLine($"Location: X={location.X:0.00}, Y={location.Y:0.00}");
                    Console.WriteLine();
                }
        }

        static void Example3()
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            var pageIndex = 0;
            var area = new Rectangle(400, 690, 150, 30);

            using (var document = PdfDocument.Load("TextContent.pdf"))
            {
                // Retrieve first page object.
                var page = document.Pages[pageIndex];

                // Retrieve text content elements that are inside specified area on the first page.
                foreach (var textElement in page.Content.Elements.All()
                    .Where(element => element.ElementType == PdfContentElementType.Text)
                    .Cast<PdfTextContent>())
                {
                    var location = textElement.Location;
                    if (location.X > area.X && location.X < area.X + area.Width &&
                        location.Y > area.Y && location.Y < area.Y + area.Height)
                        Console.Write(textElement.ToString());
                }
            }
        }
    }
}
