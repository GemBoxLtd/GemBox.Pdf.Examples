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
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Create new document.
        using (var document = new PdfDocument())
        {
            // Add new page.
            var page = document.Pages.Add();

            // Add image from PNG file.
            var image = PdfImage.Load("parrot.png");
            page.Content.DrawImage(image, new PdfPoint(0, 0));

            // Set page size.
            page.SetMediaBox(image.Width, image.Height);

            // Save as PDF file.
            document.Save("converted-png-image.pdf");
        }
    }

    static void Example2()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        string[] jpgs = { "penguin.jpg", "jellyfish.jpg", "dolphin.jpg", "lion.jpg", "deer.jpg" };

        // Create new document.
        using (var document = new PdfDocument())
        {
            // For each image add new page with margins.
            foreach (var jpg in jpgs)
            {
                var page = document.Pages.Add();
                double margins = 20;

                // Load image from JPG file.
                var image = PdfImage.Load(jpg);

                // Set page size.
                page.SetMediaBox(image.Width + 2 * margins, image.Height + 2 * margins);

                // Draw backgroud color.
                var backgroud = page.Content.Elements.AddPath();
                backgroud.AddRectangle(new PdfPoint(0, 0), page.Size);
                backgroud.Format.Fill.IsApplied = true;
                backgroud.Format.Fill.Color = PdfColor.FromRgb(1, 0, 1);

                // Draw image.
                page.Content.DrawImage(image, new PdfPoint(margins, margins));
            }

            // Save as PDF file.
            document.Save("converted-jpg-images.pdf");
        }
    }

    static void Example3()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Create new document.
        using (var document = new PdfDocument())
        {
            // Load image from PNG file.
            var image = PdfImage.Load("parrot.png");

            double width = image.Width;
            double height = image.Height;
            double ratio = width / height;

            // Add image four times, each time with 20% smaller size.
            for (int i = 0; i < 4; i++)
            {
                width *= 0.8;
                height = width / ratio;

                var page = document.Pages.Add();
                page.Content.DrawImage(image, new PdfPoint(0, 0), new PdfSize(width, height));
                page.SetMediaBox(width, height);
            }

            document.Save("converted-scaled-png-images.pdf");
        }
    }
}
