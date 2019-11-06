Imports GemBox.Pdf
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Form.pdf")

            ' Get a button field.
            Dim submitButtonField = CType(document.Form.Fields("Submit"), PdfButtonField)

            ' Create "submit form" field action.
            Dim submitFormAction = submitButtonField.Actions.AddSubmitForm("https://www.gemboxsoftware.com/")
            ' Set XFDF (XML Forms Data Format) as form data export format.
            submitFormAction.ExportFormat = PdfFormDataFormat.XFDF
            ' Submit all form fields.
            submitFormAction.SelectedFields.All = True

            ' Get a button field.
            Dim resetButtonField = CType(document.Form.Fields("Reset"), PdfButtonField)

            ' Create "reset form" field action.
            Dim resetFormAction = resetButtonField.Actions.AddResetForm()
            ' Reset "Gender" and "Age" fields.
            resetFormAction.SelectedFields.Excluded = False
            resetFormAction.SelectedFields.Add("Gender")
            resetFormAction.SelectedFields.Add("Age")

            document.Save("Form Actions.pdf")
        End Using
    End Sub
End Module
