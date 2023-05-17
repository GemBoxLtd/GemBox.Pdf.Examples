Imports GemBox.Pdf
Imports GemBox.Pdf.Content.Marked
Imports GemBox.Pdf.Objects

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            ' Surround the path with the marked content start and marked content end elements.
            Dim markStart = page.Content.Elements.AddMarkStart(New PdfContentMarkTag(PdfContentMarkTagRole.Span))

            Dim markedProperties = markStart.GetEditableProperties().GetDictionary()

            ' Set replacement text for a path, as specified in http://www.adobe.com/content/dam/acom/en/devnet/pdf/PDF32000_2008.pdf#page=623
            markedProperties(PdfName.Create("ActualText")) = PdfString.Create("H")

            ' Add the path that is a visual representation of the letter 'H'.
            Dim path = page.Content.Elements.AddPath() _
                .BeginSubpath(100, 600).LineTo(100, 800) _
                .BeginSubpath(100, 700).LineTo(200, 700) _
                .BeginSubpath(200, 600).LineTo(200, 800)

            Dim format = path.Format
            format.Stroke.IsApplied = True
            format.Stroke.Width = 10

            page.Content.Elements.AddMarkEnd()

            document.Save("MarkedContent.pdf")
        End Using
    End Sub
End Module