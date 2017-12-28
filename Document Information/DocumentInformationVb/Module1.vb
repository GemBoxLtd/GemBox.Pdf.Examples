Imports System
Imports GemBox.Pdf

Module Module1

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim document As PdfDocument = PdfDocument.Load("Reading.pdf")

        ' Get document information.
        Dim info As PdfDocumentInformation = document.Info

        ' Modify document information.
        info.Title = "Document Information Example"
        info.Author = "GemBox.Pdf"
        info.Subject = "Introduction to GemBox.Pdf"
        info.Keywords = "GemBox, Pdf, Examples"
        document.SaveOptions.CloseOutput = True
        document.Save("Document Information.pdf")

    End Sub

End Module