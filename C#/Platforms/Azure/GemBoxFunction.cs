using System.IO;
using System.Net;
using System.Threading.Tasks;
using GemBox.Pdf;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace GemBox;

public static class GemBoxFunction
{
    [Function("GemBoxFunction")]
    public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = new PdfDocument();
        // Add a first empty page.
        _ = document.Pages.Add();

        // Add a second empty page.
        _ = document.Pages.Add();

        const string fileName = "Output.pdf";

        using var stream = new MemoryStream();
        document.Save(stream);
        var bytes = stream.ToArray();

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/pdf");
        response.Headers.Add("Content-Disposition", "attachment; filename=" + fileName);
        await response.Body.WriteAsync(bytes);
        return response;
    }
}