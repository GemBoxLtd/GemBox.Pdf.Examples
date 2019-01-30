Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("LoremIpsum.pdf")

            ' Get viewer preferences specifying the way the document should be displayed on the screen.
            Dim ViewerPreferences = document.ViewerPreferences

            ' Modify viewer preferences.
            ViewerPreferences.CenterWindow = False
            ViewerPreferences.FitWindow = True
            ViewerPreferences.HideMenubar = True
            ViewerPreferences.HideToolbar = False
            ViewerPreferences.NonFullScreenPageMode = PdfPageMode.FullScreen
            ViewerPreferences.ViewArea = PdfPageBoundaryType.MediaBox

            document.Save("Viewer Preferences.pdf")
        End Using
    End Sub
End Module