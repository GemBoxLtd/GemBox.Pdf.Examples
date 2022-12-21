Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Example1()
        Example2()
        Example3()
        Example4()

    End Sub

    Sub Example1()
        ' Open a source PDF file and create a destination ZIP file.
        Using source = PdfDocument.Load("Chapters.pdf")
            Using archiveStream = File.OpenWrite("Output.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create)

                    ' Iterate through the PDF pages.
                    For pageIndex As Integer = 0 To source.Pages.Count - 1

                        ' Create a ZIP entry for each source document page.
                        Dim entry = archive.CreateEntry($"Page {pageIndex + 1}.pdf")

                        ' Save each page as a separate destination document to the ZIP entry.
                        Using entryStream = entry.Open()
                            Using destination = New PdfDocument()
                                destination.Pages.AddClone(source.Pages(pageIndex))
                                destination.Save(entryStream)
                            End Using
                        End Using

                    Next

                End Using
            End Using
        End Using
    End Sub

    Sub Example2()
        ' List of page numbers used for splitting the PDF document.
        Dim pageRanges = {
            New With {.FirstPageIndex = 0, .LastPageIndex = 2},
            New With {.FirstPageIndex = 3, .LastPageIndex = 3},
            New With {.FirstPageIndex = 4, .LastPageIndex = 6}
        }

        ' Open a source PDF file and create a destination ZIP file.
        Using source = PdfDocument.Load("Chapters.pdf")
            Using archiveStream = File.OpenWrite("OutputRanges.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create)

                    ' Iterate through page ranges.
                    For Each pageRange In pageRanges
                        Dim pageIndex As Integer = pageRange.FirstPageIndex
                        Dim pageCount As Integer = Math.Min(pageRange.LastPageIndex + 1, source.Pages.Count)

                        Dim entry = archive.CreateEntry($"Pages {pageIndex + 1}-{pageCount}.pdf")
                        Using entryStream = entry.Open()
                            Using destination = New PdfDocument()

                                ' Add range of source pages to destination document.
                                While pageIndex < pageCount
                                    destination.Pages.AddClone(source.Pages(pageIndex))
                                    pageIndex = pageIndex + 1
                                End While

                                ' Save destination document to the ZIP entry.
                                destination.Save(entryStream)
                            End Using
                        End Using
                    Next

                End Using
            End Using
        End Using
    End Sub

    Sub Example3()
        ' Open a source PDF file and create a destination ZIP file.
        Using source = PdfDocument.Load("Chapters.pdf")
            Using archiveStream = File.OpenWrite("Output.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create)

                    Dim pageIndexes As Dictionary(Of PdfPage, Integer) = source.Pages _
                        .Select(Function(page, index) New With {page, index}) _
                        .ToDictionary(Function(item) item.page, Function(item) item.index)

                    ' Iterate through document outlines.
                    Dim outlines = source.Outlines
                    For index As Integer = 0 To outlines.Count - 1

                        Dim currentOutline = outlines(index)
                        Dim nextOutline = If(index + 1 < outlines.Count, outlines(index + 1), Nothing)

                        Dim pageIndex As Integer = pageIndexes(currentOutline.Destination.Page)
                        Dim pageCount As Integer = If(nextOutline Is Nothing, source.Pages.Count, pageIndexes(nextOutline.Destination.Page))

                        ' Save each page as a separate destination document to the ZIP entry.
                        Dim entry = archive.CreateEntry($"{currentOutline.Title}.pdf")
                        Using entryStream = entry.Open()
                            Using destination = New PdfDocument()

                                ' Add source pages from current bookmark till next bookmark to destination document.
                                While pageIndex < pageCount
                                    destination.Pages.AddClone(source.Pages(pageIndex))
                                    pageIndex = pageIndex + 1
                                End While

                                ' Save destination document to the ZIP entry.
                                destination.Save(entryStream)
                            End Using
                        End Using

                    Next

                End Using
            End Using
        End Using
    End Sub

    Sub Example4()
        Using source = PdfDocument.Load("lorem-ipsum-1000-pages.pdf")

            Dim chunkSize As Integer = 220

            Dim pageIndex As Integer = 0
            Dim pageCount As Integer = source.Pages.Count
            While pageIndex < pageCount

                ' Split large PDF file into multiple PDF files of specified chunk size.
                Using destination = New PdfDocument()
                    Dim chunkCount As Integer = Math.Min(chunkSize + pageIndex, pageCount)
                    Dim chunkName As String = $"Pages {pageIndex + 1}-{chunkCount}.pdf"

                    While pageIndex < chunkCount
                        destination.Pages.AddClone(source.Pages(Math.Min(System.Threading.Interlocked.Increment(pageIndex), pageIndex - 1)))
                    End While

                    destination.Save(Path.Combine("Split Large Pdf", chunkName))
                End Using

                ' Clear previously parsed pages and thus free memory necessary for reading additional pages.
                source.Unload()
            End While
        End Using
    End Sub

End Module
