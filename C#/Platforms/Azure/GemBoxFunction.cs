using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using GemBox.Pdf;

public class GemBoxFunction
{
    [Function("GemBoxFunction")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = new PdfDocument())
        {
            // Add a first empty page.
            document.Pages.Add();

            // Add a second empty page.
            document.Pages.Add();

            var fileName = "Output.pdf";

            using var stream = new MemoryStream();
            document.Save(stream);
            var bytes = stream.ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/pdf");
            response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);
            await response.Body.WriteAsync(bytes, 0, bytes.Length);
            return response;
        }
    }
}