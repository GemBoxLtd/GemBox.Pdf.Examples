Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim form = New PdfForm(document, New PdfSize(200, 200))

            form.Content.BeginEdit()

            Dim textGroup = form.Content.Elements.AddGroup()

            ' Add text with the default fill (fill will be inherited from outer PdfFormContent).
            Using formattedText = New PdfFormattedText()

                formattedText.Font = New PdfFont("Helvetica", 24)
                formattedText.Append("Hello world!")

                ' Draw the formatted text at location (50, 150) from the bottom-left corner of the group/form.
                textGroup.DrawText(formattedText, New PdfPoint(50, 150))
            End Using

            ' Add the same text with a black fill 50 points below the first text.
            Dim blackText = CType(textGroup.Elements.AddClone(textGroup.Elements.First), PdfTextContent)
            blackText.TextTransform = PdfMatrix.CreateTranslation(0, -50) * blackText.TextTransform
            blackText.Format.Fill.Color = PdfColors.Black

            Dim pathGroup = form.Content.Elements.AddGroup()

            ' Add a rectangle path with the default fill (fill will be inherited from the outer PdfFormContent).
            Dim path = pathGroup.Elements.AddPath()
            path.AddRectangle(0, 50, 200, 40)
            path.Format.Fill.IsApplied = True

            ' Add the same path with a black fill 50 points below the first path.
            Dim blackPath = pathGroup.Elements.AddClone(path)
            blackPath.Subpaths.Transform(PdfMatrix.CreateTranslation(0, -50))
            blackPath.Format.Fill.Color = PdfColors.Black

            form.Content.EndEdit()

            Dim page = document.Pages.Add()

            ' Add the outer PdfFormContent with the default (black) fill.
            ' Elements in the inner PdfForm that do not have a fill set, will have the default (black) fill.
            Dim contentGroup = page.Content.Elements.AddGroup()
            Dim formContent1 = contentGroup.Elements.AddForm(form)
            formContent1.Transform = PdfMatrix.CreateTranslation(100, 600)

            ' Add the outer PdfFormContent with a green fill.
            ' Elements in the inner PdfForm that do not have a fill set, will have a green fill.
            contentGroup = page.Content.Elements.AddGroup()
            Dim formContent2 = contentGroup.Elements.AddForm(form)
            formContent2.Transform = PdfMatrix.CreateTranslation(100, 350)
            formContent2.Format.Fill.Color = PdfColors.Green

            ' Add the outer PdfFormContent with a red fill.
            ' Elements in the inner PdfForm that do not have a fill set, will have a red fill.
            contentGroup = page.Content.Elements.AddGroup()
            Dim formContent3 = contentGroup.Elements.AddClone(formContent1)
            formContent3.Transform = PdfMatrix.CreateTranslation(100, 100)
            formContent3.Format.Fill.Color = PdfColors.Red

            document.Save("FormXObjects.pdf")
        End Using
    End Sub
End Module