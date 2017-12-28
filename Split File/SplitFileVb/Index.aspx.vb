Imports System.IO
Imports System.IO.Compression
Imports GemBox.Pdf

Public Class Index
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")
    End Sub

    Protected Sub generateZipButton_Click(sender As Object, e As EventArgs) Handles generateZipButton.Click

        If Not pdfFileUpload.HasFile Then
            Return
        End If

        Dim fileNameWithoutExt As String = Path.GetFileNameWithoutExtension(pdfFileUpload.FileName)
        Dim fileExtension As String = Path.GetExtension(pdfFileUpload.FileName)

        If fileExtension.ToUpperInvariant() <> ".PDF" Then
            Me.Response.Write("Invalid file extension.")
            Return
        End If

        ' Open source PDF file.
        Using source As PdfDocument = PdfDocument.Load(pdfFileUpload.FileContent)
            Using archiveStream = New MemoryStream()
                ' Create a destination ZIP file.
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Update, True)
                    ' For each source document page:
                    For index As Integer = 0 To source.Pages.Count - 1
                        ' Create new ZIP entry.
                        Dim entry As ZipArchiveEntry = archive.CreateEntry(String.Format("{0}{1}{2}", fileNameWithoutExt, index + 1, fileExtension))
                        ' Open ZIP entry stream.
                        Using entryStream As Stream = entry.Open()
                            ' Create destination document.
                            Using destination As PdfDocument = PdfDocument.Create()
                                ' Clone source document page to destination document.
                                Using context As PdfCloneContext = destination.BeginClone(source)
                                    destination.Pages.AddClone(source.Pages(index))
                                End Using
                                ' Save destination document to ZIP entry stream.
                                destination.Save(entryStream)
                            End Using
                        End Using
                    Next
                End Using

                Me.Response.Clear()
                Me.Response.BufferOutput = False
                Me.Response.ContentType = "application/zip"
                Me.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}.zip", fileNameWithoutExt))

                archiveStream.WriteTo(Me.Response.OutputStream)

                Me.Response.End()

            End Using
        End Using

    End Sub
End Class