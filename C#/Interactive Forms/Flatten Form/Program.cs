using GemBox.Pdf;
using GemBox.Pdf.Content;
using GemBox.Pdf.Forms;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("FormFilled.pdf"))
        {
            foreach (var field in document.Form.Fields)
            {
                // Do not flatten button fields.
                if (field.FieldType == PdfFieldType.Button)
                    continue;

                // Get the field's appearance form.
                var fieldAppearance = field.Appearance.Get();

                // If the field doesn't have an appearance, skip it.
                if (fieldAppearance == null)
                    continue;

                // Add a new content group to the field's page and
                // add a new form content with the field's appearance form to the content group.
                // Content group is added so that transformation from the next statement is localized to the content group.
                var flattenedContent = field.Page.Content.Elements.AddGroup().Elements.AddForm(fieldAppearance);

                // Translate form content to the same position on the page as the field.
                var fieldBounds = field.Bounds;
                flattenedContent.Transform = PdfMatrix.CreateTranslation(fieldBounds.Left, fieldBounds.Bottom);
            }

            // Remove all fields, thus making the document non-interactive,
            // since their appearance is now contained directly in the content of their pages.
            document.Form.Fields.Clear();

            document.Save("FormFlattened.pdf");
        }
    }
}
