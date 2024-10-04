using GemBox.Pdf;
using GemBox.Pdf.Content;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = new PdfDocument();
        var page = document.Pages.Add();

        PdfSize pageSize = page.Size, fieldSize = new(150, 20);
        double x = pageSize.Width / 2 - 100, xLabel = x - 5, xField = x + 5, y = pageSize.Height - 50;

        using (var headerText = new PdfFormattedText())
        {
            headerText.Font = new PdfFont("Helvetica", 12);
            headerText.FontWeight = PdfFontWeight.Bold;
            headerText.MaxTextWidth = pageSize.Width - 100;
            headerText.AppendLine("PDF Form Example");
            headerText.FontWeight = PdfFontWeight.Normal;
            headerText.Append("This is an example of a user fillable PDF form. The fields of this form have been selected to demonstrate as many as possible of the common entry fields.");
            page.Content.DrawText(headerText, new PdfPoint(50, y - headerText.Height));
            y -= 100;
        }

        using (var labelText = new PdfFormattedText())
        {
            labelText.TextAlignment = PdfTextAlignment.Right;
            labelText.Font = new PdfFont("Helvetica", 12);

            // Add a 'Full name' label and a 'FullName' text field.
            labelText.Append("Full name:");
            page.Content.DrawText(labelText, new PdfPoint(xLabel, y - labelText.Height));
            var fullNameField = document.Form.Fields.AddText(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Width, fieldSize.Height);
            fullNameField.Name = "FullName";

            // Add an 'ID' label and an 'ID' text field that accepts at most 10 characters that are evenly spaced between vertical, comb-like, lines.
            y -= 40;
            labelText.Clear();
            labelText.Append("ID:");
            page.Content.DrawText(labelText, new PdfPoint(xLabel, y - labelText.Height));
            var idField = document.Form.Fields.AddText(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Width, fieldSize.Height);
            idField.Name = "ID";
            idField.CombOfCharacters = 10;
            // Make vertical comb-like lines, colored black.
            idField.Appearance.BorderColor = PdfColors.Black;

            // Add 'Gender', 'Male', and 'Female' labels and two 'Gender' radio button fields with the choices 'Male' and 'Female'.
            y -= 40;
            labelText.Clear();
            labelText.Append("Gender:");
            var labelTextHeight = labelText.Height;
            page.Content.DrawText(labelText, new PdfPoint(xLabel, y - labelTextHeight));
            document.Form.Fields.NewRadioButtonName = "Gender";
            var genderMaleField = document.Form.Fields.AddRadioButton(page, xField, y - (labelTextHeight + fieldSize.Height) / 2, fieldSize.Height, fieldSize.Height);
            genderMaleField.Choice = "Male";
            labelText.Clear();
            labelText.TextAlignment = PdfTextAlignment.Left;
            labelText.Append("Male");
            page.Content.DrawText(labelText, new PdfPoint(xField + fieldSize.Height + 5, y - (labelTextHeight + labelText.Height) / 2));
            var genderFemaleField = document.Form.Fields.AddRadioButton(page, xField + fieldSize.Width / 2, y - (labelTextHeight + fieldSize.Height) / 2, fieldSize.Height, fieldSize.Height);
            genderFemaleField.Choice = "Female";
            labelText.Clear();
            labelText.Append("Female");
            page.Content.DrawText(labelText, new PdfPoint(xField + fieldSize.Width / 2 + fieldSize.Height + 5, y - (labelTextHeight + labelText.Height) / 2));

            // Add a 'Married' label and a 'Married' check box field with the export value 'Yes'.
            y -= 40;
            labelText.Clear();
            labelText.TextAlignment = PdfTextAlignment.Right;
            labelText.Append("Married:");
            page.Content.DrawText(labelText, new PdfPoint(xLabel, y - labelText.Height));
            var marriedField = document.Form.Fields.AddCheckBox(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Height, fieldSize.Height);
            marriedField.Name = "Married";
            marriedField.ExportValue = "Yes";

            // Add a 'City' label and a 'City' combo box field that contains several predefined values and allows the user to enter a custom value.
            y -= 40;
            labelText.Clear();
            labelText.Append("City:");
            page.Content.DrawText(labelText, new PdfPoint(xLabel, y - labelText.Height));
            var cityField = document.Form.Fields.AddDropdown(page, xField, y - (labelText.Height + fieldSize.Height) / 2, fieldSize.Width, fieldSize.Height);
            cityField.Name = "City";
            cityField.Items.Add("New York");
            cityField.Items.Add("London");
            cityField.Items.Add("Berlin");
            cityField.Items.Add("Paris");
            cityField.Items.Add("Rome");
            cityField.AllowCustomText = true;

            // Add a 'Language' label and a 'Language' list box field that contains several predefined values and allows the user to select more than one value.
            y -= 40;
            labelText.Clear();
            labelText.Append("Language:");
            page.Content.DrawText(labelText, new PdfPoint(xLabel, y - labelText.Height));
            var languageField = document.Form.Fields.AddListBox(page, xField, y - 60, fieldSize.Width, 60);
            languageField.Name = "Language";
            languageField.Items.Add("English");
            languageField.Items.Add("German");
            languageField.Items.Add("French");
            languageField.Items.Add("Italian");
            languageField.MultipleSelection = true;

            // Add a 'Notes' label and a 'Notes' text field that may contain multiple lines of text.
            y -= 80;
            labelText.Clear();
            labelText.Append("Notes:");
            page.Content.DrawText(labelText, new PdfPoint(xLabel, y - labelText.Height));
            var notesField = document.Form.Fields.AddText(page, xField, y - 80, fieldSize.Width, 80);
            notesField.Name = "Notes";
            notesField.MultiLine = true;

            // Add a 'ResetButton' button field with an action that resets all form fields to their default values.
            y -= 100;
            var resetField = document.Form.Fields.AddButton(page, xField, y - fieldSize.Height, fieldSize.Width, fieldSize.Height);
            resetField.Name = "ResetButton";
            resetField.Appearance.Label = "Reset";
            resetField.Actions.AddResetForm();
        }

        document.Save("Form.pdf");
    }
}
