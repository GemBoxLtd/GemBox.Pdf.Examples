using System;
using System.Globalization;
using GemBox.Pdf;
using GemBox.Pdf.Objects;
using GemBox.Pdf.Text;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Get document's trailer dictionary.
            var trailer = document.GetDictionary();
            // Get document catalog dictionary from the trailer.
            var catalog = (PdfDictionary)((PdfIndirectObject)trailer[PdfName.Create("Root")]).Value;

            // Either retrieve "PieceInfo" entry value from document catalog or create a page-piece dictionary and set it to document catalog under "PieceInfo" entry.
            PdfDictionary pieceInfo;
            var pieceInfoKey = PdfName.Create("PieceInfo");
            var pieceInfoValue = catalog[pieceInfoKey];
            switch (pieceInfoValue.ObjectType)
            {
                case PdfBasicObjectType.Dictionary:
                    pieceInfo = (PdfDictionary)pieceInfoValue;
                    break;
                case PdfBasicObjectType.IndirectObject:
                    pieceInfo = (PdfDictionary)((PdfIndirectObject)pieceInfoValue).Value;
                    break;
                case PdfBasicObjectType.Null:
                    pieceInfo = PdfDictionary.Create();
                    catalog[pieceInfoKey] = PdfIndirectObject.Create(pieceInfo);
                    break;
                default:
                    throw new InvalidOperationException("PieceInfo entry must be dictionary.");
            }

            // Create page-piece data dictionary for "GemBox.Pdf" conforming product and set it to page-piece dictionary.
            var data = PdfDictionary.Create();
            pieceInfo[PdfName.Create("GemBox.Pdf")] = data;

            // Create a private data dictionary that will hold private data that "GemBox.Pdf" conforming product understands.
            var privateData = PdfDictionary.Create();
            data[PdfName.Create("Data")] = privateData;

            // Set "Title" and "Version" entries to private data.
            privateData[PdfName.Create("Title")] = PdfString.Create(ComponentInfo.Title);
            privateData[PdfName.Create("Version")] = PdfString.Create(ComponentInfo.Version);

            // Specify date of the last modification of "GemBox.Pdf" private data (required by PDF specification).
            data[PdfName.Create("LastModified")] = PdfString.Create(DateTimeOffset.Now);

            document.Save("Basic Objects.pdf");
        }
    }
}
