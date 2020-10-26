Imports System
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

            For Each page In document.Pages

                Dim contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator()
                While contentEnumerator.MoveNext()

                    If contentEnumerator.Current.ElementType = PdfContentElementType.Text Then

                        Dim textElement = CType(contentEnumerator.Current, PdfTextContent)

                        Dim text = textElement.ToString()
                        Dim font = textElement.Format.Text.Font
                        Dim color = textElement.Format.Fill.Color
                        Dim bounds = textElement.Bounds

                        contentEnumerator.Transform.Transform(bounds)

                        ' Read the text content element's additional information.
                        Console.WriteLine($"Unicode text: {text}")
                        Console.WriteLine($"Font name: {font.Face.Family.Name}")
                        Console.WriteLine($"Font size: {font.Size}")
                        Console.WriteLine($"Font style: {font.Face.Style}")
                        Console.WriteLine($"Font weight: {font.Face.Weight}")

                        Dim red, green, blue As Double
                        If color.TryGetRgb(red, green, blue) Then Console.WriteLine($"Color: Red={red}, Green={green}, Blue={blue}")

                        Console.WriteLine($"Bounds: Left={bounds.Left:0.00}, Bottom={bounds.Bottom:0.00}, Right={bounds.Right:0.00}, Top={bounds.Top:0.00}")
                        Console.WriteLine()
                    End If
                End While
            Next
        End Using
    End Sub

    Sub Example3()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim pageIndex = 0
        Dim areaLeft As Double = 400, areaRight As Double = 550, areaBottom As Double = 680, areaTop As Double = 720

        Using document = PdfDocument.Load("TextContent.pdf")

            ' Retrieve first page object.
            Dim page = document.Pages(pageIndex)

            ' Retrieve text content elements that are inside specified area on the first page.
            Dim contentEnumerator = page.Content.Elements.All(page.Transform).GetEnumerator()
            While contentEnumerator.MoveNext()

                If contentEnumerator.Current.ElementType = PdfContentElementType.Text Then

                    Dim textElement = CType(contentEnumerator.Current, PdfTextContent)

                    Dim bounds = textElement.Bounds

                    contentEnumerator.Transform.Transform(bounds)

                    If bounds.Left > areaLeft AndAlso bounds.Right < areaRight AndAlso
                        bounds.Bottom > areaBottom AndAlso bounds.Top < areaTop Then

                        Console.Write(textElement.ToString())
                    End If
                End If
            End While
        End Using
    End Sub
End Module
