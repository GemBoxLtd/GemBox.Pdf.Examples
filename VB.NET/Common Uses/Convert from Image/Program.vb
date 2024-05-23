Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()
        Example1()
        Example2()
        Example3()
    End Sub

    Sub Example1()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Create new document.
        Using document As New PdfDocument()

            ' Add new page.
            Dim page = document.Pages.Add()

            ' Add image from PNG file.
            Dim image = PdfImage.Load("parrot.png")
            page.Content.DrawImage(image, New PdfPoint(0, 0))

            ' Set page size.
            page.SetMediaBox(image.Width, image.Height)

            ' Save as PDF file.
            document.Save("converted-png-image.pdf")

        End Using
    End Sub

    Sub Example2()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim jpgs As String() = {"penguin.jpg", "jellyfish.jpg", "dolphin.jpg", "lion.jpg", "deer.jpg"}

        ' Create New document.
        Using document As New PdfDocument()

            ' For each image add new page with margins.
            For Each jpg In jpgs
                Dim page = document.Pages.Add()
                Dim margins As Double = 20

                ' Load image from JPG file.
                Dim image = PdfImage.Load(jpg)

                ' Set page size.
                page.SetMediaBox(image.Width + 2 * margins, image.Height + 2 * margins)

                ' Draw backgroud color.
                Dim backgroud = page.Content.Elements.AddPath()
                backgroud.AddRectangle(New PdfPoint(0, 0), page.Size)
                backgroud.Format.Fill.IsApplied = True
                backgroud.Format.Fill.Color = PdfColor.FromRgb(1, 0, 1)

                ' Draw image.
                page.Content.DrawImage(image, New PdfPoint(margins, margins))
            Next

            ' Save as PDF file.
            document.Save("converted-jpg-images.pdf")
        End Using
    End Sub

    Sub Example3()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Create new document.
        Using document As New PdfDocument()

            ' Load image from PNG file.
            Dim image = PdfImage.Load("parrot.png")

            Dim width As Double = image.Width
            Dim height As Double = image.Height
            Dim ratio As Double = width / height

            ' Add image four times, each time with 20% smaller size.
            For i = 0 To 3
                width *= 0.8
                height = width / ratio

                Dim page = document.Pages.Add()
                page.Content.DrawImage(image, New PdfPoint(0, 0), New PdfSize(width, height))
                page.SetMediaBox(width, height)
            Next

            document.Save("converted-scaled-png-images.pdf")

        End Using
    End Sub

End Module
