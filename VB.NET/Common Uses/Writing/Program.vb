Imports System.IO
Imports GemBox.Pdf
Imports GemBox.Pdf.Content

Module Program

    Sub Main()

        Example1()

        Example2()

        Example3()

        Example4()

        Example5()

    End Sub

    Sub Example1()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            ' Add a page.
            Dim page = document.Pages.Add()

            Using formattedText = New PdfFormattedText()

                ' Set font family and size.
                ' All text appended next uses the specified font family and size.
                formattedText.FontFamily = New PdfFontFamily("Times New Roman")
                formattedText.FontSize = 24

                formattedText.AppendLine("Hello World")

                ' Reset font family and size for all text appended next.
                formattedText.FontFamily = New PdfFontFamily("Calibri")
                formattedText.FontSize = 12

                formattedText.AppendLine(" with ")

                ' Set font style and color for all text appended next.
                formattedText.FontStyle = PdfFontStyle.Italic
                formattedText.Color = PdfColor.FromRgb(1, 0, 0)

                formattedText.Append("GemBox.Pdf")

                ' Reset font style and color for all text appended next.
                formattedText.FontStyle = PdfFontStyle.Normal
                formattedText.Color = PdfColor.FromRgb(0, 0, 0)

                formattedText.Append(" component!")

                ' Set the location of the bottom-left corner of the text.
                ' We want top-left corner of the text to be at location (100, 100)
                ' from the top-left corner of the page.
                ' NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                ' and the positive y axis extends vertically upward.
                Dim x As Double = 100, y As Double = page.CropBox.Top - 100 - formattedText.Height

                ' Draw text to the page.
                page.Content.DrawText(formattedText, New PdfPoint(x, y))
            End Using

            document.Save("Writing.pdf")
        End Using
    End Sub

    Sub Example2()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            Dim margin As Double = 10

            Using formattedText = New PdfFormattedText()

                formattedText.TextAlignment = PdfTextAlignment.Left
                formattedText.MaxTextWidth = 100
                formattedText.Append("This text is left aligned, ").
                    Append("placed in the top-left corner of the page and ").
                    Append("its width should not exceed 100 points.")
                page.Content.DrawText(formattedText,
                    New PdfPoint(margin,
                        page.CropBox.Top - margin - formattedText.Height))

                formattedText.Clear()

                formattedText.TextAlignment = PdfTextAlignment.Center
                formattedText.MaxTextWidth = 200
                formattedText.Append("This text is center aligned, ").
                    Append("placed in the top-center part of the page ").
                    Append("and its width should not exceed 200 points.")
                page.Content.DrawText(formattedText,
                    New PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2,
                        page.CropBox.Top - margin - formattedText.Height))

                formattedText.Clear()

                formattedText.TextAlignment = PdfTextAlignment.Right
                formattedText.MaxTextWidth = 100
                formattedText.Append("This text is right aligned, ").
                    Append("placed in the top-right corner of the page ").
                    Append("and its width should not exceed 100 points.")
                page.Content.DrawText(formattedText,
                    New PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                        page.CropBox.Top - margin - formattedText.Height))

                formattedText.Clear()

                formattedText.TextAlignment = PdfTextAlignment.Left
                formattedText.MaxTextWidth = 100
                formattedText.Append("This text is left aligned, ").
                    Append("placed in the bottom-left corner of the page and ").
                    Append("its width should not exceed 100 points.")
                page.Content.DrawText(formattedText,
                    New PdfPoint(margin,
                        margin))

                formattedText.Clear()

                formattedText.TextAlignment = PdfTextAlignment.Center
                formattedText.MaxTextWidth = 200
                formattedText.Append("This text is center aligned, ").
                    Append("placed in the bottom-center part of the page and ").
                    Append("its width should not exceed 200 points.")
                page.Content.DrawText(formattedText,
                    New PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2,
                        margin))

                formattedText.Clear()

                formattedText.TextAlignment = PdfTextAlignment.Right
                formattedText.MaxTextWidth = 100
                formattedText.Append("This text is right aligned, ").
                    Append("placed in the bottom-right corner of the page and ").
                    Append("its width should not exceed 100 points.")
                page.Content.DrawText(formattedText,
                    New PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                        margin))

                formattedText.Clear()

                formattedText.TextAlignment = PdfTextAlignment.Justify
                formattedText.MaxTextWidths = New Double() {200, 150, 100}
                formattedText.Append("This text has justified alignment, ").
                    Append("is placed in the center of the page and ").
                    Append("its first line should not exceed 200 points, ").
                    Append("its second line should not exceed 150 points and ").
                    Append("its third and all other lines should not exceed 100 points.")
                ' Center the text based on the width of the most lines, which is formattedText.MaxTextWidths(2).
                page.Content.DrawText(formattedText,
                    New PdfPoint((page.CropBox.Width - formattedText.MaxTextWidths(2)) / 2,
                        (page.CropBox.Height - formattedText.Height) / 2))
            End Using

            document.Save("Alignment and Positioning.pdf")
        End Using
    End Sub

    Sub Example3()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            Using formattedText = New PdfFormattedText()

                Dim text = "Rotated by 30 degrees around origin."
                formattedText.Opacity = 0.2
                formattedText.Append(text)
                Dim origin = New PdfPoint(50, 650)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.Append(text)
                Dim transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Rotate(30)
                page.Content.DrawText(formattedText, transform)

                formattedText.Clear()

                text = "Rotated by 30 degrees around center."
                formattedText.Opacity = 0.2
                formattedText.Append(text)
                origin = New PdfPoint(300, 650)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.Append(text)
                transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2)
                page.Content.DrawText(formattedText, transform)

                formattedText.Clear()

                text = "Scaled horizontally by 0.5 around origin."
                formattedText.Opacity = 0.2
                formattedText.Append(text)
                origin = New PdfPoint(50, 500)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.Append(text)
                transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Scale(0.5, 1)
                page.Content.DrawText(formattedText, transform)

                formattedText.Clear()

                text = "Scaled horizontally by 0.5 around center."
                formattedText.Opacity = 0.2
                formattedText.Append(text)
                origin = New PdfPoint(300, 500)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.Append(text)
                transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Scale(0.5, 1, formattedText.Width / 2, formattedText.Height / 2)
                page.Content.DrawText(formattedText, transform)

                formattedText.Clear()

                text = "Scaled vertically by 2 around origin."
                formattedText.Opacity = 0.2
                formattedText.Append(text)
                origin = New PdfPoint(50, 400)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.Append(text)
                transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Scale(1, 2)
                page.Content.DrawText(formattedText, transform)

                formattedText.Clear()

                text = "Scaled vertically by 2 around center."
                formattedText.Opacity = 0.2
                formattedText.Append(text)
                origin = New PdfPoint(300, 400)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.Append(text)
                transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Scale(1, 2, formattedText.Width / 2, formattedText.Height / 2)
                page.Content.DrawText(formattedText, transform)

                formattedText.Clear()

                text = "Rotated by 30 degrees around origin and "
                Dim text2 = "scaled horizontally by 0.5 and "
                Dim text3 = "vertically by 2 around origin."
                formattedText.Opacity = 0.2
                formattedText.AppendLine(text).AppendLine(text2).Append(text3)
                origin = New PdfPoint(50, 200)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.AppendLine(text).AppendLine(text2).Append(text3)
                transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Rotate(30)
                transform.Scale(0.5, 2)
                page.Content.DrawText(formattedText, transform)

                formattedText.Clear()

                text = "Rotated by 30 degrees around center and "
                text2 = "scaled horizontally by 0.5 and "
                text3 = "vertically by 2 around center."
                formattedText.Opacity = 0.2
                formattedText.AppendLine(text).AppendLine(text2).Append(text3)
                origin = New PdfPoint(300, 200)
                page.Content.DrawText(formattedText, origin)
                formattedText.Clear()
                formattedText.Opacity = 1
                formattedText.AppendLine(text).AppendLine(text2).Append(text3)
                transform = PdfMatrix.Identity
                transform.Translate(origin.X, origin.Y)
                transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2)
                transform.Scale(0.5, 2, formattedText.Width / 2, formattedText.Height / 2)
                page.Content.DrawText(formattedText, transform)
            End Using

            document.Save("Transformations.pdf")
        End Using
    End Sub

    Sub Example4()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            Using formattedText = New PdfFormattedText()

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)

                formattedText.AppendLine("An example of a fully vocalised (vowelised or vowelled) Arabic ").
                    Append("from the Basmala: ")

                formattedText.Language = New PdfLanguage("ar-SA")
                formattedText.Font = New PdfFont("Arial", 24)
                formattedText.Append("بِسْمِ ٱللَّٰهِ ٱلرَّحْمَٰنِ ٱلرَّحِيمِ")

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)
                formattedText.AppendLine(", which means: ").
                    Append("In the name of God, the All-Merciful, the Especially-Merciful.")

                page.Content.DrawText(formattedText, New PdfPoint(50, 750))

                formattedText.Clear()

                formattedText.Append("An example of Hebrew: ")

                formattedText.Language = New PdfLanguage("he-IL")
                formattedText.Font = New PdfFont("Arial", 24)
                formattedText.Append("מה קורה")

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)
                formattedText.AppendLine(", which means: What's going on, ").
                    Append("and ")

                formattedText.Language = New PdfLanguage("he-IL")
                formattedText.Font = New PdfFont("Arial", 24)
                formattedText.Append("תודה לכולם")

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)
                formattedText.Append(", which means: Thank you all.")

                page.Content.DrawText(formattedText, New PdfPoint(50, 650))

                formattedText.Clear()

                formattedText.LineHeight = 50

                formattedText.Append("An example of Thai: ")
                formattedText.Language = New PdfLanguage("th-TH")
                formattedText.Font = New PdfFont("Leelawadee UI", 16)
                formattedText.AppendLine("ภัำ")

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)
                formattedText.Append("An example of Tamil: ")
                formattedText.Language = New PdfLanguage("ta-IN")
                formattedText.Font = New PdfFont("Nirmala UI", 16)
                formattedText.AppendLine("போது")

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)
                formattedText.Append("An example of Bengali: ")
                formattedText.Language = New PdfLanguage("be-IN")
                formattedText.Font = New PdfFont("Nirmala UI", 16)
                formattedText.AppendLine("আবেদনকারীর মাতার পিতার বর্তমান স্থায়ী ঠিকানা নমিনি নাম")

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)
                formattedText.Append("An example of Gujarati: ")
                formattedText.Language = New PdfLanguage("gu-IN")
                formattedText.Font = New PdfFont("Nirmala UI", 16)
                formattedText.AppendLine("કાર્બન કેમેસ્ટ્રી")

                formattedText.Language = New PdfLanguage("en-US")
                formattedText.Font = New PdfFont("Calibri", 12)
                formattedText.Append("An example of Osage: ")
                formattedText.Language = New PdfLanguage("osa")
                formattedText.Font = New PdfFont("Gadugi", 16)
                formattedText.Append("𐓏𐓘𐓻𐓘𐓻𐓟 𐒻𐓟")

                page.Content.DrawText(formattedText, New PdfPoint(50, 350))
            End Using

            document.Save("Complex scripts.pdf")
        End Using
    End Sub

    Sub Example5()
        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = New PdfDocument()

            Dim page = document.Pages.Add()

            Dim margins As Double = 50
            Dim maxWidth As Double = page.CropBox.Width - margins * 2
            Dim halfWidth As Double = maxWidth / 2

            Dim maxHeight As Double = page.CropBox.Height - margins * 2
            Dim heightOffset As Double = maxHeight + margins

            Using formattedText = New PdfFormattedText()

                For Each line In File.ReadAllLines("LoremIpsum.txt")
                    formattedText.AppendLine(line)
                Next

                Dim y As Double = 0
                Dim lineIndex As Integer = 0, charIndex As Integer = 0
                Dim useHalfWidth As Boolean = False

                While charIndex < formattedText.Length

                    ' Switch every 10 lines between full and half width.
                    If lineIndex Mod 10 = 0 Then useHalfWidth = Not useHalfWidth

                    Dim line = formattedText.FormatLine(charIndex, If(useHalfWidth, halfWidth, maxWidth))
                    y += line.Height

                    ' If line cannot fit on the current page, write it on a new page.
                    Dim lineCannotFit As Boolean = y > maxHeight
                    If lineCannotFit Then
                        page = document.Pages.Add()
                        y = line.Height
                    End If

                    page.Content.DrawText(line, New PdfPoint(margins, heightOffset - y))

                    lineIndex += 1
                    charIndex += line.Length

                End While
            End Using

            document.Save("Lines.pdf")
        End Using
    End Sub

End Module
