Imports System
Imports System.Globalization
Imports GemBox.Pdf
Imports GemBox.Pdf.Objects
Imports GemBox.Pdf.Text

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("LoremIpsum.pdf")

            ' Get document's trailer dictionary.
            Dim trailer = document.GetDictionary()
            ' Get document catalog dictionary from the trailer.
            Dim catalog = CType((CType(trailer(PdfName.Create("Root")), PdfIndirectObject)).Value, PdfDictionary)

            ' Either retrieve "PieceInfo" entry value from document catalog or create a page-piece dictionary and set it to document catalog under "PieceInfo" entry.
            Dim pieceInfo As PdfDictionary
            Dim pieceInfoKey = PdfName.Create("PieceInfo")
            Dim pieceInfoValue = catalog(pieceInfoKey)
            Select Case pieceInfoValue.ObjectType

                Case PdfBasicObjectType.Dictionary
                    pieceInfo = CType(pieceInfoValue, PdfDictionary)

                Case PdfBasicObjectType.IndirectObject
                    pieceInfo = CType((CType(pieceInfoValue, PdfIndirectObject)).Value, PdfDictionary)

                Case PdfBasicObjectType.Null
                    pieceInfo = PdfDictionary.Create()
                    catalog(pieceInfoKey) = PdfIndirectObject.Create(pieceInfo)

                Case Else
                    Throw New InvalidOperationException("PieceInfo entry must be dictionary.")
            End Select

            ' Create page-piece data dictionary for "GemBox.Pdf" conforming product and set it to page-piece dictionary.
            Dim data = PdfDictionary.Create()
            pieceInfo(PdfName.Create("GemBox.Pdf")) = data

            ' Create a private data dictionary that will hold private data that "GemBox.Pdf" conforming product understands.
            Dim privateData = PdfDictionary.Create()
            data(PdfName.Create("Data")) = privateData

            ' Set "Title" and "Version" entries to private data.
            privateData(PdfName.Create("Title")) = PdfString.Create(ComponentInfo.Title)
            privateData(PdfName.Create("Version")) = PdfString.Create(ComponentInfo.Version)

            ' Specify date of the last modification of "GemBox.Pdf" private data (required by PDF specification).
            data(PdfName.Create("LastModified")) = PdfString.Create(DateTimeOffset.Now)

            document.Save("Basic Objects.pdf")
        End Using
    End Sub
End Module