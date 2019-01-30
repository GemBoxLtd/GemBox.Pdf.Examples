Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("LoremIpsum.pdf")

            ' Get document information.
            Dim info = document.Info

            ' Modify document information.
            info.Title = "Document Information Example"
            info.Author = "GemBox.Pdf"
            info.Subject = "Introduction to GemBox.Pdf"
            info.Keywords = "GemBox, Pdf, Examples"

            document.Save("Document Information.pdf")
        End Using
    End Sub
End Module