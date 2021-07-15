Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Form.pdf")

            document.Form.Fields("FullName").Value = "Jane Doe"
            document.Form.Fields("ID").Value = "0123456789"
            document.Form.Fields("Gender").Value = "Female"
            document.Form.Fields("Married").Value = "Yes"
            document.Form.Fields("City").Value = "Berlin"
            document.Form.Fields("Language").Value = New String() {"German", "Italian"}
            document.Form.Fields("Notes").Value = "Notes first line" & vbCr & "Notes second line" & vbCr & "Notes third line"

            document.Save("FormFilled.pdf")
        End Using
    End Sub
End Module
