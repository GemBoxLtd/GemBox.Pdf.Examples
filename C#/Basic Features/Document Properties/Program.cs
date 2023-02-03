using System;
using System.Xml.Linq;
using GemBox.Pdf;
using GemBox.Pdf.Objects;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        Example1();
        Example2();
        Example3();
    }

    static void Example1()
    {
        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Get document properties.
            var info = document.Info;

            // Update document properties.
            info.Title = "My Title";
            info.Author = "My Author";
            info.Subject = "My Subject";
            info.Creator = "My Application";

            // Update producer and date information, and disable their overriding.
            info.Producer = "My Producer";
            info.CreationDate = new DateTime(2023, 1, 1, 12, 0, 0);
            info.ModificationDate = new DateTime(2023, 1, 1, 12, 0, 0);
            document.SaveOptions.UpdateProducerInformation = false;
            document.SaveOptions.UpdateDateInformation = false;

            document.Save("Document Properties.pdf");
        }
    }

    static void Example2()
    {
        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            // Get document properties dictionary.
            var infoDictionary = document.Info.GetOrAddDictionary();

            // Create or update custom properties.
            infoDictionary[PdfName.Create("Custom Name 1")] = PdfString.Create("My Value 1");
            infoDictionary[PdfName.Create("Custom Name 2")] = PdfString.Create("My Value 2");

            document.Save("Custom Properties.pdf");
        }
    }

    static void Example3()
    {
        using (var document = PdfDocument.Load("LoremIpsum.pdf"))
        {
            var metadata = document.Metadata;

            var xmp = XNamespace.Get("http://ns.adobe.com/xap/1.0/");
            var dc = XNamespace.Get("http://purl.org/dc/elements/1.1/");
            var rdf = XNamespace.Get("http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            var xml = XNamespace.Xml;

            metadata.Add(new XElement(xmp + "CreatorTool", "GemBox.Pdf for .NET"));
            metadata.Add(new XElement(xmp + "CreateDate", DateTime.Now));

            metadata.Add(new XElement(dc + "title",
                new XElement(rdf + "Alt",
                    new XElement(rdf + "li", new XAttribute(xml + "lang", "x-default"), "My Title"),
                    new XElement(rdf + "li", new XAttribute(xml + "lang", "en"), "My Title"),
                    new XElement(rdf + "li", new XAttribute(xml + "lang", "es"), "Mi T�tulo"),
                    new XElement(rdf + "li", new XAttribute(xml + "lang", "fr"), "Mon Titre"))));

            document.Save("XMP Metadata.pdf");
        }
    }
}
