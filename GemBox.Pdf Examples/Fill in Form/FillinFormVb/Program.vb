Imports System.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Form.pdf")

            ' Set check box field checked state.
            Dim marriedCheckBox = CType(document.Form.Fields("Married"), PdfCheckBoxField)
            marriedCheckBox.Checked = True

            ' Set check box field value.
            Dim drivingLicenseCheckBox = CType(document.Form.Fields("Driving License"), PdfCheckBoxField)
            drivingLicenseCheckBox.Value = "Yes"

            ' Set radio button field checked state.
            ' There are multiple radio button fields with name 'Gender' so
            ' set checked state only to the radio button field whose choice is 'Male'.
            For Each genderRadioButton As PdfRadioButtonField In document.Form.Fields.Where(Function(field) field.Name = "Gender")
                genderRadioButton.Checked = genderRadioButton.Choice = "Male"
            Next

            ' Set radio button field value.
            ' It is enough to set value to only one radio button field 
            ' from a group of radio button fields with name 'Age'.
            Dim ageRadioButton = CType(document.Form.Fields("Age"), PdfRadioButtonField)
            ageRadioButton.Value = "18 - 39"

            document.Save("Fill in Form.pdf")
        End Using
    End Sub
End Module
