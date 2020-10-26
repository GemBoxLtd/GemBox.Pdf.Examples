Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            ' Add a page.
            Dim page = document.Pages.Add()

            ' Write a text.
            Using formattedText = New PdfFormattedText()

                formattedText.Append("Hello World!")

                page.Content.DrawText(formattedText, New PdfPoint(100, 700))
            End Using

            document.Save("Hello World.pdf")
        End Using
    End Sub
End Module