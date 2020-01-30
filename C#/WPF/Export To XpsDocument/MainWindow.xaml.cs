using System.Windows;
using System.Windows.Xps.Packaging;
using GemBox.Pdf;
using Microsoft.Win32;

public partial class MainWindow : Window
{
    XpsDocument xpsDocument;

    public MainWindow()
    {
        InitializeComponent();

        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "PDF files (*.pdf)|*.pdf";

        if (fileDialog.ShowDialog() == true)
        {
            using (var document = PdfDocument.Load(fileDialog.FileName))
            {
                // XpsDocument needs to stay referenced so that DocumentViewer can access additional required resources.
                // Otherwise, GC will collect/dispose XpsDocument and DocumentViewer will not work.
                this.xpsDocument = document.ConvertToXpsDocument(SaveOptions.Xps);

                this.DocumentViewer.Document = this.xpsDocument.GetFixedDocumentSequence();
            }
        }
    }
}