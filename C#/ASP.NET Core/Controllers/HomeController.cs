using System.IO;
using Microsoft.AspNetCore.Mvc;
using GemBox.Pdf;

namespace PdfCore.Controllers
{
    public class HomeController : Controller
    {
        static HomeController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Download()
        {
            // Create new PDF document.
            using (var document = new PdfDocument())
            {
                // Add a first empty page.
                document.Pages.Add();

                // Add a second empty page.
                document.Pages.Add();

                using (var stream = new MemoryStream())
                {
                    // Save PDF to stream.
                    document.Save(stream);

                    // Download PDF to client's browser.
                    return File(stream.ToArray(), "application/pdf", "Hello World.pdf");
                }
            }
        }
    }
}