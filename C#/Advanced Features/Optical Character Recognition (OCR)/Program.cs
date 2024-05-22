using GemBox.Pdf;
using GemBox.Pdf.Content;
using GemBox.Pdf.Ocr;
using System;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        Example1();
        Example2();
    }

    static void Example1()
    {
        using (PdfDocument document = OcrReader.Read("BookPage.jpg"))
        {
            var page = document.Pages[0];
            var contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator();

            while (contentEnumerator.MoveNext())
            {
                if (contentEnumerator.Current.ElementType == PdfContentElementType.Text)
                {
                    var textElement = (PdfTextContent)contentEnumerator.Current;
                    Console.WriteLine(textElement.ToString());
                }
            }
        }
    }

    static void Example2()
    {
        // TesseractDataPath specifies the directory which contains language data.
        // You can download the language data files from: https://www.gemboxsoftware.com/pdf/docs/ocr.html#language-data
        var readOptions = new OcrReadOptions() { TesseractDataPath = "languagedata" };

        // The language of the text.
        readOptions.Languages.Add(OcrLanguages.German);

        using (PdfDocument document = OcrReader.Read("GermanDocument.pdf", readOptions))
        {
            document.Save("GermanDocumentEditable.pdf");
        }
    }
}
