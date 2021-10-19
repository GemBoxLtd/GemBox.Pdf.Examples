Imports System.IO
Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports GemBox.Pdf.Content.Marked

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            ' Make Attachments panel visible.
            document.PageMode = PdfPageMode.UseAttachments

            Using sourceDocument = PdfDocument.Load("Invoice.pdf")

                ' Import the first page of an 'Invoice.pdf' document.
                Dim page = document.Pages.AddClone(sourceDocument.Pages(0))

                ' Associate the 'Invoice.docx' file to the imported page as a source file and also add it to the document's embedded files.
                page.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Source, "Invoice.docx", Nothing, document.EmbeddedFiles)
            End Using

            Using sourceDocument = PdfDocument.Load("Chart.pdf")

                ' Import the first page of a 'Chart.pdf' document.
                Dim page = document.Pages.AddClone(sourceDocument.Pages(0))

                ' Group the content of an imported page and mark it with the 'AF' tag.
                Dim chartContentGroup = page.Content.Elements.Group(page.Content.Elements.First, page.Content.Elements.Last)
                Dim markStart = chartContentGroup.Elements.AddMarkStart(New PdfContentMarkTag(PdfContentMarkTagRole.AF), chartContentGroup.Elements.First)
                chartContentGroup.Elements.AddMarkEnd()

                ' Associate the 'Chart.xlsx' to the marked content as a source file and also add it to the document's embedded files.
                ' The 'Chart.xlsx' file is associated without using a file system utility code.
                Dim embeddedFile = markStart.AssociatedFiles.AddEmpty(PdfAssociatedFileRelationshipType.Source, "Chart.xlsx", Nothing, document.EmbeddedFiles).EmbeddedFile
                ' Associated file must specify modification date.
                embeddedFile.ModificationDate = File.GetLastWriteTime("Chart.xlsx")
                ' Associated file stream is not compressed since the source file, 'Chart.xlsx', is already compressed.
                Using fileStream = File.OpenRead("Chart.xlsx")
                    Using embeddedFileStream = embeddedFile.OpenWrite(compress:=False)
                        fileStream.CopyTo(embeddedFileStream)
                    End Using
                End Using

                ' Associate another file, the 'ChartData.csv', to the marked content as a data file and also add it to the document's embedded files.
                markStart.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Data, "ChartData.csv", Nothing, document.EmbeddedFiles)
            End Using

            Using sourceDocument = PdfDocument.Load("Equation.pdf")

                ' Import the first page of an 'Equation.pdf' document into a form (PDF equivalent of a vector image).
                Dim form As PdfForm = sourceDocument.Pages(0).ConvertToForm(document)
                Dim page = document.Pages(1)

                ' Add the imported form to the bottom-left corner of the second page.
                page.Content.Elements.AddForm(form)

                ' Associate the 'Equation.mml' to the imported form as a supplement file and also add it to the document's embedded files.
                ' Associated file must specify media type and since GemBox.Pdf doesn't have built-in support for '.mml' file extension,
                ' the media type 'application/mathml+xml' is specified explicitly.
                form.AssociatedFiles.Add(PdfAssociatedFileRelationshipType.Supplement, "Equation.mml", "application/mathml+xml", document.EmbeddedFiles)
            End Using

            document.Save("Associated Files.pdf")
        End Using
    End Sub
End Module
