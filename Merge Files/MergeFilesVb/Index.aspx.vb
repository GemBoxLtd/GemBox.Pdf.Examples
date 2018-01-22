Imports System.IO
Imports System.IO.Compression
Imports GemBox.Pdf

Public Class Index
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")
    End Sub

    Protected Sub generatePdfButton_Click(sender As Object, e As EventArgs) Handles generatePdfButton.Click

        If Not zipFileUpload.HasFile Then
            Return
        End If

        Dim fileNameWithoutExt As String = Path.GetFileNameWithoutExtension(zipFileUpload.FileName)
        Dim fileExtension As String = Path.GetExtension(zipFileUpload.FileName)

        If fileExtension.ToUpperInvariant() <> ".ZIP" Then
            Me.Response.Write("Invalid file extension.")
            Return
        End If

        ' Create a destination document.
        Using destination As PdfDocument = New PdfDocument()

            ' Open source ZIP file.
            Using archive As ZipArchive = New ZipArchive(zipFileUpload.FileContent, ZipArchiveMode.Read)
                For Each entry As ZipArchiveEntry In archive.Entries
                    ' Open ZIP file entry stream.
                    Using entryStream As Stream = entry.Open()
                        ' Load a document from the opened stream.
                        Using source As PdfDocument = PdfDocument.Load(entryStream)
                            ' Clone all pages from source to destination document.
                            For Each sourcePage As PdfPage In source.Pages
                                destination.Pages.AddClone(sourcePage)
                            Next
                        End Using
                    End Using
                Next
            End Using

            Me.Response.Clear()
            Me.Response.BufferOutput = False
            Me.Response.ContentType = "application/pdf"
            Me.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}.pdf", fileNameWithoutExt))

            destination.Save(Me.Response.OutputStream)

            Me.Response.End()

        End Using
    End Sub
End Class