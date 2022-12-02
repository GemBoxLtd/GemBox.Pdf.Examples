Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document As PdfDocument = PdfDocument.Load("Print.pdf")
            ' Print Word document to default printer.
            Dim printer As String = Nothing
            document.Print(printer)
        End Using

    End Sub
End Module