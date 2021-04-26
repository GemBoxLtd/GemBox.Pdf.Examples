Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Using formattedText = New PdfFormattedText()

                ' Set font to TrueType font that will be subset and embedded in the document.
                formattedText.Font = New PdfFont("Calibri", 96)

                ' Draw a single letter on each page.
                For i As Integer = 0 To 1

                    formattedText.Append(ChrW(AscW("A"c) + i).ToString())

                    Dim page = document.Pages.Add()

                    ' Begin editing the page content, but don't end it until all pages are edited.
                    page.Content.BeginEdit()

                    page.Content.DrawText(formattedText, New PdfPoint(100, 500))

                    formattedText.Clear()
                Next
            End Using

            ' End editing of all pages.
            ' This will convert back the content of each page to the underlying content stream and the accompanying resource dictionary.
            ' Subset of the 'Calibri' font, that contains only glyphs for characters 'A' to 'B' will be calculated just once before being
            ' embedded in the document.
            For Each page In document.Pages
                page.Content.EndEdit()
            Next

            document.Save("Content Streams And Resources.pdf")
        End Using
    End Sub
End Module