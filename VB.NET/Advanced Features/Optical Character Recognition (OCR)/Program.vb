Imports System
Imports GemBox.Pdf
Imports GemBox.Pdf.Content
Imports GemBox.Pdf.Ocr

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Example1()
        Example2()

    End Sub

    Sub Example1()
        Using document = OcrReader.Read("BookPage.jpg")

            Dim page = document.Pages(0)
            Dim contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator()
            While contentEnumerator.MoveNext()

                If contentEnumerator.Current.ElementType = PdfContentElementType.Text Then

                    Dim textElement = CType(contentEnumerator.Current, PdfTextContent)
                    Console.WriteLine(textElement.ToString())
                End If
            End While

        End Using
    End Sub

    Sub Example2()
        ' TesseractDataPath specifies the directory which contains language data.
        ' You can download the language data files from: https://www.gemboxsoftware.com/pdf/docs/ocr.html#language-data
        Dim readOptions As New OcrReadOptions() With {.TesseractDataPath = "languagedata"}

        ' The language of the text.
        readOptions.Languages.Add(OcrLanguages.German)

        Using document = OcrReader.Read("GermanDocument.pdf", readOptions)
            document.Save("GermanDocumentEditable.pdf")
        End Using
    End Sub

End Module