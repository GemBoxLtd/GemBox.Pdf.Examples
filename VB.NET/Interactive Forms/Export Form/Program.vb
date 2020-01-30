Imports GemBox.Pdf
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("FormFilled.pdf")
            document.Form.ExportData("Form Data.fdf")
        End Using
    End Sub
End Module
