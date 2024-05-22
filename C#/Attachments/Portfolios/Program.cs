using GemBox.Pdf;
using GemBox.Pdf.Objects;
using GemBox.Pdf.Portfolios;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

class Program
{
    static void Main()
    {
        CreatePortfolioFromFileSystem();

        CreatePortfolioFromStreams();

        ExtractFilesAndFoldersFromPortfolio();
    }

    static void CreatePortfolioFromFileSystem()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("PortfolioTemplate.pdf"))
        {
            // Make the document a PDF portfolio (a collection of file attachments).
            var portfolio = document.SetPortfolio();

            // Extract all the files in the zip archive to a directory on the file system.
            ZipFile.ExtractToDirectory("Attachments.zip", "Attachments");

            // Add files contained directly in the 'Attachments' directory to the portfolio files.
            foreach (var filePath in Directory.GetFiles("Attachments", "*", SearchOption.TopDirectoryOnly))
                portfolio.Files.Add(filePath);

            // Recursively add directories and their files contained in the 'Attachments' directory to the portfolio folders.
            foreach (var folderPath in Directory.GetDirectories("Attachments", "*", SearchOption.TopDirectoryOnly))
                portfolio.Folders.Add(folderPath, recursive: true);

            // Delete the directory where zip archive files were extracted to.
            Directory.Delete("Attachments", recursive: true);

            // Set the first PDF file contained in the portfolio to be initially presented in the user interface.
            // Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
            portfolio.InitialFile = document.EmbeddedFiles.Select(entry => entry.Value).FirstOrDefault(fileSpec => fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal));

            // Hide all existing portfolio fields except 'Size'.
            foreach (var portfolioFieldEntry in portfolio.Fields)
                portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name != "Size";

            // Add a new portfolio field with display name 'Full Name' and it should be in the first column.
            var portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName");
            var portfolioField = portfolioFieldKeyAndValue.Value;
            portfolioField.Name = "Full Name";
            portfolioField.Order = 0;

            // For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
            SetFullNameFieldValue(portfolio.Files, portfolio.Folders, string.Empty, portfolioFieldKeyAndValue.Key);

            document.Save("Portfolio from file system.pdf");
        }
    }

    static void CreatePortfolioFromStreams()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("PortfolioTemplate.pdf"))
        {
            // Make the document a PDF portfolio (a collection of file attachments).
            var portfolio = document.SetPortfolio();

            // Add all files and folders from the zip archive to the portfolio.
            using (var archiveStream = File.OpenRead("Attachments.zip"))
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen: true))
                foreach (var entry in archive.Entries)
                {
                    // Get or create portfolio folder hierarchy from the zip entry full name.
                    var folder = GetOrAddFolder(portfolio, entry.FullName);

                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        // Zip archive entry is a file.
                        var files = folder == null ? portfolio.Files : folder.Files;

                        var embeddedFile = files.AddEmpty(entry.Name).EmbeddedFile;

                        // Set the portfolio file size and modification date.
                        if (entry.Length < int.MaxValue)
                            embeddedFile.Size = (int)entry.Length;
                        embeddedFile.ModificationDate = entry.LastWriteTime;

                        // Copy portfolio file contents from the zip archive entry.
                        // Portfolio file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                        using (var entryStream = entry.Open())
                        using (var embeddedFileStream = embeddedFile.OpenWrite(compress: entry.CompressedLength < entry.Length))
                            entryStream.CopyTo(embeddedFileStream);
                    }
                    else
                        // Zip archive entry is a folder.
                        // Set the portfolio folder modification date.
                        folder.ModificationDate = entry.LastWriteTime;
                }

            // Set the first PDF file contained in the portfolio to be initially presented in the user interface.
            // Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
            portfolio.InitialFile = document.EmbeddedFiles.Select(entry => entry.Value).FirstOrDefault(fileSpec => fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal));

            // Hide all existing portfolio fields except 'Size'.
            foreach (var portfolioFieldEntry in portfolio.Fields)
                portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name != "Size";

            // Add a new portfolio field with display name 'Full Name' and it should be in the first column.
            var portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName");
            var portfolioField = portfolioFieldKeyAndValue.Value;
            portfolioField.Name = "Full Name";
            portfolioField.Order = 0;

            // For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
            SetFullNameFieldValue(portfolio.Files, portfolio.Folders, string.Empty, portfolioFieldKeyAndValue.Key);

            document.Save("Portfolio from Streams.pdf");
        }
    }

    static void ExtractFilesAndFoldersFromPortfolio()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        // Add to zip archive all files and folders from a PDF portfolio.
        using (var document = PdfDocument.Load("Portfolio.pdf"))
        using (var archiveStream = File.Create("Portfolio Files and Folders.zip"))
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            var portfolio = document.Portfolio;
            if (portfolio != null)
                ExtractFilesAndFoldersToArchive(portfolio.Files, portfolio.Folders, archive, string.Empty, PdfName.Create("FullName"));
        }
    }

    static void SetFullNameFieldValue(PdfPortfolioFileCollection files, PdfPortfolioFolderCollection folders, string parentFolderFullName, PdfName portfolioFieldKey)
    {
        // Set FullName field value for all the fields.
        foreach (var fileSpecification in files)
            fileSpecification.PortfolioFieldValues.Add(portfolioFieldKey, new PdfPortfolioFieldValue(parentFolderFullName + fileSpecification.Name));

        foreach (var folder in folders)
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
        var folders = portfolio.Folders;

        // Last name is the name of the file, so it is skipped.
        for (int i = 0; i < folderNames.Length - 1; ++i)
        {
            // Get or add folder with the specific name.
            var folderName = folderNames[i];
            folder = folders.FirstOrDefault(f => f.Name == folderName);
            if (folder == null)
                folder = folders.AddEmpty(folderName);

            folders = folder.Folders;
        }

        return folder;
    }

    static void ExtractFilesAndFoldersToArchive(PdfPortfolioFileCollection files, PdfPortfolioFolderCollection folders, ZipArchive archive, string parentFolderFullName, PdfName portfolioFieldKey)
    {
        foreach (var fileSpecification in files)
        {
            // Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            string entryFullName;
            if (fileSpecification.PortfolioFieldValues.TryGet(portfolioFieldKey, out PdfPortfolioFieldValue fullNameValue))
                entryFullName = fullNameValue.ToString();
            else
                entryFullName = parentFolderFullName + fileSpecification.Name;

            var embeddedFile = fileSpecification.EmbeddedFile;

            // Create zip archive entry.
            // Zip archive entry is compressed if the portfolio embedded file's compressed size is less than its uncompressed size.
            bool compress = embeddedFile.Size == null || embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault();
            var entry = archive.CreateEntry(entryFullName, compress ? CompressionLevel.Optimal : CompressionLevel.NoCompression);

            // Set the modification date, if it is specified in the portfolio embedded file.
            var modificationDate = embeddedFile.ModificationDate;
            if (modificationDate != null)
                entry.LastWriteTime = modificationDate.GetValueOrDefault();

            // Copy embedded file contents to the zip archive entry.
            using (var embeddedFileStream = embeddedFile.OpenRead())
            using (var entryStream = entry.Open())
                embeddedFileStream.CopyTo(entryStream);
        }

        foreach (var folder in folders)
        {
            // Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            string folderFullName;
            if (folder.PortfolioFieldValues.TryGet(portfolioFieldKey, out PdfPortfolioFieldValue fullNameValue))
                folderFullName = fullNameValue.ToString();
            else
                folderFullName = parentFolderFullName + folder.Name + '/';

            // Set the modification date, if it is specified in the portfolio folder.
            var modificationDate = folder.ModificationDate;
            if (modificationDate.HasValue)
                archive.CreateEntry(folderFullName).LastWriteTime = modificationDate.GetValueOrDefault();

            // Recursively add to zip archive all files and folders underneath the current portfolio folder.
            ExtractFilesAndFoldersToArchive(folder.Files, folder.Folders, archive, folderFullName, portfolioFieldKey);
        }
    }
}
