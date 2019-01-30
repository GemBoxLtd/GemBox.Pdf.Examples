Imports GemBox.Pdf

Module Program

    Sub Main(args As String())

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load a PDF document from a file.
        Using document = PdfDocument.Load("Hello World.pdf")

            ' Add a new empty page.
            document.Pages.Add()

            ' Save all the changes made to the current PDF document using an incremental update.
            document.Save()
        End Using
    End Sub
End Module
