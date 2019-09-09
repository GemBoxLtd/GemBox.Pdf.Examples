using GemBox.Pdf;
using GemBox.Pdf.Forms;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Form.pdf"))
        {
            // Get a button field.
            var submitButtonField = (PdfButtonField)document.Form.Fields["Submit"];

            // Create "submit form" field action.
            var submitFormAction = submitButtonField.Actions.AddSubmitForm("https://www.gemboxsoftware.com/");
            // Set XFDF (XML Forms Data Format) as form data export format.
            submitFormAction.ExportFormat = PdfFormDataFormat.XFDF;
            // Submit all form fields.
            submitFormAction.SelectedFields.All = true;

            // Get a button field.
            var resetButtonField = (PdfButtonField)document.Form.Fields["Reset"];

            // Create "reset form" field action.
            var resetFormAction = resetButtonField.Actions.AddResetForm();
            // Reset "Gender" and "Age" fields.
            resetFormAction.SelectedFields.Excluded = false;
            resetFormAction.SelectedFields.Add("Gender");
            resetFormAction.SelectedFields.Add("Age");

            document.Save("Form Actions.pdf");
        }
    }
}
