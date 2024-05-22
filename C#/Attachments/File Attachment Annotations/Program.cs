using GemBox.Pdf;
using GemBox.Pdf.Annotations;
using System;
using System.IO;
using System.IO.Compression;

class Program
{
    static void Main()
    {
        CreateFileAttachmentAnnotationsFromFileSystem();

        CreateFileAttachmentAnnotationsFromStreams();

        ExtractFilesFromFileAttachmentAnnotations();
    }

    static void CreateFileAttachmentAnnotationsFromFileSystem()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            // Extract all the files in the zip archive to a directory on the file system.
            ZipFile.ExtractToDirectory("Attachments.zip", "Attachments");

            var page = document.Pages[0];
            int rowCount = 0;
            double spacing = page.CropBox.Width / 5,
                left = spacing,
                bottom = page.CropBox.Height - 200;

            // Add file attachment annotations to the PDF page from all the files extracted from the zip archive.
            foreach (var filePath in Directory.GetFiles("Attachments", "*", SearchOption.AllDirectories))
            {
                var fileAttachmentAnnotation = page.Annotations.AddFileAttachment(left - 10, bottom - 10, filePath);

                // Set a different icon for each file attachment annotation in a row.
                fileAttachmentAnnotation.Appearance.Icon = (PdfFileAttachmentIcon)(rowCount + 1);

                // Set attachment description to the relative path of the file in the zip archive.
                fileAttachmentAnnotation.Description = filePath.Substring(filePath.IndexOf('\\') + 1).Replace('\\', '/');

                // There are, at most, 4 file attachment annotations in a row.
                ++rowCount;
                if (rowCount < 4)
                    left += spacing;
                else
                {
                    rowCount = 0;
                    left = spacing;
                    bottom -= spacing;
                }
            }

            // Delete the directory where zip archive files were extracted to.
            Directory.Delete("Attachments", recursive: true);

            document.Save("File Attachment Annotations from file system.pdf");
        }
    }

    static void CreateFileAttachmentAnnotationsFromStreams()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Reading.pdf"))
        {
            var page = document.Pages[0];
            int rowCount = 0;
            double spacing = page.CropBox.Width / 5,
                left = spacing,
                bottom = page.CropBox.Height - 200;

            // Add file attachment annotations to the PDF page from all the files from the zip archive.
            using (var archiveStream = File.OpenRead("Attachments.zip"))
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true))
                foreach (var entry in archive.Entries)
                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        var fileAttachmentAnnotation = page.Annotations.AddEmptyFileAttachment(left, bottom, entry.Name);

                        // Set a different icon for each file attachment annotation in a row.
                        fileAttachmentAnnotation.Appearance.Icon = (PdfFileAttachmentIcon)(rowCount + 1);

                        // Set attachment description to the relative path of the file in the zip archive.
                        fileAttachmentAnnotation.Description = entry.FullName;

                        var embeddedFile = fileAttachmentAnnotation.File.EmbeddedFile;

                        // Set the embedded file size and modification date.
                        if (entry.Length < int.MaxValue)
                            embeddedFile.Size = (int)entry.Length;
                        embeddedFile.ModificationDate = entry.LastWriteTime;

                        // Copy embedded file contents from the zip archive entry.
                        // Embedded file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                        using (var entryStream = entry.Open())
                        using (var embeddedFileStream = embeddedFile.OpenWrite(compress: entry.CompressedLength < entry.Length))
                            entryStream.CopyTo(embeddedFileStream);

                        // There are, at most, 4 file attachment annotations in a row.
                        ++rowCount;
                        if (rowCount < 4)
                            left += spacing;
                        else
                        {
                            rowCount = 0;
                            left = spacing;
                            bottom -= spacing;
                        }
                    }

            document.Save("File Attachment Annotations from Streams.pdf");
        }
    }

    static void ExtractFilesFromFileAttachmentAnnotations()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Add to zip archive all files from file attachment annotations located on the first page.
        using (var document = PdfDocument.Load("File Attachment Annotations.pdf"))
        using (var archiveStream = File.Create("File Attachment Annotation Files.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            foreach (var annotation in document.Pages[0].Annotations)
                if (annotation.AnnotationType == PdfAnnotationType.FileAttachment)
                {
                    var fileAttachmentAnnotation = (PdfFileAttachmentAnnotation)annotation;

                    var fileSpecification = fileAttachmentAnnotation.File;

                    // Use the description or the file name as the relative path of the entry in the zip archive.
                    var entryFullName = fileAttachmentAnnotation.Description;
                    if (entryFullName == null || !entryFullName.EndsWith(fileSpecification.Name, StringComparison.Ordinal))
                        entryFullName = fileSpecification.Name;

                    var embeddedFile = fileSpecification.EmbeddedFile;

                    // Create zip archive entry.
                    // Zip archive entry is compressed if the embedded file's compressed size is less than its uncompressed size.
                    bool compress = embeddedFile.Size == null || embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault();
                    var entry = archive.CreateEntry(entryFullName, compress ? CompressionLevel.Optimal : CompressionLevel.NoCompression);

                    // Set the modification date, if it is specified in the embedded file.
                    var modificationDate = embeddedFile.ModificationDate;
                    if (modificationDate != null)
                        entry.LastWriteTime = modificationDate.GetValueOrDefault();

                    // Copy embedded file contents to the zip archive entry.
                    using (var embeddedFileStream = embeddedFile.OpenRead())
                    using (var entryStream = entry.Open())
                        embeddedFileStream.CopyTo(entryStream);
                }
    }
}
