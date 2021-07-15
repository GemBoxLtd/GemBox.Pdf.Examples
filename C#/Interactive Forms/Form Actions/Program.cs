using GemBox.Pdf;
using GemBox.Pdf.Annotations;
using GemBox.Pdf.Content;
using GemBox.Pdf.Forms;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Form.pdf"))
        {
            // Update action and label of a 'ResetButton' field so that only 'Notes' field is reset.
            var resetButtonField = (PdfButtonField)document.Form.Fields["ResetButton"];
            var resetFormAction = (PdfResetFormAction)resetButtonField.Actions[0];
            resetFormAction.SelectedFields.Excluded = false;
            resetFormAction.SelectedFields.Add("Notes");
            resetButtonField.Appearance.Label = "Reset Notes";

            var bounds = resetButtonField.Bounds;

            // Add 'ImportButton' field with label and icon that imports field values from the FDF (Forms Data Format) file.
            var importButtonField = document.Form.Fields.AddButton(document.Pages[0], bounds.Left, bounds.Bottom - 80, 150, 60);
            importButtonField.Name = "ImportButton";
            importButtonField.Actions.AddImportFormData("FormData.fdf");
            var appearance = importButtonField.Appearance;
            appearance.LabelPlacement = PdfTextPlacement.TextAboveIcon;
            appearance.Label = "Import Data";
            var icon = new PdfForm(document, new PdfSize(128, 128));
            icon.Content.BeginEdit();
            icon.Content.DrawImage(PdfImage.Load("import.png"), new PdfPoint(0, 0), new PdfSize(128, 128));
            icon.Content.EndEdit();
            appearance.Icon = icon;

            bounds = importButtonField.Bounds;

            // Add 'SubmitButton' field with icon that submits all field values to the URL in XFDF (XML Forms Data Format) format.
            var submitButtonField = document.Form.Fields.AddButton(document.Pages[0], bounds.Left, bounds.Bottom - 60, 150, 40);
            submitButtonField.Name = "SubmitButton";
            var submitFormAction = submitButtonField.Actions.AddSubmitForm("https://www.gemboxsoftware.com/");
            submitFormAction.ExportFormat = PdfFormDataFormat.XFDF;
            submitFormAction.SelectedFields.All = true;
            appearance = submitButtonField.Appearance;
            appearance.LabelPlacement = PdfTextPlacement.IconOnly;
            icon = new PdfForm(document, new PdfSize(128, 128));
            icon.Content.BeginEdit();
            icon.Content.DrawImage(PdfImage.Load("submit.png"), new PdfPoint(0, 0), new PdfSize(128, 128));
            icon.Content.EndEdit();
            appearance.Icon = icon;

            document.Save("Form Actions.pdf");
        }
    }
}
