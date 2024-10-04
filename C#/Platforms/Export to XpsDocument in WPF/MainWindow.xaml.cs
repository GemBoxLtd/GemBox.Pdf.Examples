using System.Windows;
using System.Windows.Xps.Packaging;
using GemBox.Pdf;
using Microsoft.Win32;

//namespace WpfExportToXpsDocument;     // tried for CA1050 but fatally unlinks InitializeComponent, DocumentViewer in default NS

public partial class MainWindow : Window
{
    XpsDocument xpsDocument;

    public MainWindow()
    {
        InitializeComponent();

        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
    }

    void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog
        {
            Filter = "PDF files (*.pdf)|*.pdf"
        };

        if (fileDialog.ShowDialog() == true)
        {
            using var document = PdfDocument.Load(fileDialog.FileName);
            // XpsDocument needs to stay referenced so that DocumentViewer can access additional required resources.
            // Otherwise, GC will collect/dispose XpsDocument and DocumentViewer will not work.
            xpsDocument = document.ConvertToXpsDocument(SaveOptions.Xps);

            DocumentViewer.Document = xpsDocument.GetFixedDocumentSequence();
        }
    }
}