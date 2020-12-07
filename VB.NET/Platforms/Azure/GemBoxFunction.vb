Imports System.IO
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Azure.WebJobs
Imports Microsoft.Azure.WebJobs.Extensions.Http
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Extensions.Logging
Imports GemBox.Pdf

Module GemBoxFunction
        <FunctionName("GemBoxFunction")>
        Async Function Run(
            <HttpTrigger(AuthorizationLevel.Anonymous, "get", Route := Nothing)> req As HttpRequest,
            log As ILogger) as Task(Of IActionResult)
        
            ' If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY")

            Using document As New PdfDocument()

                ' Add a first empty page.
                document.Pages.Add()

                ' Add a second empty page.
                document.Pages.Add()

                Dim fileName = "Output.pdf"

                Using stream As new MemoryStream()
                    document.Save(stream)
                    return New FileContentResult(stream.ToArray(), "application/pdf") With { .FileDownloadName = fileName }
                End Using

            End Using
             
        End Function
End Module
