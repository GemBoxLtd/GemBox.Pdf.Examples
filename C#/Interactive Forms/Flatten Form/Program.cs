using GemBox.Pdf;
using GemBox.Pdf.Forms;

namespace FlattenForm;

static class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("FormFilled.pdf");
        // A flag specifying whether to construct appearance for all form fields in the document.
        bool needAppearances = document.Form.NeedAppearances;

        foreach (var field in document.Form.Fields)
        {
            // Do not flatten button fields.
            if (field.FieldType == PdfFieldType.Button)
            {
                continue;
            }

            // Construct appearance, if needed.
            if (needAppearances)
            {
                field.Appearance.Refresh();
            }

            // Get the field's appearance form.
            var fieldAppearance = field.Appearance.Get();

            // If the field doesn't have an appearance, skip it.
            if (fieldAppearance == null)
            {
                continue;
            }

            // Draw field's appearance on the page.
            field.Page.Content.DrawAnnotation(field);
        }

        // Remove all fields, thus making the document non-interactive,
        // since their appearance is now contained directly in the content of their pages.
        document.Form.Fields.Clear();

        document.Save("FormFlattened.pdf");
    }
}