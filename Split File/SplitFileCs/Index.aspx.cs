using System;
using System.IO;
using System.IO.Compression;
using GemBox.Pdf;

namespace SplitFileCs
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        protected void generateZipButton_Click(object sender, EventArgs e)
        {
            if (!pdfFileUpload.HasFile)
                return;

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(pdfFileUpload.FileName);
            string fileExtension = Path.GetExtension(pdfFileUpload.FileName);

            if (fileExtension.ToUpperInvariant() != ".PDF")
            {
                this.Response.Write("Invalid file extension.");
                return;
            }

            // Open source PDF file.
            using (PdfDocument source = PdfDocument.Load(pdfFileUpload.FileContent))
            {
                using (MemoryStream archiveStream = new MemoryStream())
                {
                    // Create a destination ZIP file.
                    using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Update, true))
                    {
                        // For each source document page:
                        for (int index = 0; index < source.Pages.Count; index++)
                        {
                            // Create new ZIP entry.
                            ZipArchiveEntry entry = archive.CreateEntry(string.Format("{0}{1}{2}", fileNameWithoutExt, index + 1, fileExtension));
                            // Open ZIP entry stream.
                            using (Stream entryStream = entry.Open())
                            {
                                // Create destination document.
                                using (PdfDocument destination = new PdfDocument())
                                {
                                    // Clone source document page to destination document.
                                    destination.Pages.AddClone(source.Pages[index]);
                                    // Save destination document to ZIP entry stream.
                                    destination.Save(entryStream);
                                }
                            }
                        }
                    }

                    this.Response.Clear();
                    this.Response.BufferOutput = false;
                    this.Response.ContentType = "application/zip";
                    this.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.zip", fileNameWithoutExt));

                    archiveStream.WriteTo(this.Response.OutputStream);

                    this.Response.End();
                }
            }
        }
    }
}