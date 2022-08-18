using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using GemBox.Pdf;

public static class GemBoxFunction
{
    [FunctionName("GemBoxFunction")]
#pragma warning disable CS1998 // Async method lacks 'await' operators.
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
#pragma warning restore CS1998
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // Add a first empty page.
            document.Pages.Add();

            // Add a second empty page.
            document.Pages.Add();

            var fileName = "Output.pdf";

            using (var stream = new MemoryStream())
            {
                document.Save(stream);
                return new FileContentResult(stream.ToArray(), "application/pdf") { FileDownloadName = fileName };
            }
        }
    }
}