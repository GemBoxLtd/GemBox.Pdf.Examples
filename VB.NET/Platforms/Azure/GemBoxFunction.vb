Imports GemBox.Pdf
Imports Microsoft.Azure.Functions.Worker
Imports Microsoft.Azure.Functions.Worker.Http
Imports System.IO
Imports System.Net
Imports System.Threading.Tasks

Public Class GemBoxFunction
    <[Function]("GemBoxFunction")>
    Public Async Function Run(<HttpTrigger(AuthorizationLevel.Anonymous, "get")> req As HttpRequestData) As Task(Of HttpResponseData)

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document As New PdfDocument()

            ' Add a first empty page.
            document.Pages.Add()

            ' Add a second empty page.
            document.Pages.Add()

            Dim fileName = "Output.pdf"

            Using stream As New MemoryStream()
                document.Save(stream)
                Dim bytes = stream.ToArray()

                Dim response = req.CreateResponse(HttpStatusCode.OK)
                response.Headers.Add("Content-Type", "application/pdf")
                response.Headers.Add("Content-Disposition", "attachment; filename=" & fileName)
                Await response.Body.WriteAsync(bytes, 0, bytes.Length)
                Return response
            End Using
        End Using

    End Function
End Class
