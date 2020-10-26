using GemBox.Pdf;
using GemBox.Pdf.Content;

class Program
{
    static void Main()
    {
        Example1();

        Example2();

        Example3();

        Example4();
    }

    static void Example1()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // Add a page.
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                // Set font family and size.
                // All text appended next uses the specified font family and size.
                formattedText.FontFamily = new PdfFontFamily("Times New Roman");
                formattedText.FontSize = 24;

                formattedText.AppendLine("Hello World");

                // Reset font family and size for all text appended next.
                formattedText.FontFamily = new PdfFontFamily("Calibri");
                formattedText.FontSize = 12;

                formattedText.AppendLine(" with ");

                // Set font style and color for all text appended next.
                formattedText.FontStyle = PdfFontStyle.Italic;
                formattedText.Color = PdfColor.FromRgb(1, 0, 0);

                formattedText.Append("GemBox.Pdf");

                // Reset font style and color for all text appended next.
                formattedText.FontStyle = PdfFontStyle.Normal;
                formattedText.Color = PdfColor.FromRgb(0, 0, 0);

                formattedText.Append(" component!");

                // Set the location of the bottom-left corner of the text.
                // We want top-left corner of the text to be at location (100, 100)
                // from top-left corner of the page.
                // NOTE: In PDF, location (0, 0) is at the bottom-left corner of the page
                // and the positive y axis extends vertically upward.
                double x = 100, y = page.CropBox.Top - 100 - formattedText.Height;

