Imports GemBox.Pdf
Imports System.IO

Module Program

    Sub Main()

        Example1()
        Example2()
    End Sub

    Sub Example1()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' List of source file names.
        Dim fileNames = New String() _
        {
            "MergeFile01.pdf",
            "MergeFile02.pdf",
            "MergeFile03.pdf"
        }

        Using document = New PdfDocument()

            ' Merge multiple PDF files into single PDF by loading source documents
            ' and cloning all their pages to destination document.
            For Each fileName In fileNames
                Using source = PdfDocument.Load(fileName)
                    document.Pages.Kids.AddClone(source.Pages)
                End Using
            Next

            document.Save("Merge Files.pdf")

        End Using
    End Sub

    Sub Example2()
        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim files = Directory.EnumerateFiles("Merge Many Pdfs")

        Dim fileCounter As Integer = 0
        Dim chunkSize As Integer = 50

        Using document = New PdfDocument()

            ' Create output PDF file that will have large number of PDF files merged into it.
            document.Save("Merged Files.pdf")

            For Each file In files

                Using source = PdfDocument.Load(file)
                    document.Pages.Kids.AddClone(source.Pages)
                End Using

                fileCounter += 1
                If fileCounter Mod chunkSize = 0 Then

                    ' Save the new pages that were added after the document was last saved.
                    document.Save()

                    ' Clear previously parsed pages and thus free memory necessary for merging additional pages.
                    document.Unload()

                End If
            Next

            ' Save the last chunk of merged files.
            document.Save()
        End Using
    End Sub

End Module
