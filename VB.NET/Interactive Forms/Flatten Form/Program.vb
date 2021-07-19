Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("FormFilled.pdf")

            For Each field In document.Form.Fields

                ' Do not flatten button fields.
                If field.FieldType = PdfFieldType.Button Then Continue For

                ' Get the field's appearance form.
                Dim fieldAppearance = field.Appearance.Get()

                ' If the field doesn't have an appearance, skip it.
                If fieldAppearance Is Nothing Then Continue For

                ' Add a new content group to the field's page and
                ' add a new form content with the field's appearance form to the content group.
                ' Content group is added so that transformation from the next statement is localized to the content group.
                Dim flattenedContent = field.Page.Content.Elements.AddGroup().Elements.AddForm(fieldAppearance)

                ' Translate form content to the same position on the page as the field.
                Dim fieldBounds = field.Bounds
                flattenedContent.Transform = PdfMatrix.CreateTranslation(fieldBounds.Left, fieldBounds.Bottom)
            Next

            ' Remove all fields, thus making the document non-interactive,
            ' since their appearance is now contained directly in the content of their pages.
            document.Form.Fields.Clear()

            document.Save("FormFlattened.pdf")
        End Using
    End Sub
End Module
