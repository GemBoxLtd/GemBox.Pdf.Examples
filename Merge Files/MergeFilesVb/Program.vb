Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' List of source file names.
        Dim fileNames = New String() _
        {
            "MergeFile01.pdf",
            "MergeFile02.pdf",
            "MergeFile03.pdf"
        }

        Using document = New PdfDocument()

            For Each fileName In fileNames
                ' Load a source document from the specified path.
                Using source = PdfDocument.Load(fileName)
                    ' Clone all pages from the source document and add them to the destination document.
                    For Each page In source.Pages
                        document.Pages.AddClone(page)
                    Next
                End Using
            Next

            document.Save("Merge Files.pdf")
        End Using
    End Sub
End Module