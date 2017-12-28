using System;
using System.IO;
using System.IO.Compression;
using GemBox.Pdf;

namespace MergeFilesCs
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        protected void generatePdfButton_Click(object sender, EventArgs e)
        {
            if (!zipFileUpload.HasFile)
                return;

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(zipFileUpload.FileName);
            string fileExtension = Path.GetExtension(zipFileUpload.FileName);

            if (fileExtension.ToUpperInvariant() != ".ZIP")
            {
                this.Response.Write("Invalid file extension.");
                return;
            }

            // Create a destination document.
            using (PdfDocument destination = PdfDocument.Create())
            {
                // Open source ZIP file.
                using (ZipArchive archive = new ZipArchive(zipFileUpload.FileContent, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        // Open ZIP file entry stream.
                        using (Stream entryStream = entry.Open())
                        {
                            // Load a document from the opened stream.
                            using (PdfDocument source = PdfDocument.Load(entryStream))
                            {
                                // Clone all pages from source to destination document.
                                using (PdfCloneContext context = destination.BeginClone(source))
                                {
                                    foreach (PdfPage sourcePage in source.Pages)
                                        destination.Pages.AddClone(sourcePage);
                                }
                            }
                        }
                    }
                }

                this.Response.Clear();
                this.Response.BufferOutput = false;
                this.Response.ContentType = "application/pdf";
                this.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.pdf", fileNameWithoutExt));

                destination.Save(this.Response.OutputStream);

                this.Response.End();
            }
        }
    }
}