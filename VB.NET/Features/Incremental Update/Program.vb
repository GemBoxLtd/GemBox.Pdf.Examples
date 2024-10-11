Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load a PDF document from a file.
        Using document = PdfDocument.Load("Hello World.pdf")

            ' Add a page.
            Dim page = document.Pages.Add()

            ' Write a text.
            Using formattedText = New PdfFormattedText()

                formattedText.Append("Hello World again!")

                page.Content.DrawText(formattedText, New PdfPoint(100, 700))
            End Using

            ' Save all the changes made to the current PDF document using an incremental update.
            document.Save()
        End Using
    End Sub
End Module
