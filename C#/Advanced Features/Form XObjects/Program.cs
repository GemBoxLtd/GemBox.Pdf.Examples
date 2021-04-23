using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var form = new PdfForm(document, new PdfSize(200, 200));

            form.Content.BeginEdit();

            var textGroup = form.Content.Elements.AddGroup();

            // Add text with default fill (fill will be inherited from the outer PdfFormContent).
            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Font = new PdfFont("Helvetica", 24);
                formattedText.Append("Hello world!");

                // Draw the formatted text at location (50, 150) from the bottom-left corner of the group/form.
                textGroup.DrawText(formattedText, new PdfPoint(50, 150));
            }

            // Add the same text with black fill 50 points below the first text.
            var blackText = (PdfTextContent)textGroup.Elements.AddClone(textGroup.Elements.First);
            blackText.TextTransform = PdfMatrix.CreateTranslation(0, -50) * blackText.TextTransform;
            blackText.Format.Fill.Color = PdfColors.Black;

            var pathGroup = form.Content.Elements.AddGroup();

            // Add rectangle path with default fill (fill will be inherited from the outer PdfFormContent).
            var path = pathGroup.Elements.AddPath();
            path.AddRectangle(0, 50, 200, 40);
            path.Format.Fill.IsApplied = true;

            // Add the same path with black fill 50 points below the first path.
            var blackPath = pathGroup.Elements.AddClone(path);
            blackPath.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -50));
            blackPath.Format.Fill.Color = PdfColors.Black;

            form.Content.EndEdit();

            var page = document.Pages.Add();

            // Add the outer PdfFormContent with default (black) fill.
            // Elements in the inner PdfForm that do not have fill set, will have default (black) fill.
            var group = page.Content.Elements.AddGroup();
            var formContent1 = group.Elements.AddForm(form);
            formContent1.Transform = PdfMatrix.CreateTranslation(100, 600);

            // Add the outer PdfFormContent with green fill.
            // Elements in the inner PdfForm that do not have fill set, will have green fill.
            group = page.Content.Elements.AddGroup();
            var formContent2 = group.Elements.AddForm(form);
            formContent2.Transform = PdfMatrix.CreateTranslation(100, 350);
            formContent2.Format.Fill.Color = PdfColors.Green;

            // Add the outer PdfFormContent with red fill.
            // Elements in the inner PdfForm that do not have fill set, will have red fill.
            group = page.Content.Elements.AddGroup();
            var formContent3 = group.Elements.AddClone(formContent1);
            formContent3.Transform = PdfMatrix.CreateTranslation(100, 100);
            formContent3.Format.Fill.Color = PdfColors.Red;

            document.Save("FormXObjects.pdf");
        }
    }
}