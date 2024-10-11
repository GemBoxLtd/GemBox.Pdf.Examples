Imports GemBox.Pdf
Imports Microsoft.Win32

Class MainWindow

    Public Sub New()

        InitializeComponent()

        ComponentInfo.SetLicense("FREE-LIMITED-KEY")
    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)

        Dim fileDialog = New OpenFileDialog()
        fileDialog.Filter = "PDF files (*.pdf)|*.pdf"

        If (fileDialog.ShowDialog() = True) Then
            Using document = PdfDocument.Load(fileDialog.FileName)
                Me.ImageControl.Source = document.ConvertToImageSource(SaveOptions.Image)
            End Using
        End If

    End Sub
End Class