Imports System
Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("LoremIpsum.pdf")

            ' Remove all bookmarks.
            document.Outlines.Clear()

            ' Get the number of pages.
            Dim numberOfPages As Integer = document.Pages.Count

            For i As Integer = 0 To numberOfPages - 1 Step 10

                ' Add a new outline item (bookmark) at the end of the document outline collection.
                Dim bookmark = document.Outlines.AddLast(String.Format("PAGES {0}-{1}", i + 1, Math.Min(i + 10, numberOfPages)))

                ' Set the explicit destination on the New outline item (bookmark).
                bookmark.SetDestination(document.Pages(i), PdfDestinationViewType.FitRectangle, 0, 0, 100, 100)

                For j As Integer = 0 To Math.Min(10, numberOfPages - i) - 1
                    ' Add a new outline item (bookmark) at the end of parent outline item (bookmark) and set the explicit destination.
                    bookmark.Outlines.AddLast(String.Format("PAGE {0}", i + j + 1)).SetDestination(document.Pages(i + j), PdfDestinationViewType.FitPage)
                Next
            Next

            document.PageMode = PdfPageMode.UseOutlines

            document.Save("Outlines.pdf")
        End Using
    End Sub
End Module