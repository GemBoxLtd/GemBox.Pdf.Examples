Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports System.Text.RegularExpressions

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Example1()
        Example2()
        Example3()
        Example4()
        Example5()

    End Sub

    Sub Example1()
        Using document = PdfDocument.Load("Invoice.pdf")
            Dim page = document.Pages(0)

            ' Adding and applying redaction annotation to the area with the content we want to redact.
            Dim redaction = page.Annotations.AddRedaction(300, 440, 225, 160)
            redaction.Apply()

            document.Save("Redacted.pdf")
        End Using
    End Sub

    Sub Example2()
        Using document = PdfDocument.Load("Reading.pdf")
            Dim page = document.Pages(0)

            ' Adding redaction annotation with any non-zero area.
            Dim redaction = page.Annotations.AddRedaction(0, 0, 1, 1)

            ' Adding quads for the areas we want to redact
            redaction.Quads.Add(New PdfQuad(0, 0, 100, 100))
            redaction.Quads.Add(New PdfQuad(200, 0, 300, 100))
            redaction.Quads.Add(New PdfQuad(400, 0, 500, 100))
            redaction.Quads.Add(New PdfQuad(100, 100, 200, 200))
            redaction.Quads.Add(New PdfQuad(300, 100, 400, 200))
            redaction.Quads.Add(New PdfQuad(0, 200, 100, 300))
            redaction.Quads.Add(New PdfQuad(200, 200, 300, 300))
            redaction.Quads.Add(New PdfQuad(400, 200, 500, 300))
            redaction.Quads.Add(New PdfQuad(100, 300, 200, 400))
            redaction.Quads.Add(New PdfQuad(300, 300, 400, 400))
            redaction.Quads.Add(New PdfQuad(0, 400, 100, 500))
            redaction.Quads.Add(New PdfQuad(200, 400, 300, 500))
            redaction.Quads.Add(New PdfQuad(400, 400, 500, 500))

            ' Applying redaction to remove all content in the area.
            redaction.Apply()

            document.Save("MultipleRedactions.pdf")
        End Using
    End Sub

    Sub Example3()
        Using document = PdfDocument.Load("Document.pdf")
            ' Applying all redactions existing in the PDF document
            For Each page In document.Pages
                page.Annotations.ApplyRedactions()
            Next

            document.Save("RedactedOutput.pdf")
        End Using
    End Sub

    Sub Example4()
        Using document = PdfDocument.Load("Invoice.pdf")
            Dim page = document.Pages(0)

            ' Regex to match with decimal numbers
            Dim regex = New Regex("\d+\.\d+")

            ' Redacting everything that matches with the regex
            For Each text As PdfText In page.Content.GetText().Find(regex)
                text.Redact()
            Next

            document.Save("RegexRedactedOutput.pdf")
        End Using
    End Sub

    Sub Example5()
        Using document = PdfDocument.Load("Invoice.pdf")
            Dim page = document.Pages(0)
            Dim redaction = page.Annotations.AddRedaction(0, 0, 1, 1)
            Dim regex = New Regex("\d+\.\d+")

            ' Adding quads for each matching text
            For Each text As PdfText In page.Content.GetText().Find(regex)
                redaction.Quads.Add(text.Bounds)
            Next

            ' Setting custom fill color for the redacted areas
            redaction.Appearance.RedactedAreaFillColor = PdfColor.FromRgb(0.95, 0.4, 0.14)
            redaction.Apply()

            document.Save("CustomFilledRedactedOutput.pdf")
        End Using
    End Sub

End Module