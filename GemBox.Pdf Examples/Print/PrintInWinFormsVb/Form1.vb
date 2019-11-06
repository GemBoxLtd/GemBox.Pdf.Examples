Imports System.Drawing.Printing
Imports System.IO
Imports GemBox.Pdf

Partial Public Class Form1
    Inherits Form

    Dim document As PdfDocument

    Public Sub New()
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")
        InitializeComponent()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Me.document IsNot Nothing Then
            Me.document.Close()
        End If
    End Sub

    Private Sub LoadFileMenuItem_Click(sender As Object, e As EventArgs) Handles LoadFileMenuItem.Click

        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter =
            "PDF files (*.pdf)|*.pdf"

        If (openFileDialog.ShowDialog() = DialogResult.OK) Then
            If Me.document IsNot Nothing Then
                Me.document.Close()
            End If

            Me.document = PdfDocument.Load(openFileDialog.FileName)
            Me.ShowPrintPreview()
        End If

    End Sub

    Private Sub PrintFileMenuItem_Click(sender As Object, e As EventArgs) Handles PrintFileMenuItem.Click

        If document Is Nothing Then Return

        Dim printDialog As New PrintDialog() With {.AllowSomePages = True}
        If (printDialog.ShowDialog() = DialogResult.OK) Then

            Dim printerSettings As PrinterSettings = printDialog.PrinterSettings
            Dim printOptions As New PrintOptions()

            ' Set PrintOptions properties based on PrinterSettings properties.
            printOptions.CopyCount = printerSettings.Copies
            printOptions.FromPage = printerSettings.FromPage - 1
            printOptions.ToPage = If(printerSettings.ToPage = 0, Integer.MaxValue, printerSettings.ToPage - 1)

            Me.document.Print(printerSettings.PrinterName, printOptions)
        End If

    End Sub

    Private Sub ShowPrintPreview()

        ' Create image for each Word document's page.
        Dim images As Image() = Me.CreatePrintPreviewImages()
        Dim imageIndex As Integer = 0

        ' Draw each page's image on PrintDocument for print preview.
        Dim printDocument = New PrintDocument()
        AddHandler printDocument.PrintPage,
            Sub(sender, e)
                Using image As Image = images(imageIndex)
                    Dim graphics = e.Graphics
                    Dim region = graphics.VisibleClipBounds

                    ' Rotate image if it has landscape orientation.
                    If image.Width > image.Height Then image.RotateFlip(RotateFlipType.Rotate270FlipNone)

                    graphics.DrawImage(image, 0, 0, region.Width, region.Height)
                End Using

                imageIndex += 1
                e.HasMorePages = imageIndex < images.Length
            End Sub

        Me.PageUpDown.Value = 1
        Me.PageUpDown.Maximum = images.Length
        Me.printPreviewControl.Document = printDocument

    End Sub

    Private Function CreatePrintPreviewImages() As Image()

        Dim pageCount As Integer = Me.document.Pages.Count
        Dim images = New Image(pageCount - 1) {}

        For pageIndex As Integer = 0 To pageCount - 1
            Dim imageStream = New MemoryStream()
            Dim imageOptions = New ImageSaveOptions(ImageSaveFormat.Png) With {.PageNumber = pageIndex}

            Me.document.Save(imageStream, imageOptions)
            images(pageIndex) = Image.FromStream(imageStream)
        Next

        Return images

    End Function

    Private Sub PageUpDown_ValueChanged(sender As Object, e As EventArgs) Handles PageUpDown.ValueChanged
        Me.printPreviewControl.StartPage = Me.PageUpDown.Value - 1
    End Sub

End Class