Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document As PdfDocument = PdfDocument.Load("Print.pdf")
            ' Print PDF document to default printer (e.g. 'Microsoft Print to Pdf').
            Dim printerName As String = Nothing
            document.Print(printerName)
        End Using

    End Sub

End Module
