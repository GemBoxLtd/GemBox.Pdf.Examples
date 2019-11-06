Imports GemBox.Pdf

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document As PdfDocument = PdfDocument.Load("Reading.pdf")
            ' In order to achieve the conversion of a loaded PDF file to image,
            ' we just need to save a PdfDocument object to desired output file format.
            document.Save("Convert.png")
        End Using

    End Sub

End Module