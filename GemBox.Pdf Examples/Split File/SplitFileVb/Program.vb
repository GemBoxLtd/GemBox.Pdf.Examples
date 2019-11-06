Imports System.IO
Imports System.IO.Compression
Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim fileNameWithoutExt = Path.GetFileNameWithoutExtension("LoremIpsum.pdf")

        ' Open source PDF file and create a destination ZIP file.
        Using source = PdfDocument.Load("LoremIpsum.pdf")
            Using archiveStream = File.OpenWrite(fileNameWithoutExt & ".zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen:=True)
                    For index As Integer = 0 To source.Pages.Count - 1

                        ' Create new ZIP entry for each source document page.
                        Dim entry = archive.CreateEntry(fileNameWithoutExt & (index + 1) & ".pdf")

                        ' Open ZIP entry stream.
                        Using entryStream = entry.Open()
                            ' Create destination document.
                            Using destination = New PdfDocument()

                                ' Clone source document page to destination document.
                                destination.Pages.AddClone(source.Pages(index))

                                ' Save destination document to ZIP entry stream.
                                destination.Save(entryStream)
                            End Using
                        End Using
                    Next
                End Using
            End Using
        End Using
    End Sub
End Module
