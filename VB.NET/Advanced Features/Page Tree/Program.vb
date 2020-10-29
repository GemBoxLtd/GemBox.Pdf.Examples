Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")
        AddHandler ComponentInfo.FreeLimitReached, Sub(sender, e) e.FreeLimitReachedAction = FreeLimitReachedAction.Stop

        Using document = New PdfDocument()

            Using formattedText = New PdfFormattedText()

                ' Get a page tree root node.
                Dim rootNode = document.Pages
                ' Set page rotation for a whole set of pages.
                rootNode.Rotate = 90

                ' Create a left page tree node.
                Dim childNode = rootNode.Kids.AddPages()
                ' Overwrite a parent tree node rotation value.
                childNode.Rotate = 0

                ' Create a first page.
                Dim page = childNode.Kids.AddPage()
                formattedText.Append("FIRST PAGE")
                page.Content.DrawText(formattedText, New PdfPoint(0, 0))

                ' Create a second page and set a page media box value.
                page = childNode.Kids.AddPage()
                page.SetMediaBox(0, 0, 200, 400)
                formattedText.Clear()
                formattedText.Append("SECOND PAGE")
                page.Content.DrawText(formattedText, New PdfPoint(0, 0))

                ' Create a right page tree node.
                childNode = rootNode.Kids.AddPages()
                ' Set a media box value.
                childNode.SetMediaBox(0, 0, 100, 200)

                ' Create a third page.
                page = childNode.Kids.AddPage()
                formattedText.Clear()
                formattedText.Append("THIRD PAGE")
                page.Content.DrawText(formattedText, New PdfPoint(0, 0))

                ' Create a fourth page and overwrite a rotation value.
                page = childNode.Kids.AddPage()
                page.Rotate = 0
                formattedText.Clear()
                formattedText.Append("FOURTH PAGE")
                page.Content.DrawText(formattedText, New PdfPoint(0, 0))
            End Using

            document.Save("Page Tree.pdf")
        End Using
    End Sub
End Module
