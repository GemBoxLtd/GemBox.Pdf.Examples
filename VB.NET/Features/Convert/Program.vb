Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports System.IO
Imports System.IO.Compression

Module Program

    Sub Main()

        Example1()
        Example2()
        Example3()

    End Sub

    Sub Example1()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load a PDF document.
        Using document = PdfDocument.Load("Input.pdf")

            ' Create image save options.
            Dim imageOptions As New ImageSaveOptions(ImageSaveFormat.Jpeg) With
            {
                .PageNumber = 0, ' Select the first PDF page.
                .Width = 1240 ' Set the image width and keep the aspect ratio.
            }

            ' Save a PDF document to a JPEG file.
            document.Save("Output.jpg", imageOptions)

        End Using
    End Sub

    Sub Example2()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load a PDF document.
        Using document = PdfDocument.Load("Input.pdf")

            Dim imageOptions = New ImageSaveOptions(ImageSaveFormat.Png)

            ' Create a ZIP file for storing PNG files.
            Using archiveStream = File.OpenWrite("Output.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create)
                    ' Iterate through the PDF pages.
                    For pageIndex As Integer = 0 To document.Pages.Count - 1

                        ' Add a white background color to the page.
                        Dim page = document.Pages(pageIndex)
                        Dim elements = page.Content.Elements
                        Dim background = elements.AddPath(elements.First)
                        background.AddRectangle(0, 0, page.Size.Width, page.Size.Height)
                        background.Format.Fill.IsApplied = True
                        background.Format.Fill.Color = PdfColor.FromRgb(1, 1, 1)

                        ' Create a ZIP entry for each page.
                        Dim entry = archive.CreateEntry($"Page {pageIndex + 1}.png")

                        ' Save each page as a PNG image to the ZIP entry.
                        Using imageStream = New MemoryStream()
                            Using entryStream = entry.Open()
                                imageOptions.PageNumber = pageIndex
                                document.Save(imageStream, imageOptions)

                                imageStream.Position = 0
                                imageStream.CopyTo(entryStream)
                            End Using
                        End Using
                    Next
                End Using
            End Using

        End Using
    End Sub

    Sub Example3()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load a PDF document.
        Using document = PdfDocument.Load("Input.pdf")

            ' Max integer value indicates that all document pages should be saved.
            Dim imageOptions As New ImageSaveOptions(ImageSaveFormat.Tiff) With
            {
                .PageCount = Integer.MaxValue
            }

            ' Save the TIFF file with multiple frames, each frame represents a single PDF page.
            document.Save("Output.tiff", imageOptions)
        End Using
    End Sub

End Module
