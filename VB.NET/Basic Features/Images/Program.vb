Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program
    Sub Main()

        Example1()

        Example2()

        Example3()
    End Sub

    Sub Example1()
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

    Sub Example2()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            ' Add a page.
            Dim page = document.Pages.Add()

            ' Load the image from a file.
            Dim image = PdfImage.Load("FragonardReader.jpg")

            ' Set the location of the bottom-left corner of the image.
            ' We want the top-left corner of the image to be at location (50, 50)
            ' from the top-left corner of the page.
            ' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
            ' and the positive y axis extends vertically upward.
            Dim x As Double = 50, y As Double = page.CropBox.Top - 50 - image.Size.Height

            ' Draw the image to the page.
            page.Content.DrawImage(image, New PdfPoint(x, y))

            document.Save("Import Images.pdf")
        End Using
    End Sub

    Sub Example3()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            ' Load the image from a file.
            Dim image = PdfImage.Load("Corner.png")

            Dim margin As Double = 50

            ' Set the location of the first image in the top-left corner of the page (with a specified margin).
            Dim x As Double = margin
            Dim y As Double = page.CropBox.Top - margin - image.Size.Height

            ' Draw the first image.
            page.Content.DrawImage(image, New PdfPoint(x, y))

            ' Set the location of the second image in the top-right corner of the page (with the same margin).
            x = page.CropBox.Right - margin - image.Size.Width
            y = page.CropBox.Top - margin - image.Size.Height

            ' Initialize the transformation.
            Dim transform = PdfMatrix.Identity
            ' Use the translate operation to position the image.
            transform.Translate(x, y)
            ' Use the scale operation to resize the image.
            ' NOTE: The unit square of user space, bounded by user coordinates (0, 0) and (1, 1), 
            ' corresponds to the boundary of the image in the image space.
            transform.Scale(image.Size.Width, image.Size.Height)
            ' Use the scale operation to flip the image horizontally.
            transform.Scale(-1, 1, 0.5, 0)

            ' Draw the second image.
            page.Content.DrawImage(image, transform)

            ' Set the location of the third image in the bottom-left corner of the page (with the same margin).
            x = margin
            y = margin

            ' Initialize the transformation.
            transform = PdfMatrix.Identity
            ' Use the translate operation to position the image.
            transform.Translate(x, y)
            ' Use the scale operation to resize the image.
            transform.Scale(image.Size.Width, image.Size.Height)
            ' Use the scale operation to flip the image vertically.
            transform.Scale(1, -1, 0, 0.5)

            ' Draw the third image.
            page.Content.DrawImage(image, transform)

            ' Set the location of the fourth image in the bottom-right corner of the page (with the same margin).
            x = page.CropBox.Right - margin - image.Size.Width
            y = margin

            ' Initialize the transformation.
            transform = PdfMatrix.Identity
            ' Use the translate operation to position the image.
            transform.Translate(x, y)
            ' Use the scale operation to resize the image.
            transform.Scale(image.Size.Width, image.Size.Height)
            ' Use the scale operation to flip the image horizontally And vertically.
            transform.Scale(-1, -1, 0.5, 0.5)

            ' Draw the fourth image.
            page.Content.DrawImage(image, transform)

            document.Save("Positioning and Transformations.pdf")
        End Using
    End Sub
End Module
