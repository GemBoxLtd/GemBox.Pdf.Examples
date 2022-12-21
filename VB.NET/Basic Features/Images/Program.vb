Imports System
Imports System.IO
Imports System.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Content

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
        Using document = PdfDocument.Load("ExportImages.pdf")

            ' Iterate through PDF pages.
            For Each page In document.Pages

                ' Get all image content elements on the page.
                Dim imageElements = page.Content.Elements.All().OfType(Of PdfImageContent)().ToList()

                ' Export the first image element to an image file.
                If imageElements.Count > 0 Then
                    imageElements(0).Save("Export Images.jpeg")
                    Exit For
                End If

            Next

        End Using
    End Sub

    Sub Example2()
        Using document = PdfDocument.Load("ExportImages.pdf")

            ' Iterate through all PDF pages and through each page's content elements,
            ' and retrieve only the image content elements.
            For index As Integer = 0 To document.Pages.Count - 1
                Dim page = document.Pages(index)
                Dim contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator()

                While contentEnumerator.MoveNext()
                    If contentEnumerator.Current.ElementType = PdfContentElementType.Image Then
                        Dim imageElement = CType(contentEnumerator.Current, PdfImageContent)
                        Console.Write($"Image on page {index + 1} | ")

                        Dim bounds = imageElement.Bounds
                        contentEnumerator.Transform.Transform(bounds)
                        Console.Write($"from ({bounds.Left},{bounds.Bottom}) to ({bounds.Right},{bounds.Top}) | ")

                        Dim image = imageElement.Image
                        Console.WriteLine($"size {image.Size.Width}x{image.Size.Height}")
                    End If
                End While
            Next

        End Using
    End Sub

    Sub Example3()
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

    Sub Example4()
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

    Sub Example5()
        Dim imageFiles = Directory.EnumerateFiles("Images")

        Dim imageCounter As Integer = 0
        Dim chunkSize As Integer = 1000

        Using document = New PdfDocument()

            ' Create output PDF file that will have large number of images imported into it.
            document.Save("Import Many Images.pdf")

            For Each imageFile In imageFiles

                Dim page = document.Pages.Add()
                Dim image = PdfImage.Load(imageFile)

                Dim ratioX As Double = page.Size.Width / image.Width
                Dim ratioY As Double = page.Size.Height / image.Height
                Dim ratio As Double = Math.Min(ratioX, ratioY)

                Dim imageSize = If(ratio < 1,
                    New PdfSize(image.Width * ratio, image.Height * ratio),
                    New PdfSize(image.Width, image.Height))
                Dim imagePosition = New PdfPoint(0, page.Size.Height - imageSize.Height)
                page.Content.DrawImage(image, imagePosition, imageSize)

                imageCounter += 1
                If imageCounter Mod chunkSize = 0 Then

                    ' Save the new images that were added after the document was last saved.
                    document.Save()

                    ' Clear previously parsed images and thus free memory necessary for merging additional pages.
                    document.Unload()

                End If
            Next

            ' Save the last chunk of imported images.
            document.Save()
        End Using
    End Sub

End Module
