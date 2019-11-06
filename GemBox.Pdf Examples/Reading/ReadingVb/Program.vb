Imports System
Imports System.Drawing
Imports System.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        Example1()

        Example2()

        Example3()
    End Sub

    Sub Example1()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Iterate through PDF pages and extract each page's Unicode text content.
        Using document = PdfDocument.Load("Reading.pdf")
            For Each page In document.Pages
                Console.WriteLine(page.Content.ToString())
            Next
        End Using
    End Sub

    Sub Example2()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Iterate through all PDF pages and through each page's content elements,
        ' and retrieve only the text content elements.
        Using document = PdfDocument.Load("TextContent.pdf")
            For Each textElement In document.Pages _
                .SelectMany(Function(page) page.Content.Elements.All()) _
                .Where(Function(element) element.ElementType = PdfContentElementType.Text) _
                .Cast(Of PdfTextContent)()

                Dim text = textElement.ToString()
                Dim font = textElement.Format.Text.Font
                Dim color = textElement.Format.Fill.Color
                Dim location = textElement.Location

                ' Read the text content element's additional information.
                Console.WriteLine($"Unicode text: {text}")
                Console.WriteLine($"Font name: {font.Face.Family.Name}")
                Console.WriteLine($"Font size: {font.Size}")
                Console.WriteLine($"Font style: {font.Face.Style}")
                Console.WriteLine($"Font weight: {font.Face.Weight}")

                Dim red, green, blue As Double
                If color.TryGetRgb(red, green, blue) Then Console.WriteLine($"Color: Red={red}, Green={green}, Blue={blue}")

                Console.WriteLine($"Location: X={location.X:0.00}, Y={location.Y:0.00}")
                Console.WriteLine()
            Next
        End Using
    End Sub

    Sub Example3()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim pageIndex = 0
        Dim area = New Rectangle(400, 690, 150, 30)

        Using document = PdfDocument.Load("TextContent.pdf")

            ' Retrieve first page object.
            Dim page = document.Pages(pageIndex)

            ' Retrieve text content elements that are inside specified area on the first page.
            For Each textElement In page.Content.Elements.All() _
                .Where(Function(element) element.ElementType = PdfContentElementType.Text) _
                .Cast(Of PdfTextContent)()

                Dim location = textElement.Location
                If location.X > area.X AndAlso location.X < area.X + area.Width AndAlso
                   location.Y > area.Y AndAlso location.Y < area.Y + area.Height Then
                    Console.Write(textElement.ToString())
                End If
            Next
        End Using
    End Sub
End Module
