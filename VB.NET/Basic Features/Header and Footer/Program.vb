Imports System
Imports System.Globalization
Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")
        AddHandler ComponentInfo.FreeLimitReached, Sub(sender, e) e.FreeLimitReachedAction = FreeLimitReachedAction.Stop

        Using document = PdfDocument.Load("LoremIpsum.pdf")

            Dim marginLeft As Double = 20, marginTop As Double = 10, marginRight As Double = 20, marginBottom As Double = 10

            Using formattedText = New PdfFormattedText()

                formattedText.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture))

                ' Add a header with the current date and time to all pages.
                For Each page In document.Pages

                    ' Set the location of the bottom-left corner of the text.
                    ' We want the top-left corner of the text to be at location (marginLeft, marginTop)
                    ' from the top-left corner of the page.
                    ' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                    ' and the positive y axis extends vertically upward.
                    Dim x As Double = marginLeft, y As Double = page.CropBox.Top - marginTop - formattedText.Height

                    page.Content.DrawText(formattedText, New PdfPoint(x, y))
                Next

                ' Add a footer with the current page number to all pages.
                Dim pageCount As Integer = document.Pages.Count, pageNumber As Integer = 0
                For Each page In document.Pages

                    pageNumber += 1

                    formattedText.Clear()
                    formattedText.Append(String.Format("Page {0} of {1}", pageNumber, pageCount))

                    ' Set the location of the bottom-left corner of the text.
                    Dim x As Double = page.CropBox.Width - marginRight - formattedText.Width, y As Double = marginBottom

                    page.Content.DrawText(formattedText, New PdfPoint(x, y))
                Next
            End Using

            document.Save("Header and Footer.pdf")
        End Using
    End Sub
End Module