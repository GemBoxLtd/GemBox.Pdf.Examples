Imports System
Imports System.Xml.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Objects

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Example1()
        Example2()
        Example3()
    End Sub

    Sub Example1()
        Using document = PdfDocument.Load("LoremIpsum.pdf")

            ' Get document properties.
            Dim info = document.Info

            ' Update document properties.
            info.Title = "My Title"
            info.Author = "My Author"
            info.Subject = "My Subject"
            info.Creator = "My Application"

            ' Update producer and date information, and disable their overriding.
            info.Producer = "My Producer"
            info.CreationDate = New DateTime(2023, 1, 1, 12, 0, 0)
            info.ModificationDate = New DateTime(2023, 1, 1, 12, 0, 0)
            document.SaveOptions.UpdateProducerInformation = False
            document.SaveOptions.UpdateDateInformation = False

            document.Save("Document Properties.pdf")
        End Using
    End Sub

    Sub Example2()
        Using document = PdfDocument.Load("LoremIpsum.pdf")

            ' Get document properties dictionary.
            Dim infoDictionary = document.Info.GetOrAddDictionary()

            ' Create or update custom properties.
            infoDictionary(PdfName.Create("Custom Name 1")) = PdfString.Create("My Value 1")
            infoDictionary(PdfName.Create("Custom Name 2")) = PdfString.Create("My Value 2")

            document.Save("Custom Properties.pdf")
        End Using
    End Sub

    Sub Example3()
        Using document = PdfDocument.Load("LoremIpsum.pdf")

            Dim metadata = document.Metadata

            Dim xmp = XNamespace.Get("http://ns.adobe.com/xap/1.0/")
            Dim dc = XNamespace.Get("http://purl.org/dc/elements/1.1/")
            Dim rdf = XNamespace.Get("http://www.w3.org/1999/02/22-rdf-syntax-ns#")
            Dim xml = XNamespace.Xml

            metadata.Add(New XElement(xmp + "CreatorTool", "GemBox.Pdf for .NET"))
            metadata.Add(New XElement(xmp + "CreateDate", DateTime.Now))

            metadata.Add(New XElement(dc + "title",
                New XElement(rdf + "Alt",
                    New XElement(rdf + "li", New XAttribute(xml + "lang", "x-default"), "My Title"),
                    New XElement(rdf + "li", New XAttribute(xml + "lang", "en"), "My Title"),
                    New XElement(rdf + "li", New XAttribute(xml + "lang", "es"), "Mi T�tulo"),
                    New XElement(rdf + "li", New XAttribute(xml + "lang", "fr"), "Mon Titre"))))

            document.Save("XMP Metadata.pdf")
        End Using
    End Sub
End Module