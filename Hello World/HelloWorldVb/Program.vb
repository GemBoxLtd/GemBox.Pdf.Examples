Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document As New PdfDocument()

            ' Add a first empty page.
            document.Pages.Add()

            ' Add a second empty page.
            document.Pages.Add()

            document.Save("Hello World.pdf")
        End Using
    End Sub
End Module