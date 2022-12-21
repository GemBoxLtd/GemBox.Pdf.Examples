using System;
using System.IO;
using System.Linq;
using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        Example1();
        Example2();
        Example3();
        Example4();
        Example5();
    }

    static void Example1()
    {
        using (var document = PdfDocument.Load("ExportImages.pdf"))
        {
            // Iterate through PDF pages.
            foreach (var page in document.Pages)
            {
                // Get all image content elements on the page.
                var imageElements = page.Content.Elements.All().OfType<PdfImageContent>().ToList();

                // Export the first image element to an image file.
                if (imageElements.Count > 0)
                {
                    imageElements[0].Save("Export Images.jpeg");
                    break;
                }
            }
        }
    }

    static void Example2()
    {
        using (var document = PdfDocument.Load("ExportImages.pdf"))
        {
            // Iterate through all PDF pages and through each page's content elements,
            // and retrieve only the image content elements.
            for (int index = 0; index < document.Pages.Count; index++)
            {
                var page = document.Pages[index];
                var contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator();
                while (contentEnumerator.MoveNext())
                {
                    if (contentEnumerator.Current.ElementType == PdfContentElementType.Image)
                    {
                        var imageElement = (PdfImageContent)contentEnumerator.Current;
                        Console.Write($"Image on page {index + 1} | ");

                        var bounds = imageElement.Bounds;
                        contentEnumerator.Transform.Transform(ref bounds);
                        Console.Write($"from ({bounds.Left:#},{bounds.Bottom:#}) to ({bounds.Right:#},{bounds.Top:#}) | ");

                        var image = imageElement.Image;
                        Console.WriteLine($"size {image.Size.Width}x{image.Size.Height}");
                    }
                }
            }
        }
    }

    static void Example3()
    {
        using (var document = new PdfDocument())
        {
            // Add a page.
            var page = document.Pages.Add();

            // Load the image from a file.
            var image = PdfImage.Load("FragonardReader.jpg");

            // Set the location of the bottom-left corner of the image.
            // We want the top-left corner of the image to be at location (50, 50)
            // from the top-left corner of the page.
            // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
            // and the positive y axis extends vertically upward.
            double x = 50, y = page.CropBox.Top - 50 - image.Size.Height;

            // Draw the image to the page.
            page.Content.DrawImage(image, new PdfPoint(x, y));

            document.Save("Import Images.pdf");
        }
    }

    static void Example4()
    {
        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            // Load the image from a file.
            var image = PdfImage.Load("Corner.png");

            double margin = 50;

            // Set the location of the first image in the top-left corner of the page (with a specified margin).
            double x = margin;
            double y = page.CropBox.Top - margin - image.Size.Height;

            // Draw the first image.
            page.Content.DrawImage(image, new PdfPoint(x, y));

            // Set the location of the second image in the top-right corner of the page (with the same margin).
            x = page.CropBox.Right - margin - image.Size.Width;
            y = page.CropBox.Top - margin - image.Size.Height;

            // Initialize the transformation.
            var transform = PdfMatrix.Identity;
            // Use the translate operation to position the image.
            transform.Translate(x, y);
            // Use the scale operation to resize the image.
            // NOTE: The unit square of user space, bounded by user coordinates (0, 0) and (1, 1), 
            // corresponds to the boundary of the image in the image space.
            transform.Scale(image.Size.Width, image.Size.Height);
            // Use the scale operation to flip the image horizontally.
            transform.Scale(-1, 1, 0.5, 0);

            // Draw the second image.
            page.Content.DrawImage(image, transform);

            // Set the location of the third image in the bottom-left corner of the page (with the same margin).
            x = margin;
            y = margin;

            // Initialize the transformation.
            transform = PdfMatrix.Identity;
            // Use the translate operation to position the image.
            transform.Translate(x, y);
            // Use the scale operation to resize the image.
            transform.Scale(image.Size.Width, image.Size.Height);
            // Use the scale operation to flip the image vertically.
            transform.Scale(1, -1, 0, 0.5);

            // Draw the third image.
            page.Content.DrawImage(image, transform);

            // Set the location of the fourth image in the bottom-right corner of the page (with the same margin).
            x = page.CropBox.Right - margin - image.Size.Width;
            y = margin;

            // Initialize the transformation.
            transform = PdfMatrix.Identity;
            // Use the translate operation to position the image.
            transform.Translate(x, y);
            // Use the scale operation to resize the image.
            transform.Scale(image.Size.Width, image.Size.Height);
            // Use the scale operation to flip the image horizontally and vertically.
            transform.Scale(-1, -1, 0.5, 0.5);

            // Draw the fourth image.
            page.Content.DrawImage(image, transform);

            document.Save("Positioning and Transformations.pdf");
        }
    }

    static void Example5()
    {
        var imageFiles = Directory.EnumerateFiles("Images");

        int imageCounter = 0;
        int chunkSize = 1000;

        using (var document = new PdfDocument())
        {
            // Create output PDF file that will have large number of images imported into it.
            document.Save("Import Many Images.pdf");

            foreach (var imageFile in imageFiles)
            {
                var page = document.Pages.Add();
                var image = PdfImage.Load(imageFile);

                double ratioX = page.Size.Width / image.Width;
                double ratioY = page.Size.Height / image.Height;
                double ratio = Math.Min(ratioX, ratioY);

                var imageSize = ratio < 1 ?
                    new PdfSize(image.Width * ratio, image.Height * ratio) :
                    new PdfSize(image.Width, image.Height);
                var imagePosition = new PdfPoint(0, page.Size.Height - imageSize.Height);
                page.Content.DrawImage(image, imagePosition, imageSize);

                ++imageCounter;
                if (imageCounter % chunkSize == 0)
                {
                    // Save the new images that were added after the document was last saved.
                    document.Save();

                    // Clear previously parsed images and thus free memory necessary for merging additional pages.
                    document.Unload();
                }
            }

            // Save the last chunk of imported images.
            document.Save();
        }
    }
}
