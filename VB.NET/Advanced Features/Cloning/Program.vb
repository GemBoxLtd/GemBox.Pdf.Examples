Imports GemBox.Pdf
Imports System

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Invoice.pdf")

            Dim pageCount As Integer = 5

            ' Load a source document.
            Using source = PdfDocument.Load("LoremIpsum.pdf")

                ' Get the number of pages to clone.
                Dim cloneCount = Math.Min(pageCount, source.Pages.Count)

                ' Clone the requested number of pages from the source document 
                ' and add them to the destination document.
                For i As Integer = 0 To cloneCount - 1
                    document.Pages.AddClone(source.Pages(i))
                Next
            End Using

            document.Save("Cloning.pdf")
        End Using
    End Sub
End Module
