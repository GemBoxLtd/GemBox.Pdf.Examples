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
            var page = document.Pages.Add();

            // Add a rectangle with a green fill whose bottom-left corner is at location (100, 450) from the bottom-left corner of the page.
            var path = page.Content.Elements.AddPath();
            path.AddRectangle(100, 450, 200, 100);
            var format = path.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColors.Green;

            // Add an outer group whose bottom-left corner is at the same location as the page's bottom-left corner.
            var outerGroup = page.Content.Elements.AddGroup();
            // Add an inner group whose bottom-left corner is at location (100, 250) from the bottom-left corner of the outer group/page.
            var innerGroup = outerGroup.Elements.AddGroup();
            innerGroup.Transform = PdfMatrix.CreateTranslation(100, 250);

            // Add a rectangle that clips its content and the content of all elements that come after it in the same group.
            // The bottom-left corner of the clipping rectangle is at location (50, -150) from the bottom-left corner of the inner group.
            // The clipping rectangle is also stroked to show that it goes over the first and the last rectangle from the main content group, but it doesn't clip them because they are in the main content group.
            var clippingPath = innerGroup.Elements.AddPath();
            clippingPath.AddRectangle(50, -150, 100, 400);
            format = clippingPath.Format;
            format.Clip.IsApplied = true;
            format.Stroke.IsApplied = true;
            format.Stroke.Width = 2;
            format.Stroke.DashPattern = PdfLineDashPatterns.Dash;

            // Add a rectangle with a red fill that gets clipped by the previous rectangle, thus making it a square.
            // The bottom-left corner is at the same location as the inner group's bottom-left corner.
            var clippedPath = innerGroup.Elements.AddPath();
            clippedPath.AddRectangle(0, 0, 200, 100);
            format = clippedPath.Format;
            format.Fill.IsApplied = true;
            format.Fill.Color = PdfColors.Red;

            // Add the same rectangle as the first one and move it down by 400 points.
            path = page.Content.Elements.AddClone(path);
            path.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -400));

            document.Save("ContentGroups.pdf");
        }
    }
}