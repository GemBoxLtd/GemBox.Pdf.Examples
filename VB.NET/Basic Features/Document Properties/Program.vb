Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("LoremIpsum.pdf")

            ' Get document properties.
            Dim info = document.Info

            ' Modify document properties.
            info.Title = "Document Properties Example"
            info.Author = "GemBox.Pdf"
            info.Subject = "Introduction to GemBox.Pdf"
            info.Keywords = "GemBox, Pdf, Examples"

            document.Save("Document Properties.pdf")
        End Using
    End Sub
End Module