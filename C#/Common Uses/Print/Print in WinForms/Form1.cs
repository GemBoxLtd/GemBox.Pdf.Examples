using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using GemBox.Pdf;

public partial class Form1 : Form
{
    PdfDocument document;

    public Form1()
    {
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        InitializeComponent();
    }

    void Form1_FormClosing(object sender, FormClosingEventArgs e) => document?.Close();

    void LoadFileMenuItem_Click(object sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter =
            "PDF files (*.pdf)|*.pdf"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            document?.Close();

            document = PdfDocument.Load(openFileDialog.FileName);
            ShowPrintPreview();
        }
    }

    void PrintFileMenuItem_Click(object sender, EventArgs e)
    {
        if (document == null)
        {
            return;
        }

        var printDialog = new PrintDialog() { AllowSomePages = true };
        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            PrinterSettings printerSettings = printDialog.PrinterSettings;
            var printOptions = new PrintOptions
            {
                // Set PrintOptions properties based on PrinterSettings properties.
                CopyCount = printerSettings.Copies,
                FromPage = printerSettings.FromPage - 1,
                ToPage = printerSettings.ToPage == 0 ? int.MaxValue : printerSettings.ToPage - 1
            };

            document.Print(printerSettings.PrinterName, printOptions);
        }
    }

    void ShowPrintPreview()
    {
        // Create image for each Word document's page.
        Image[] images = CreatePrintPreviewImages();
        var imageIndex = 0;

        // Draw each page's image on PrintDocument for print preview.
        var printDocument = new PrintDocument();
        printDocument.PrintPage += (sender, e) =>
        {
            using (Image image = images[imageIndex])
            {
                Graphics graphics = e.Graphics;
                RectangleF region = graphics.VisibleClipBounds;

                // Rotate image if it has landscape orientation.
                if (image.Width > image.Height)
                {
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                }

                graphics.DrawImage(image, 0, 0, region.Width, region.Height);
            }

            ++imageIndex;
            e.HasMorePages = imageIndex < images.Length;
        };

        PageUpDown.Value = 1;
        PageUpDown.Maximum = images.Length;
        PrintPreviewControl.Document = printDocument;
    }

    Image[] CreatePrintPreviewImages()
    {
        var pageCount = document.Pages.Count;
        var images = new Image[pageCount];

        for (var pageIndex = 0; pageIndex < pageCount; ++pageIndex)
        {
            var imageStream = new MemoryStream();
            var imageOptions = new ImageSaveOptions(ImageSaveFormat.Png) { PageNumber = pageIndex };

            document.Save(imageStream, imageOptions);
            images[pageIndex] = Image.FromStream(imageStream);
        }

        return images;
    }

    void PageUpDown_ValueChanged(object sender, EventArgs e) => PrintPreviewControl.StartPage = (int)PageUpDown.Value - 1;
}