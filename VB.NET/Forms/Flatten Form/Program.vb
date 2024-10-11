Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("FormFilled.pdf")

            ' A flag specifying whether to construct appearance for all form fields in the document.
            Dim needAppearances = document.Form.NeedAppearances

            For Each field In document.Form.Fields

                ' Do not flatten button fields.
                If field.FieldType = PdfFieldType.Button Then Continue For

                ' Construct appearance, if needed.
                if needAppearances Then field.Appearance.Refresh()

                ' Get the field's appearance form.
                Dim fieldAppearance = field.Appearance.Get()

                ' If the field doesn't have an appearance, skip it.
                If fieldAppearance Is Nothing Then Continue For

                ' Draw field's appearance on the page.
                field.Page.Content.DrawAnnotation(field)
            Next

            ' Remove all fields, thus making the document non-interactive,
            ' since their appearance is now contained directly in the content of their pages.
            document.Form.Fields.Clear()

            document.Save("FormFlattened.pdf")
        End Using
    End Sub
End Module
