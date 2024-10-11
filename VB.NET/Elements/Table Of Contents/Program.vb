Imports GemBox.Document
Imports GemBox.Pdf
Imports GemBox.Pdf.Annotations
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Module Program

    Sub Main()

        ' If using the Professional version, put your GemBox.Pdf serial key below.
        GemBox.Pdf.ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' If using the Professional version, put your GemBox.Document serial key below.
        GemBox.Document.ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim files = New String() _
        {
            "MergeFile01.pdf",
            "MergeFile02.pdf",
            "MergeFile03.pdf"
        }

        Dim tocEntries = New List(Of (Title As String, PagesCount As Integer))()
        Using document As New PdfDocument()
            ' Merge PDF files.
            For Each file In files
                Using source = PdfDocument.Load(file)
                    document.Pages.Kids.AddClone(source.Pages)
                    tocEntries.Add((Path.GetFileNameWithoutExtension(file), source.Pages.Count))
                End Using
            Next

            Dim pagesCount As Integer
            Dim tocPagesCount As Integer

            ' Create PDF with Table of Contents.
            Using tocDocument = PdfDocument.Load(CreatePdfWithToc(tocEntries))

                pagesCount = tocDocument.Pages.Count
                tocPagesCount = pagesCount - tocEntries.Sum(Function(entry) entry.PagesCount)

                ' Remove empty (placeholder) pages.
                For i = pagesCount - 1 To tocPagesCount Step -1
                    tocDocument.Pages.RemoveAt(i)
                Next

                ' Insert TOC pages.
                document.Pages.Kids.InsertClone(0, tocDocument.Pages)
            End Using

            Dim entryIndex As Integer = 0
            Dim entryPageIndex As Integer = tocPagesCount

            ' Update TOC links and outlines so that they point to adequate pages instead of placeholder pages.
            For i = 0 To tocPagesCount - 1
                For Each annotation In document.Pages(i).Annotations.OfType(Of PdfLinkAnnotation)()
                    Dim entryPage = document.Pages(entryPageIndex)
                    annotation.SetDestination(entryPage, PdfDestinationViewType.FitPage)
                    document.Outlines(entryIndex).SetDestination(entryPage, PdfDestinationViewType.FitPage)

                    entryPageIndex += tocEntries(entryIndex).PagesCount
                    entryIndex += 1
                Next
            Next

            document.Save("Merge Files With Toc.pdf")
        End Using
    End Sub

    Function CreatePdfWithToc(tocEntries As List(Of (Title As String, PagesCount As Integer))) As Stream
        ' Create new document.
        Dim document As New DocumentModel()
        Dim section As New Section(document)
        document.Sections.Add(section)

        ' Add Table of Content.
        Dim toc As New TableOfEntries(document, FieldType.TOC)
        section.Blocks.Add(toc)

        ' Create heading style.
        Dim heading1Style = CType(document.Styles.GetOrAdd(StyleTemplateType.Heading1), ParagraphStyle)
        heading1Style.ParagraphFormat.PageBreakBefore = True

        ' Add heading paragraphs and empty (placeholder) pages.
        For Each tocEntry In tocEntries
            section.Blocks.Add(
                New Paragraph(document, tocEntry.Title) With
                {.ParagraphFormat = New ParagraphFormat() With {.Style = heading1Style}})

            For i As Integer = 0 To tocEntry.PagesCount - 1
                section.Blocks.Add(
                    New Paragraph(document,
                        New SpecialCharacter(document, SpecialCharacterType.PageBreak)))
            Next
        Next

        ' Remove last extra-added empty page.
        section.Blocks.RemoveAt(section.Blocks.Count - 1)

        ' When updating TOC element, an entry is created for each paragraph that has heading style.
        ' The entries have the correct page numbers because of the added placeholder pages.
        toc.Update()

        ' Save document as PDF.
        Dim pdfStream As New MemoryStream()
        document.Save(pdfStream, New GemBox.Document.PdfSaveOptions())
        Return pdfStream
    End Function

End Module
