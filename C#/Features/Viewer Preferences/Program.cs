using GemBox.Pdf;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Get viewer preferences specifying the way the document should be displayed on the screen.
            var viewerPreferences = document.ViewerPreferences;

            // Modify viewer preferences.
            viewerPreferences.CenterWindow = false;
            viewerPreferences.FitWindow = true;
            viewerPreferences.HideMenubar = true;
            viewerPreferences.HideToolbar = false;
            viewerPreferences.NonFullScreenPageMode = PdfPageMode.FullScreen;
            viewerPreferences.ViewArea = PdfPageBoundaryType.MediaBox;

            document.Save("Viewer Preferences.pdf");
        }
    }
}
