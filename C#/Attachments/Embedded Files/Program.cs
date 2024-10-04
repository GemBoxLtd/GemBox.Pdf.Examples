using System;
using System.IO;
using System.IO.Compression;
using GemBox.Pdf;

namespace EmbeddedFiles;

static class Program
{
    static void Main()
    {
        Example1();
        Example2();
        Example3();
    }

    static void Example1()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Reading.pdf");
        // Make Attachments panel visible.
        document.PageMode = PdfPageMode.UseAttachments;

        // Extract all the files in the zip archive to a directory on the file system.
        ZipFile.ExtractToDirectory("Attachments.zip", "Attachments");

        // Embed in the PDF document all the files extracted from the zip archive.
        foreach (var filePath in Directory.GetFiles("Attachments", "*", SearchOption.AllDirectories))
        {
            PdfFileSpecification fileSpecification = document.EmbeddedFiles.Add(filePath).Value;

            // Set embedded file description to the relative path of the file in the zip archive.
            fileSpecification.Description = filePath.Substring(filePath.IndexOf('\\') + 1).Replace('\\', '/');
        }

        // Delete the directory where zip archive files were extracted to.
        Directory.Delete("Attachments", recursive: true);

        document.Save("Embedded Files from file system.pdf");
    }

    static void Example2()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("Reading.pdf");
        // Make Attachments panel visible.
        document.PageMode = PdfPageMode.UseAttachments;

        // Embed in the PDF document all the files from the zip archive.
        using (FileStream archiveStream = File.OpenRead("Attachments.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (!string.IsNullOrEmpty(entry.Name))
                {
                    PdfFileSpecification fileSpecification = document.EmbeddedFiles.AddEmpty(entry.Name).Value;

                    // Set embedded file description to the relative path of the file in the zip archive.
                    fileSpecification.Description = entry.FullName;

                    PdfEmbeddedFile embeddedFile = fileSpecification.EmbeddedFile;

                    // Set the embedded file size and modification date.
                    if (entry.Length < int.MaxValue)
                    {
                        embeddedFile.Size = (int)entry.Length;
                    }

                    embeddedFile.ModificationDate = entry.LastWriteTime;

                    // Copy embedded file contents from the zip archive entry.
                    // Embedded file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                    using Stream entryStream = entry.Open();
                    using Stream embeddedFileStream = embeddedFile.OpenWrite(compress: entry.CompressedLength < entry.Length);
                    entryStream.CopyTo(embeddedFileStream);
                }
            }
        }

        document.Save("Embedded Files from Streams.pdf");
    }

    static void Example3()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Add to zip archive all files embedded in the PDF document.
        using var document = PdfDocument.Load("Embedded Files.pdf");
        using FileStream archiveStream = File.Create("Embedded Files.zip");
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true);
        foreach (System.Collections.Generic.KeyValuePair<GemBox.Pdf.Objects.PdfString, PdfFileSpecification> keyFilePair in document.EmbeddedFiles)
        {
            PdfFileSpecification fileSpecification = keyFilePair.Value;

            // Use the description or the name as the relative path of the entry in the zip archive.
            var entryFullName = fileSpecification.Description;
            if (entryFullName?.EndsWith(fileSpecification.Name, StringComparison.Ordinal) != true)
            {
                entryFullName = fileSpecification.Name;
            }

            PdfEmbeddedFile embeddedFile = fileSpecification.EmbeddedFile;

            // Create zip archive entry.
            // Zip archive entry is compressed if the embedded file's compressed size is less than its uncompressed size.
            var compress = embeddedFile.Size == null || embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault();
            ZipArchiveEntry entry = archive.CreateEntry(entryFullName, compress ? CompressionLevel.Optimal : CompressionLevel.NoCompression);

            // Set the modification date, if it is specified in the embedded file.
            DateTimeOffset? modificationDate = embeddedFile.ModificationDate;
            if (modificationDate != null)
            {
                entry.LastWriteTime = modificationDate.GetValueOrDefault();
            }

            // Copy embedded file contents to the zip archive entry.
            using Stream embeddedFileStream = embeddedFile.OpenRead();
            using Stream entryStream = entry.Open();
            embeddedFileStream.CopyTo(entryStream);
        }
    }
}