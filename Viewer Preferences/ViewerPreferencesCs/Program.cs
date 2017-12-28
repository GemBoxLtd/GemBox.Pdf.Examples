using System;
using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        PdfDocument document = PdfDocument.Load("Reading.pdf");

        // Get viewer preferences specifying the way the document should be displayed on the screen.
        PdfViewerPreferences viewerPreferences = document.ViewerPreferences;

        // Modify viewer preferences.
        viewerPreferences.CenterWindow = false;
        viewerPreferences.FitWindow = true;
        viewerPreferences.HideMenubar = true;
        viewerPreferences.HideToolbar = false;
        viewerPreferences.NonFullScreenPageMode = PdfPageMode.FullScreen;
        viewerPreferences.ViewArea = PdfPageBoundaryType.MediaBox;

        document.SaveOptions.CloseOutput = true;
        document.Save("Viewer Preferences.pdf");
    }
}
