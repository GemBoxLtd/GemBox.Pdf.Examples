Imports System.IO
Imports GemBox.Pdf

Module Module1

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim document As PdfDocument = New PdfDocument()

        Dim pathToResources As String = "Resources"

        ' List of source file names.
        Dim fileNames As String() = New String() {
            "MergeFile01.pdf",
            "MergeFile02.pdf",
            "MergeFile03.pdf"
        }

        For Each fileName As String In fileNames
            ' Load a source document from the specified path.
            Using source As PdfDocument = PdfDocument.Load(Path.Combine(pathToResources, fileName))
                ' Clone all pages from the source document and add them to the destination document.
                For Each page As PdfPage In source.Pages
                    document.Pages.AddClone(page)
                Next
            End Using
        Next

        document.Save("Merge Files.pdf")
        document.Close()

    End Sub

End Module