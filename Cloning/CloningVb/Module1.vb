Imports System
Imports System.IO
Imports GemBox.Pdf

Module Module1

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim document As PdfDocument = PdfDocument.Load("Invoice.pdf")

        Dim pageCount As Integer = 5
        Dim pathToResources As String = "Resources"

        ' Load a source document.
        Using source As PdfDocument = PdfDocument.Load(Path.Combine(pathToResources, "Reading.pdf"))
            ' Get the number of pages to clone.
            Dim cloneCount = Math.Min(pageCount, source.Pages.Count)

            ' Clone the requested number of pages from the source document 
            ' and add them to the destination document.
            For i As Integer = 0 To cloneCount - 1
                document.Pages.AddClone(source.Pages(i))
            Next
        End Using
        document.SaveOptions.CloseOutput = True
        document.Save("Cloning.pdf")

    End Sub

End Module