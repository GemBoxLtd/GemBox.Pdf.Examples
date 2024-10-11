Imports GemBox.Pdf
Imports Microsoft.Win32
Imports System.Windows.Xps.Packaging

Class MainWindow

    Dim xpsDoc As XpsDocument

    Public Sub New()
        InitializeComponent()

        ComponentInfo.SetLicense("FREE-LIMITED-KEY")
    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)

        Dim fileDialog = New OpenFileDialog()
        fileDialog.Filter = "PDF files (*.pdf)|*.pdf"

        If (fileDialog.ShowDialog() = True) Then
            Using document = PdfDocument.Load(fileDialog.FileName)
                ' XpsDocument needs to stay referenced so that DocumentViewer can access additional required resources.
                ' Otherwise, GC will collect/dispose XpsDocument and DocumentViewer will not work.
                Me.xpsDoc = document.ConvertToXpsDocument(SaveOptions.Xps)

                Me.DocumentViewer.Document = Me.xpsDoc.GetFixedDocumentSequence()
            End Using
        End If

    End Sub

End Class