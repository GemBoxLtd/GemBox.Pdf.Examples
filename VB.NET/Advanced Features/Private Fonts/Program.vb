Imports System.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            Using formattedText = New PdfFormattedText()

                formattedText.FontSize = 48
                formattedText.LineHeight = 72

                ' Use font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
                formattedText.FontFamily = New PdfFontFamily("Resources", "Almonte Snow")
                formattedText.AppendLine("Hello World 1!")

                ' Use font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
                formattedText.FontFamily = New PdfFontFamily(Nothing, "Resources", "Almonte Woodgrain")
                formattedText.AppendLine("Hello World 2!")

                ' Another way to use font family 'Almonte Snow' whose font file is located in the 'Resources' directory.
                formattedText.FontFamily = PdfFonts.GetFontFamilies("Resources").First(Function(ff) ff.Name = "Almonte Snow")
                formattedText.AppendLine("Hello World 3!")

                ' Another way to use font family 'Almonte Woodgrain' whose font file is located in the 'Resources' location of the current assembly.
                formattedText.FontFamily = PdfFonts.GetFontFamilies(Nothing, "Resources").First(Function(ff) ff.Name = "Almonte Woodgrain")
                formattedText.Append("Hello World 4!")

                page.Content.DrawText(formattedText, New PdfPoint(100, 500))
            End Using

            document.Save("Private Fonts.pdf")
        End Using
    End Sub
End Module
