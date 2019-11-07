using GemBox.Pdf;

class Program
{
    static void Main(string[] args)
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (PdfDocument document = PdfDocument.Load("Reading.pdf"))
            // In order to achieve the conversion of a loaded PDF file to image,
            // we just need to save a PdfDocument object to desired image format.
            document.Save("Convert.png");
    }
}
