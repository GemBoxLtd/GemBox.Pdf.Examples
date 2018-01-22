Imports System
Imports GemBox.Pdf

Module Module1

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim document As PdfDocument = New PdfDocument()

        ' Add a first empty page.
        document.Pages.Add()

        ' Add a second empty page.
        document.Pages.Add()

        document.SaveOptions.CloseOutput = True
        document.Save("Hello World.pdf")

    End Sub

End Module