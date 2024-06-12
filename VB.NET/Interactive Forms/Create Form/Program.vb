Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            Dim pageSize As PdfSize = page.Size, fieldSize As PdfSize = New PdfSize(150, 20)
            Dim x As Double = pageSize.Width / 2 - 100, xLabel As Double = x - 5, xField As Double = x + 5, y As Double = pageSize.Height - 50

            Using headerText = New PdfFormattedText()

                headerText.Font = New PdfFont("Helvetica", 12)
                headerText.FontWeight = PdfFontWeight.Bold
                headerText.MaxTextWidth = pageSize.Width - 100
                headerText.AppendLine("PDF Form Example")
                headerText.FontWeight = PdfFontWeight.Normal
                headerText.Append("This is an example of a user fillable PDF form. The fields of this form have been selected to demonstrate as many as possible of the common entry fields.")
                page.Content.DrawText(headerText, New PdfPoint(50, y - headerText.Height))
                y -= 100
            End Using

            Using labelText = New PdfFormattedText()

                labelText.TextAlignment = PdfTextAlignment.Right
                labelText.Font = New PdfFont("Helvetica", 12)

                ' Add a 'Full name' label and a 'FullName' text field.
                labelText.Append("Full name:")
                page.Content.DrawText(labelText, New PdfPoint(xLabel, y - labelText.Height))
                Dim fullNameField = document.Form.Fields.AddText(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Width, fieldSize.Height)
                fullNameField.Name = "FullName"

                ' Add an 'ID' label and an 'ID' text field that accepts at most 10 characters that are evenly spaced between vertical, comb-like, lines.
                y -= 40
                labelText.Clear()
                labelText.Append("ID:")
                page.Content.DrawText(labelText, New PdfPoint(xLabel, y - labelText.Height))
                Dim idField = document.Form.Fields.AddText(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Width, fieldSize.Height)
                idField.Name = "ID"
                idField.CombOfCharacters = 10
                ' Make vertical, comb-like, lines black.
                idField.Appearance.BorderColor = PdfColors.Black

                ' Add 'Gender', 'Male', and 'Female' labels and two 'Gender' radio button fields with the choices 'Male' and 'Female'.
                y -= 40
                labelText.Clear()
                labelText.Append("Gender:")
                Dim labelTextHeight = labelText.Height
                page.Content.DrawText(labelText, New PdfPoint(xLabel, y - labelTextHeight))
                document.Form.Fields.NewRadioButtonName = "Gender"
                Dim genderMaleField = document.Form.Fields.AddRadioButton(page, xField, y - (labelTextHeight + fieldSize.Height) / 2, fieldSize.Height, fieldSize.Height)
                genderMaleField.Choice = "Male"
                labelText.Clear()
                labelText.TextAlignment = PdfTextAlignment.Left
                labelText.Append("Male")
                page.Content.DrawText(labelText, New PdfPoint(xField + fieldSize.Height + 5, y - (labelTextHeight + labelText.Height) / 2))
                Dim genderFemaleField = document.Form.Fields.AddRadioButton(page, xField + fieldSize.Width / 2, y - (labelTextHeight + fieldSize.Height) / 2, fieldSize.Height, fieldSize.Height)
                genderFemaleField.Choice = "Female"
                labelText.Clear()
                labelText.Append("Female")
                page.Content.DrawText(labelText, New PdfPoint(xField + fieldSize.Width / 2 + fieldSize.Height + 5, y - (labelTextHeight + labelText.Height) / 2))

                ' Add a 'Married' label and a 'Married' check box field with the export value 'Yes'.
                y -= 40
                labelText.Clear()
                labelText.TextAlignment = PdfTextAlignment.Right
                labelText.Append("Married:")
                page.Content.DrawText(labelText, New PdfPoint(xLabel, y - labelText.Height))
                Dim marriedField = document.Form.Fields.AddCheckBox(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Height, fieldSize.Height)
                marriedField.Name = "Married"
                marriedField.ExportValue = "Yes"

                ' Add a 'City' label and a 'City' combo box field that contains several predefined values and allows the user to enter a custom value.
                y -= 40
                labelText.Clear()
                labelText.Append("City:")
                page.Content.DrawText(labelText, New PdfPoint(xLabel, y - labelText.Height))
                Dim cityField = document.Form.Fields.AddDropdown(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Width, fieldSize.Height)
                cityField.Name = "City"
                cityField.Items.Add("New York")
                cityField.Items.Add("London")
                cityField.Items.Add("Berlin")
                cityField.Items.Add("Paris")
                cityField.Items.Add("Rome")
                cityField.AllowCustomText = True

                ' Add a 'Language' label and a 'Language' list box field that contains several predefined values and allows the user to select more than one value.
                y -= 40
                labelText.Clear()
                labelText.Append("Language:")
                page.Content.DrawText(labelText, New PdfPoint(xLabel, y - labelText.Height))
                Dim languageField = document.Form.Fields.AddListBox(page, xField, y - 60, fieldSize.Width, 60)
                languageField.Name = "Language"
                languageField.Items.Add("English")
                languageField.Items.Add("German")
                languageField.Items.Add("French")
                languageField.Items.Add("Italian")
                languageField.MultipleSelection = True

                ' Add a 'Notes' label and a 'Notes' text field that may contain multiple lines of text.
                y -= 80
                labelText.Clear()
                labelText.Append("Notes:")
                page.Content.DrawText(labelText, New PdfPoint(xLabel, y - labelText.Height))
                Dim notesField = document.Form.Fields.AddText(page, xField, y - 80, fieldSize.Width, 80)
                notesField.Name = "Notes"
                notesField.MultiLine = True

                ' Add a 'ResetButton' button field with an action that resets all form fields to their default values.
                y -= 100
                Dim resetField = document.Form.Fields.AddButton(page, xField, y - fieldSize.Height, fieldSize.Width, fieldSize.Height)
                resetField.Name = "ResetButton"
                resetField.Appearance.Label = "Reset"
                resetField.Actions.AddResetForm()
            End Using

            document.Save("Form.pdf")
        End Using
    End Sub
End Module
