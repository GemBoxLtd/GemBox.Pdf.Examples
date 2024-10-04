using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;
using GemBox.Pdf;
using Microsoft.Win32;

public partial class MainWindow : Window
{
    PdfDocument document;

    public MainWindow()
    {
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        InitializeComponent();
    }

    void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => document?.Close();

    void LoadFileBtn_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter =
            "PDF files (*.pdf)|*.pdf"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            document?.Close();

            document = PdfDocument.Load(openFileDialog.FileName);
            ShowPrintPreview();
        }
    }

    void PrintFileBtn_Click(object sender, RoutedEventArgs e)
    {
        if (document == null)
        {
            return;
        }

        var printDialog = new PrintDialog() { UserPageRangeEnabled = true };
        if (printDialog.ShowDialog() == true)
        {
            var printOptions = new PrintOptions(printDialog.PrintTicket.GetXmlStream())
            {
                FromPage = printDialog.PageRange.PageFrom - 1,
                ToPage = printDialog.PageRange.PageTo == 0 ? int.MaxValue : printDialog.PageRange.PageTo - 1
            };

            document.Print(printDialog.PrintQueue.FullName, printOptions);
        }
    }

    void ShowPrintPreview()
    {
        XpsDocument xpsDocument = document.ConvertToXpsDocument(SaveOptions.Xps);

        // Note, XpsDocument must stay referenced so that DocumentViewer can access additional resources from it.
        // Otherwise, GC will collect/dispose XpsDocument and DocumentViewer will no longer work.
        DocViewer.Tag = xpsDocument;
        DocViewer.Document = xpsDocument.GetFixedDocumentSequence();
    }
}