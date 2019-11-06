Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("ExportImages.pdf")
            ' Iterate through PDF pages and through each page's content elements.
            For Each page In document.Pages
                For Each contentElement In page.Content.Elements.All()
                    If contentElement.ElementType = PdfContentElementType.Image Then
                        ' Export an image content element to selected image format.
                        Dim imageContent = CType(contentElement, PdfImageContent)
                        imageContent.Save("ExportImages.jpg")
                        Return
                    End If
                Next
            Next
        End Using

    End Sub

End Module
