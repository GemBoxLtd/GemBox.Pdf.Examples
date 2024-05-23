Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Create new document.
        Using document = New PdfDocument()

            ' Add a page.
            Dim page = document.Pages.Add()

            ' Write a text.
            Using formattedText = New PdfFormattedText()
                formattedText.Append("Hello World!")
                page.Content.DrawText(formattedText, New PdfPoint(100, 700))
            End Using

            ' Save as PDF file.
            document.Save("output.pdf")
        End Using

    End Sub
End Module
