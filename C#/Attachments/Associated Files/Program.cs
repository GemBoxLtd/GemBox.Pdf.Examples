using GemBox.Pdf;
using GemBox.Pdf.Content;
using GemBox.Pdf.Content.Marked;
using System.IO;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // Make Attachments panel visible.
            document.PageMode = PdfPageMode.UseAttachments;

            using (var sourceDocument = PdfDocument.Load("Invoice.pdf"))
            {
                // Import the first page of an 'Invoice.pdf' document.
                var page = document.Pages.AddClone(sourceDocument.Pages[0]);

                // Associate the 'Invoice.docx' file to the imported page as a source file and also add it to the document's embedded files.
                page.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Source, "Invoice.docx", null, document.EmbeddedFiles);
            }

            using (var sourceDocument = PdfDocument.Load("Chart.pdf"))
            {
                // Import the first page of a 'Chart.pdf' document.
                var page = document.Pages.AddClone(sourceDocument.Pages[0]);

                // Group the content of an imported page and mark it with the 'AF' tag.
                var chartContentGroup = page.Content.Elements.Group(page.Content.Elements.First, page.Content.Elements.Last);
                var markStart = chartContentGroup.Elements.AddMarkStart(new PdfContentMarkTag(PdfContentMarkTagRole.AF), chartContentGroup.Elements.First);
                chartContentGroup.Elements.AddMarkEnd();

                // Associate the 'Chart.xlsx' to the marked content as a source file and also add it to the document's embedded files.
                // The 'Chart.xlsx' file is associated without using a file system utility code.
                var embeddedFile = markStart.AssociatedFiles.AddEmpty(PdfAssociatedFileRelationshipType.Source, "Chart.xlsx", null, document.EmbeddedFiles).EmbeddedFile;
                // Associated file must specify modification date.
                embeddedFile.ModificationDate = File.GetLastWriteTime("Chart.xlsx");
                // Associated file stream is not compressed since the source file, 'Chart.xlsx', is already compressed.
                using (var fileStream = File.OpenRead("Chart.xlsx"))
                using (var embeddedFileStream = embeddedFile.OpenWrite(compress: false))
                    fileStream.CopyTo(embeddedFileStream);

                // Associate another file, the 'ChartData.csv', to the marked content as a data file and also add it to the document's embedded files.
                markStart.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Data, "ChartData.csv", null, document.EmbeddedFiles);
            }

            using (var sourceDocument = PdfDocument.Load("Equation.pdf"))
            {
                // Import the first page of an 'Equation.pdf' document into a form (PDF equivalent of a vector image).
                PdfForm form = sourceDocument.Pages[0].ConvertToForm(document);

                var page = document.Pages[1];

                // Add the imported form to the bottom-left corner of the second page.
                page.Content.Elements.AddForm(form);

                // Associate the 'Equation.mml' to the imported form as a supplement file and also add it to the document's embedded files.
                // Associated file must specify media type and since GemBox.Pdf doesn't have built-in support for '.mml' file extension,
                // the media type 'application/mathml+xml' is specified explicitly.
                form.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Supplement, "Equation.mml", "application/mathml+xml", document.EmbeddedFiles);
            }

            document.Save("Associated Files.pdf");
        }
    }
}
