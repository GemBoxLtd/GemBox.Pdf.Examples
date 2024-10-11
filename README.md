[![NuGet version](https://img.shields.io/nuget/v/GemBox.Pdf?style=for-the-badge)](https://www.nuget.org/packages/GemBox.Pdf/) [![NuGet downloads](https://img.shields.io/nuget/dt/GemBox.Pdf?style=for-the-badge)](https://www.nuget.org/packages/GemBox.Pdf/) [![Visual Studio Marketplace rating](https://img.shields.io/visual-studio-marketplace/stars/GemBoxSoftware.GemBoxPdf?style=for-the-badge)](https://marketplace.visualstudio.com/items?itemName=GemBoxSoftware.GemBoxPdf)

## What is GemBox.Pdf?

GemBox.Pdf is a .NET component that enables you to read, write, edit, and print PDF files from .NET applications.

With GemBox.Pdf you get a fast and reliable component that’s easy to use and doesn't depend on Adobe Acrobat. It requires only .NET so you can deploy your applications without having to think about other licenses.

## GemBox.Pdf Features

- [Read](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-read-pdf/205), [write](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-create-write-pdf-file/209), and [update](https://www.gemboxsoftware.com/pdf/examples/incremental-update/204) PDF files.
- [Convert](https://www.gemboxsoftware.com/pdf/examples/c-sharp-convert-pdf-to-image/208) PDF files to image (PNG, JPEG, GIF, BMP, TIFF, and WMP) and XML Paper Specification (XPS) formats.
- View PDF files in [Azure Functions](https://www.gemboxsoftware.com/pdf/examples/create-pdf-on-azure-functions-app-service/1601), [Blazor](https://www.gemboxsoftware.com/pdf/examples/blazor-create-pdf/1402), [ASP.NET Core](https://www.gemboxsoftware.com/pdf/examples/asp-net-core-create-pdf/1401), [MAUI](https://www.gemboxsoftware.com/pdf/examples/create-pdf-file-maui/1502), and [WPF](https://www.gemboxsoftware.com/pdf/examples/pdf-xpsdocument-wpf/1001) applications.
- Process PDF files on Windows, [Linux, macOS](https://www.gemboxsoftware.com/pdf/examples/create-pdf-on-linux-net-core/1301), [Android, and iOS](https://www.gemboxsoftware.com/pdf/examples/create-pdf-file-xamarin/1501) operating systems.
- [Extract text](https://www.gemboxsoftware.com/pdf/examples/extract-content-pdf/6005) from PDF files.
- [Extract images](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-export-import-images-to-pdf/206#export) from PDF files.
- [Redact content](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-redact-content-pdf/410) from PDF files.
- [Print](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-print-pdf/207) PDF files.
- [Merge](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-merge-pdf/201) PDF files.
- [Split](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-split-pdf/202) PDF files.
- [Create](https://www.gemboxsoftware.com/pdf/examples/c-sharp-create-pdf-interactive-form-fields/505), [fill in](https://www.gemboxsoftware.com/pdf/examples/c-sharp-fill-in-pdf-interactive-form/502), [flatten](https://www.gemboxsoftware.com/pdf/examples/c-sharp-flatten-pdf-interactive-form-fields/506), [read](https://www.gemboxsoftware.com/pdf/examples/c-sharp-read-pdf-interactive-form-fields/501), and [export](https://www.gemboxsoftware.com/pdf/examples/c-sharp-export-pdf-interactive-form-data/503) PDF interactive forms.
- Extract text from images or scanned PDF files with [optical character recognition (OCR)](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-ocr-pdf/408).
- [Encrypt](https://www.gemboxsoftware.com/pdf/examples/decrypt-encrypt-pdf-file/1101) and [digitally sign](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-pdf-digital-signature/1102) PDF files.
- [Clone or import](https://www.gemboxsoftware.com/pdf/examples/cloning-pdf-pages/203) pages between PDF documents.
- Get, create, or edit [bookmarks (outlines)](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-pdf-bookmarks-outlines/301).
- Get and set [document properties](https://www.gemboxsoftware.com/pdf/examples/pdf-document-properties/302).
- Get and set [viewer preferences](https://www.gemboxsoftware.com/pdf/examples/pdf-viewer-preferences/303).
- Add [watermark](https://www.gemboxsoftware.com/pdf/examples/pdf-watermarks/305), [header and footer](https://www.gemboxsoftware.com/pdf/examples/pdf-header-footer/304) to PDF pages.
- Get, create, remove, or reorder [pages](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-pdf-pages/401).
- Add [text](https://www.gemboxsoftware.com/pdf/examples/pdf-content-groups/409) and [marked content](https://www.gemboxsoftware.com/pdf/examples/pdf-marked-content/407) to pages and [format](https://www.gemboxsoftware.com/pdf/examples/pdf-content-formatting/307) (fill, stroke, and clip) the content.
- Annotate PDF pages with [hyperlinks](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-pdf-hyperlinks/308).
- [Use basic PDF objects](https://www.gemboxsoftware.com/pdf/examples/basic-pdf-objects/402) for currently unsupported PDF features.
- [Specify fonts location](https://www.gemboxsoftware.com/pdf/examples/use-embed-private-fonts-pdf/404).
- [Medium trust](https://www.gemboxsoftware.com/pdf/examples/getting-started/101) support.

## Get Started

You are not sure how to start working with PDF documents in .NET using GemBox.Pdf? Check the code below that shows how to create a PDF file from scratch and write 'Hello World!' on it.

```csharp
// If using Professional version, put your serial key below.
ComponentInfo.SetLicense("FREE-LIMITED-KEY");

// Create a new PDF document.
using (var document = new PdfDocument())
{
    // Add a page.
    var page = document.Pages.Add();

    using (var formattedText = new PdfFormattedText())
    {
        // Add text.
        formattedText.AppendLine("Hello World");

        // Draw text to the page.
        page.Content.DrawText(formattedText, new PdfPoint(100, 700));
    }

    // Save the document as PDF file.
    document.Save("Writing.pdf");
}
```

For more GemBox.Pdf code examples and demos, please visit our [examples page](https://www.gemboxsoftware.com/pdf/examples/c-sharp-vb-net-pdf-library/101).

## Installation

You can download GemBox.Pdf from [NuGet 📦](https://www.nuget.org/packages/GemBox.Pdf/) or from [Downloads 🛠️](https://www.gemboxsoftware.com/pdf/downloads/).

## Resources

- [Product Page](https://www.gemboxsoftware.com/pdf)
- [Examples](https://www.gemboxsoftware.com/pdf/examples)
- [Documentation](https://www.gemboxsoftware.com/pdf/docs/introduction.html)
- [API Reference](https://www.gemboxsoftware.com/pdf/docs/GemBox.Pdf.html)
- [Forum](https://forum.gemboxsoftware.com/c/gembox-pdf/7)
- [Blog](https://www.gemboxsoftware.com/gembox-pdf)
