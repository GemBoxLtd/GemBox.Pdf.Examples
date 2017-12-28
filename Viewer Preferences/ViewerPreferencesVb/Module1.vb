Imports System
Imports GemBox.Pdf

Module Module1

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim document As PdfDocument = PdfDocument.Load("Reading.pdf")

        ' Get viewer preferences specifying the way the document should be displayed on the screen.
        Dim ViewerPreferences As PdfViewerPreferences = document.ViewerPreferences

        ' Modify viewer preferences.
        ViewerPreferences.CenterWindow = False
        ViewerPreferences.FitWindow = True
        ViewerPreferences.HideMenubar = True
        ViewerPreferences.HideToolbar = False
        ViewerPreferences.NonFullScreenPageMode = PdfPageMode.FullScreen
        ViewerPreferences.ViewArea = PdfPageBoundaryType.MediaBox
        document.SaveOptions.CloseOutput = True
        document.Save("Viewer Preferences.pdf")

    End Sub

End Module