                // Draw text to the page.
                page.Content.DrawText(formattedText, new PdfPoint(x, y));
            }

            document.Save("Writing.pdf");
        }
    }

    static void Example2()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            double margin = 10;

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.TextAlignment = PdfTextAlignment.Left;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is left aligned, ").
                    Append("placed in the top-left corner of the page and ").
                    Append("its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(margin,
                        page.CropBox.Top - margin - formattedText.Height));

                formattedText.Clear();

                formattedText.TextAlignment = PdfTextAlignment.Center;
                formattedText.MaxTextWidth = 200;
                formattedText.Append("This text is center aligned, ").
                    Append("placed in the top-center part of the page ").
                    Append("and its width should not exceed 200 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2,
                        page.CropBox.Top - margin - formattedText.Height));

                formattedText.Clear();

                formattedText.TextAlignment = PdfTextAlignment.Right;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is right aligned, ").
                    Append("placed in the top-right corner of the page ").
                    Append("and its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                        page.CropBox.Top - margin - formattedText.Height));

                formattedText.Clear();

                formattedText.TextAlignment = PdfTextAlignment.Left;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is left aligned, ").
                    Append("placed in the bottom-left corner of the page and ").
                    Append("its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(margin,
                        margin));

                formattedText.Clear();

                formattedText.TextAlignment = PdfTextAlignment.Center;
                formattedText.MaxTextWidth = 200;
                formattedText.Append("This text is center aligned, ").
                    Append("placed in the bottom-center part of the page and ").
                    Append("its width should not exceed 200 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint((page.CropBox.Width - formattedText.MaxTextWidth) / 2,
                        margin));

                formattedText.Clear();

                formattedText.TextAlignment = PdfTextAlignment.Right;
                formattedText.MaxTextWidth = 100;
                formattedText.Append("This text is right aligned, ").
                    Append("placed in the bottom-right corner of the page and ").
                    Append("its width should not exceed 100 points.");
                page.Content.DrawText(formattedText,
                    new PdfPoint(page.CropBox.Width - margin - formattedText.MaxTextWidth,
                        margin));

                formattedText.Clear();

                formattedText.TextAlignment = PdfTextAlignment.Justify;
                formattedText.MaxTextWidths = new double[] { 200, 150, 100 };
                formattedText.Append("This text has justified alignment, ").
                    Append("is placed in the center of the page and ").
                    Append("its first line should not exceed 200 points, ").
                    Append("its second line should not exceed 150 points and ").
                    Append("its third and all other lines should not exceed 100 points.");
                // Center the text based on the width of the most lines, which is formattedText.MaxTextWidths[2].
                page.Content.DrawText(formattedText,
                    new PdfPoint((page.CropBox.Width - formattedText.MaxTextWidths[2]) / 2,
                        (page.CropBox.Height - formattedText.Height) / 2));
            }

            document.Save("Alignment and Positioning.pdf");
        }
    }

    static void Example3()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                var text = "Rotated by 30 degrees around origin.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                var origin = new PdfPoint(50, 650);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                var transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Rotated by 30 degrees around center.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(300, 650);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled horizontally by 0.5 around origin.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(50, 500);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(0.5, 1);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled horizontally by 0.5 around center.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(300, 500);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(0.5, 1, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled vertically by 2 around origin.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(50, 400);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(1, 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Scaled vertically by 2 around center.";
                formattedText.Opacity = 0.2;
                formattedText.Append(text);
                origin = new PdfPoint(300, 400);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.Append(text);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Scale(1, 2, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Rotated by 30 degrees around origin and ";
                var text2 = "scaled horizontally by 0.5 and ";
                var text3 = "vertically by 2 around origin.";
                formattedText.Opacity = 0.2;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                origin = new PdfPoint(50, 200);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30);
                transform.Scale(0.5, 2);
                page.Content.DrawText(formattedText, transform);

                formattedText.Clear();

                text = "Rotated by 30 degrees around center and ";
                text2 = "scaled horizontally by 0.5 and ";
                text3 = "vertically by 2 around center.";
                formattedText.Opacity = 0.2;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                origin = new PdfPoint(300, 200);
                page.Content.DrawText(formattedText, origin);
                formattedText.Clear();
                formattedText.Opacity = 1;
                formattedText.AppendLine(text).AppendLine(text2).Append(text3);
                transform = PdfMatrix.Identity;
                transform.Translate(origin.X, origin.Y);
                transform.Rotate(30, formattedText.Width / 2, formattedText.Height / 2);
                transform.Scale(0.5, 2, formattedText.Width / 2, formattedText.Height / 2);
                page.Content.DrawText(formattedText, transform);
            }

            document.Save("Transformations.pdf");
        }
    }

    static void Example4()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            var page = document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);

                formattedText.AppendLine("An example of a fully vocalised (vowelised or vowelled) Arabic ").
                    Append("from the Basmala: ");

                formattedText.Language = new PdfLanguage("ar-SA");
                formattedText.Font = new PdfFont("Arial", 24);
                formattedText.Append("بِسْمِ ٱللَّٰهِ ٱلرَّحْمَٰنِ ٱلرَّحِيمِ");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.AppendLine(", which means: ").
                    Append("In the name of God, the All-Merciful, the Especially-Merciful.");

                page.Content.DrawText(formattedText, new PdfPoint(50, 750));

                formattedText.Clear();

                formattedText.Append("An example of Hebrew: ");

                formattedText.Language = new PdfLanguage("he-IL");
                formattedText.Font = new PdfFont("Arial", 24);
                formattedText.Append("מה קורה");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.AppendLine(", which means: What's going on, ").
                    Append("and ");

                formattedText.Language = new PdfLanguage("he-IL");
                formattedText.Font = new PdfFont("Arial", 24);
                formattedText.Append("תודה לכולם");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append(", which means: Thank you all.");

                page.Content.DrawText(formattedText, new PdfPoint(50, 650));

                formattedText.Clear();

                formattedText.LineHeight = 50;

                formattedText.Append("An example of Thai: ");
                formattedText.Language = new PdfLanguage("th-TH");
                formattedText.Font = new PdfFont("Leelawadee UI", 16);
                formattedText.AppendLine("ภัำ");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Tamil: ");
                formattedText.Language = new PdfLanguage("ta-IN");
                formattedText.Font = new PdfFont("Nirmala UI", 16);
                formattedText.AppendLine("போது");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Bengali: ");
                formattedText.Language = new PdfLanguage("be-IN");
                formattedText.Font = new PdfFont("Nirmala UI", 16);
                formattedText.AppendLine("আবেদনকারীর মাতার পিতার বর্তমান স্থায়ী ঠিকানা নমিনি নাম");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Gujarati: ");
                formattedText.Language = new PdfLanguage("gu-IN");
                formattedText.Font = new PdfFont("Nirmala UI", 16);
                formattedText.AppendLine("કાર્બન કેમેસ્ટ્રી");

                formattedText.Language = new PdfLanguage("en-US");
                formattedText.Font = new PdfFont("Calibri", 12);
                formattedText.Append("An example of Osage: ");
                formattedText.Language = new PdfLanguage("osa");
                formattedText.Font = new PdfFont("Gadugi", 16);
                formattedText.Append("𐓏𐓘𐓻𐓘𐓻𐓟 𐒻𐓟");

                page.Content.DrawText(formattedText, new PdfPoint(50, 350));
            }

            document.Save("Complex scripts.pdf");
        }
    }
}
