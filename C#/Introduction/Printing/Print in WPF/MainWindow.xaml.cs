using GemBox.Pdf;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;

public partial class MainWindow : Window
{
    private PdfDocument document;

    public MainWindow()
    {
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        InitializeComponent();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (this.document != null)
            this.document.Close();
    }

    private void LoadFileBtn_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter =
            "PDF files (*.pdf)|*.pdf";

        if (openFileDialog.ShowDialog() == true)
        {
            if (this.document != null)
                this.document.Close();

            this.document = PdfDocument.Load(openFileDialog.FileName);
            this.ShowPrintPreview();
        }
    }

    private void PrintFileBtn_Click(object sender, RoutedEventArgs e)
    {
        if (this.document == null)
            return;

        PrintDialog printDialog = new PrintDialog() { UserPageRangeEnabled = true };
        if (printDialog.ShowDialog() == true)
        {
            PrintOptions printOptions = new PrintOptions(printDialog.PrintTicket.GetXmlStream());

            printOptions.FromPage = printDialog.PageRange.PageFrom - 1;
            printOptions.ToPage = printDialog.PageRange.PageTo == 0 ? int.MaxValue : printDialog.PageRange.PageTo - 1;

            this.document.Print(printDialog.PrintQueue.FullName, printOptions);
        }
    }

    private void ShowPrintPreview()
    {
        XpsDocument xpsDocument = this.document.ConvertToXpsDocument(SaveOptions.Xps);

        // Note, XpsDocument must stay referenced so that DocumentViewer can access additional resources from it.
        // Otherwise, GC will collect/dispose XpsDocument and DocumentViewer will no longer work.
        this.DocViewer.Tag = xpsDocument;
        this.DocViewer.Document = xpsDocument.GetFixedDocumentSequence();
    }
}