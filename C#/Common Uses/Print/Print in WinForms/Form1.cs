using GemBox.Pdf;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private PdfDocument document;

    public Form1()
    {
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        InitializeComponent();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (this.document != null)
            this.document.Close();
    }

    private void LoadFileMenuItem_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter =
            "PDF files (*.pdf)|*.pdf";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            if (this.document != null)
                this.document.Close();

            this.document = PdfDocument.Load(openFileDialog.FileName);
            this.ShowPrintPreview();
        }
    }

    private void PrintFileMenuItem_Click(object sender, EventArgs e)
    {
        if (this.document == null)
            return;

        PrintDialog printDialog = new PrintDialog() { AllowSomePages = true };
        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            PrinterSettings printerSettings = printDialog.PrinterSettings;
            PrintOptions printOptions = new PrintOptions();

            // Set PrintOptions properties based on PrinterSettings properties.
            printOptions.CopyCount = printerSettings.Copies;
            printOptions.FromPage = printerSettings.FromPage - 1;
            printOptions.ToPage = printerSettings.ToPage == 0 ? int.MaxValue : printerSettings.ToPage - 1;

            this.document.Print(printerSettings.PrinterName, printOptions);
        }
    }

    private void ShowPrintPreview()
    {
        // Create image for each Word document's page.
        Image[] images = this.CreatePrintPreviewImages();
        int imageIndex = 0;

        // Draw each page's image on PrintDocument for print preview.
        var printDocument = new PrintDocument();
        printDocument.PrintPage += (sender, e) =>
        {
            using (Image image = images[imageIndex])
            {
                var graphics = e.Graphics;
                var region = graphics.VisibleClipBounds;

                // Rotate image if it has landscape orientation.
                if (image.Width > image.Height)
                    image.RotateFlip(RotateFlipType.Rotate270FlipNone);

                graphics.DrawImage(image, 0, 0, region.Width, region.Height);
            }

            ++imageIndex;
            e.HasMorePages = imageIndex < images.Length;
        };

        this.PageUpDown.Value = 1;
        this.PageUpDown.Maximum = images.Length;
        this.PrintPreviewControl.Document = printDocument;
    }

    private Image[] CreatePrintPreviewImages()
    {
        int pageCount = this.document.Pages.Count;
        var images = new Image[pageCount];

        for (int pageIndex = 0; pageIndex < pageCount; ++pageIndex)
        {
            var imageStream = new MemoryStream();
            var imageOptions = new ImageSaveOptions(ImageSaveFormat.Png) { PageNumber = pageIndex };

            this.document.Save(imageStream, imageOptions);
            images[pageIndex] = Image.FromStream(imageStream);
        }

        return images;
    }

    private void PageUpDown_ValueChanged(object sender, EventArgs e)
    {
        this.PrintPreviewControl.StartPage = (int)this.PageUpDown.Value - 1;
    }
}