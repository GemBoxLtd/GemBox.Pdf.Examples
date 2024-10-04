using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using GemBox.Pdf;
using GemBox.Pdf.Objects;
using GemBox.Pdf.Portfolios;

namespace Portfolios;

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

        using var document = PdfDocument.Load("PortfolioTemplate.pdf");
        // Make the document a PDF portfolio (a collection of file attachments).
        PdfPortfolio portfolio = document.SetPortfolio();

        // Extract all the files in the zip archive to a directory on the file system.
        ZipFile.ExtractToDirectory("Attachments.zip", "Attachments");

        // Add files contained directly in the 'Attachments' directory to the portfolio files.
        foreach (var filePath in Directory.GetFiles("Attachments", "*", SearchOption.TopDirectoryOnly))
        {
            portfolio.Files.Add(filePath);
        }

        // Recursively add directories and their files contained in the 'Attachments' directory to the portfolio folders.
        foreach (var folderPath in Directory.GetDirectories("Attachments", "*", SearchOption.TopDirectoryOnly))
        {
            portfolio.Folders.Add(folderPath, recursive: true);
        }

        // Delete the directory where zip archive files were extracted to.
        Directory.Delete("Attachments", recursive: true);

        // Set the first PDF file contained in the portfolio to be initially presented in the user interface.
        // Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
        portfolio.InitialFile = document.EmbeddedFiles.Select(entry => entry.Value).FirstOrDefault(fileSpec => fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal));

        // Hide all existing portfolio fields except 'Size'.
        foreach (System.Collections.Generic.KeyValuePair<PdfName, PdfPortfolioField> portfolioFieldEntry in portfolio.Fields)
        {
            portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name != "Size";
        }

        // Add a new portfolio field with display name 'Full Name' and it should be in the first column.
        System.Collections.Generic.KeyValuePair<PdfName, PdfPortfolioField> portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName");
        PdfPortfolioField portfolioField = portfolioFieldKeyAndValue.Value;
        portfolioField.Name = "Full Name";
        portfolioField.Order = 0;

        // For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
        SetFullNameFieldValue(portfolio.Files, portfolio.Folders, string.Empty, portfolioFieldKeyAndValue.Key);

        document.Save("Portfolio from file system.pdf");
    }

    static void Example2()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using var document = PdfDocument.Load("PortfolioTemplate.pdf");
        // Make the document a PDF portfolio (a collection of file attachments).
        PdfPortfolio portfolio = document.SetPortfolio();

        // Add all files and folders from the zip archive to the portfolio.
        using (FileStream archiveStream = File.OpenRead("Attachments.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                // Get or create portfolio folder hierarchy from the zip entry full name.
                PdfPortfolioFolder folder = GetOrAddFolder(portfolio, entry.FullName);

                if (!string.IsNullOrEmpty(entry.Name))
                {
                    // Zip archive entry is a file.
                    PdfPortfolioFileCollection files = folder == null ? portfolio.Files : folder.Files;

                    PdfEmbeddedFile embeddedFile = files.AddEmpty(entry.Name).EmbeddedFile;

                    // Set the portfolio file size and modification date.
                    if (entry.Length < int.MaxValue)
                    {
                        embeddedFile.Size = (int)entry.Length;
                    }

                    embeddedFile.ModificationDate = entry.LastWriteTime;

                    // Copy portfolio file contents from the zip archive entry.
                    // Portfolio file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                    using Stream entryStream = entry.Open();
                    using Stream embeddedFileStream = embeddedFile.OpenWrite(compress: entry.CompressedLength < entry.Length);
                    entryStream.CopyTo(embeddedFileStream);
                }
                else
                {
                    // Zip archive entry is a folder.
                    // Set the portfolio folder modification date.
                    folder.ModificationDate = entry.LastWriteTime;
                }
            }
        }

        // Set the first PDF file contained in the portfolio to be initially presented in the user interface.
        // Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
        portfolio.InitialFile = document.EmbeddedFiles.Select(entry => entry.Value).FirstOrDefault(fileSpec => fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal));

        // Hide all existing portfolio fields except 'Size'.
        foreach (System.Collections.Generic.KeyValuePair<PdfName, PdfPortfolioField> portfolioFieldEntry in portfolio.Fields)
        {
            portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name != "Size";
        }

        // Add a new portfolio field with display name 'Full Name' and it should be in the first column.
        System.Collections.Generic.KeyValuePair<PdfName, PdfPortfolioField> portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName");
        PdfPortfolioField portfolioField = portfolioFieldKeyAndValue.Value;
        portfolioField.Name = "Full Name";
        portfolioField.Order = 0;

        // For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
        SetFullNameFieldValue(portfolio.Files, portfolio.Folders, string.Empty, portfolioFieldKeyAndValue.Key);

        document.Save("Portfolio from Streams.pdf");
    }

    static void Example3()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Add to zip archive all files and folders from a PDF portfolio.
        using var document = PdfDocument.Load("Portfolio.pdf");
        using FileStream archiveStream = File.Create("Portfolio Files and Folders.zip");
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true);
        PdfPortfolio portfolio = document.Portfolio;
        if (portfolio != null)
        {
            ExtractFilesAndFoldersToArchive(portfolio.Files, portfolio.Folders, archive, string.Empty, PdfName.Create("FullName"));
        }
    }

    static void SetFullNameFieldValue(PdfPortfolioFileCollection files, PdfPortfolioFolderCollection folders, string parentFolderFullName, PdfName portfolioFieldKey)
    {
        // Set FullName field value for all the fields.
        foreach (PdfFileSpecification fileSpecification in files)
        {
            fileSpecification.PortfolioFieldValues.Add(portfolioFieldKey, new PdfPortfolioFieldValue(parentFolderFullName + fileSpecification.Name));
        }

        foreach (PdfPortfolioFolder folder in folders)
        {
            // Set FullName field value for the folder.
            var folderFullName = parentFolderFullName + folder.Name + '/';
            folder.PortfolioFieldValues.Add(portfolioFieldKey, new PdfPortfolioFieldValue(folderFullName));

            // Recursively set FullName field value for all files and folders underneath the current portfolio folder.
            SetFullNameFieldValue(folder.Files, folder.Folders, folderFullName, portfolioFieldKey);
        }
    }

    static PdfPortfolioFolder GetOrAddFolder(PdfPortfolio portfolio, string fullName)
    {
        var folderNames = fullName.Split('/');

        PdfPortfolioFolder folder = null;
        PdfPortfolioFolderCollection folders = portfolio.Folders;

        // Last name is the name of the file, so it is skipped.
        for (var i = 0; i < folderNames.Length - 1; ++i)
        {
            // Get or add folder with the specific name.
            var folderName = folderNames[i];
            folder = folders.FirstOrDefault(f => f.Name == folderName);
            folder ??= folders.AddEmpty(folderName);

            folders = folder.Folders;
        }

        return folder;
    }

    static void ExtractFilesAndFoldersToArchive(PdfPortfolioFileCollection files, PdfPortfolioFolderCollection folders, ZipArchive archive, string parentFolderFullName, PdfName portfolioFieldKey)
    {
        foreach (PdfFileSpecification fileSpecification in files)
        {
            // Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            var entryFullName = fileSpecification.PortfolioFieldValues.TryGet(portfolioFieldKey, out PdfPortfolioFieldValue fullNameValue)
                ? fullNameValue.ToString()
                : parentFolderFullName + fileSpecification.Name;
            PdfEmbeddedFile embeddedFile = fileSpecification.EmbeddedFile;

            // Create zip archive entry.
            // Zip archive entry is compressed if the portfolio embedded file's compressed size is less than its uncompressed size.
            var compress = embeddedFile.Size == null || embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault();
            ZipArchiveEntry entry = archive.CreateEntry(entryFullName, compress ? CompressionLevel.Optimal : CompressionLevel.NoCompression);

            // Set the modification date, if it is specified in the portfolio embedded file.
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

        foreach (PdfPortfolioFolder folder in folders)
        {
            // Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            var folderFullName = folder.PortfolioFieldValues.TryGet(portfolioFieldKey, out PdfPortfolioFieldValue fullNameValue)
                ? fullNameValue.ToString()
                : parentFolderFullName + folder.Name + '/';

            // Set the modification date, if it is specified in the portfolio folder.
            DateTimeOffset? modificationDate = folder.ModificationDate;
            if (modificationDate.HasValue)
            {
                archive.CreateEntry(folderFullName).LastWriteTime = modificationDate.GetValueOrDefault();
            }

            // Recursively add to zip archive all files and folders underneath the current portfolio folder.
            ExtractFilesAndFoldersToArchive(folder.Files, folder.Folders, archive, folderFullName, portfolioFieldKey);
        }
    }
}
