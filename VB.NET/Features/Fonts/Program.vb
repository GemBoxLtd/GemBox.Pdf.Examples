Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            Using formattedText = New PdfFormattedText()

                formattedText.FontSize = 48
                formattedText.LineHeight = 72

                ' Use the font family 'Almonte Snow' whose font file is located in the 'MyFonts' directory.
                formattedText.FontFamily = New PdfFontFamily("MyFonts", "Almonte Snow")
                formattedText.AppendLine("Hello World!")

                page.Content.DrawText(formattedText, New PdfPoint(100, 500))
            End Using

            document.Save("Private Fonts.pdf")
        End Using
    End Sub
End Module
