using GemBox.Pdf;
using GemBox.Pdf.Content;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        Example1();
        Example2();
        Example3();
        Example4();
        Example5();
    }

    static void Example1()
    {
        using (var document = PdfDocument.Load("Invoice.pdf"))
        {
            var page = document.Pages[0];

            // Adding and applying redaction annotation to the area with the content we want to redact.
            var redaction = page.Annotations.AddRedaction(300, 440, 225, 160);
            redaction.Apply();

            document.Save("Redacted.pdf");
        }
    }

    static void Example2()
    {
        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            var page = document.Pages[0];
            
            // Adding redaction annotation with any non-zero area.
            var redaction = page.Annotations.AddRedaction(0, 0, 1, 1);
            
            // Adding quads for the areas we want to redact
            redaction.Quads.Add(new PdfQuad(0, 0, 100, 100));
            redaction.Quads.Add(new PdfQuad(200, 0, 300, 100));
            redaction.Quads.Add(new PdfQuad(400, 0, 500, 100));
            redaction.Quads.Add(new PdfQuad(100, 100, 200, 200));
            redaction.Quads.Add(new PdfQuad(300, 100, 400, 200));
            redaction.Quads.Add(new PdfQuad(0, 200, 100, 300));
            redaction.Quads.Add(new PdfQuad(200, 200, 300, 300));
            redaction.Quads.Add(new PdfQuad(400, 200, 500, 300));
            redaction.Quads.Add(new PdfQuad(100, 300, 200, 400));
            redaction.Quads.Add(new PdfQuad(300, 300, 400, 400));
            redaction.Quads.Add(new PdfQuad(0, 400, 100, 500));
            redaction.Quads.Add(new PdfQuad(200, 400, 300, 500));
            redaction.Quads.Add(new PdfQuad(400, 400, 500, 500));
            
            // Applying redaction to remove all content in the area.
            redaction.Apply();
            
            document.Save("MultipleRedactions.pdf");
        }
    }

    static void Example3()
    {
        using (var document = PdfDocument.Load("Document.pdf"))
        {
            // Applying all redactions existing in the PDF document
            foreach (var page in document.Pages)
                page.Annotations.ApplyRedactions();
                
            document.Save("RedactedOutput.pdf");
        }
    }

    static void Example4()
    {
        using (var document = PdfDocument.Load("Invoice.pdf"))
        {
            var page = document.Pages[0];
            
            // Regex to match with decimal numbers
            var regex = new Regex(@"\d+\.\d+");
            
            // Redacting everything that matches with the regex
            foreach (PdfText text in page.Content.GetText().Find(regex))
                text.Redact();
                
            document.Save("RegexRedactedOutput.pdf");
        }
    }

    static void Example5()
    {
        using (var document = PdfDocument.Load("Invoice.pdf"))
        {
            var page = document.Pages[0];
            var redaction = page.Annotations.AddRedaction(0, 0, 1, 1);
            var regex = new Regex(@"\d+\.\d+");

            // Adding quads for each matching text
            foreach (PdfText text in page.Content.GetText().Find(regex))
                redaction.Quads.Add(text.Bounds);

            // Setting custom fill color for the redacted areas
            redaction.Appearance.RedactedAreaFillColor = PdfColor.FromRgb(0.95, 0.4, 0.14);
            redaction.Apply();

            document.Save("CustomFilledRedactedOutput.pdf");
        }
    }
}
