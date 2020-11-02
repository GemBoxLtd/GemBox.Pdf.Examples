Imports System.Text
Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports GemBox.Pdf.Filters
Imports GemBox.Pdf.Objects
Imports GemBox.Pdf.Text

Module Program

    Sub Main()

        ContentStream()

        ContentStreamSimple()
    End Sub

    Sub ContentStream()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            ' Specify the content stream's content as a sequence of content stream operands and operators.
            Dim content = New StringBuilder()
            ' Begin a text object.
            content.AppendLine("BT")
            ' Set the font and font size to use, installing them as parameters in the text state.
            ' In this case, the font resource identified by the name F1 specifies the font externally known as Helvetica.
            content.AppendLine("/F1 12 Tf")
            ' Specify a starting position on the page, setting parameters in the text object.
            content.AppendLine("70 760 Td")
            ' Paint the glyphs for a string of characters at that position.
            content.AppendLine("(GemBox.Pdf) Tj")
            ' End the text object.
            content.AppendLine("ET")

            ' Create a content stream and write content to it.
            Dim contentStream = PdfStream.Create()
            contentStream.Filters.AddFilter(PdfFilterType.FlateDecode)

            Using stream = contentStream.Open(PdfStreamDataMode.Write, PdfStreamDataState.Decoded)

                Dim contentBytes = PdfEncoding.Byte.GetBytes(content.ToString())
                stream.Write(contentBytes, 0, contentBytes.Length)
            End Using

            ' Create a font dictionary for Standard Type 1 'Helvetica' font.
            Dim font = PdfDictionary.Create()
            font(PdfName.Create("Type")) = PdfName.Create("Font")
            font(PdfName.Create("Subtype")) = PdfName.Create("Type1")
            font(PdfName.Create("BaseFont")) = PdfName.Create("Helvetica")

            ' Add a font dictionary to resources.
            Dim fontResources = PdfDictionary.Create()
            fontResources(PdfName.Create("F1")) = PdfIndirectObject.Create(font)

            Dim resources = PdfDictionary.Create()
            resources(PdfName.Create("Font")) = fontResources

            ' Create a new empty A4 page.
            Dim page = document.Pages.Add()

            ' Set the contents and resources of a page.
            Dim pageDictionary = page.GetDictionary()
            pageDictionary(PdfName.Create("Contents")) = PdfIndirectObject.Create(contentStream)
            pageDictionary(PdfName.Create("Resources")) = resources

            document.Save("Content Stream.pdf")
        End Using
    End Sub

    Sub ContentStreamSimple()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            ' Create a new empty A4 page.
            Dim page = document.Pages.Add()

            Using formattedText = New PdfFormattedText()

                formattedText.Font = New PdfFont("Helvetica", 12)

                formattedText.Append("GemBox.Pdf")

                page.Content.DrawText(formattedText, New PdfPoint(70, 760))
            End Using

            document.Save("Content Stream Simple.pdf")
        End Using
    End Sub
End Module
