Imports System
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Objects
Imports GemBox.Pdf.Portfolios

Module Program

    Sub Main()

        CreatePortfolioFromFileSystem()

        CreatePortfolioFromStreams()

        ExtractFilesAndFoldersFromPortfolio()
    End Sub

    Sub CreatePortfolioFromFileSystem()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("PortfolioTemplate.pdf")

            ' Make the document a PDF portfolio (a collection of file attachments).
            Dim portfolio = document.SetPortfolio()

            ' Extract all the files in the zip archive to a directory on the file system.
            ZipFile.ExtractToDirectory("Attachments.zip", "Attachments")

            ' Add files contained directly in the 'Attachments' directory to the portfolio files.
            For Each filePath In Directory.GetFiles("Attachments", "*", SearchOption.TopDirectoryOnly)
                portfolio.Files.Add(filePath)
            Next

            ' Recursively add directories and their files contained in the 'Attachments' directory to the portfolio folders.
            For Each folderPath In Directory.GetDirectories("Attachments", "*", SearchOption.TopDirectoryOnly)
                portfolio.Folders.Add(folderPath, recursive:=True)
            Next

            ' Delete the directory where zip archive files were extracted to.
            Directory.Delete("Attachments", recursive:=True)

            ' Set the first PDF file contained in the portfolio to be initially presented in the user interface.
            ' Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
            portfolio.InitialFile = document.EmbeddedFiles.Select(Function(entry) entry.Value).FirstOrDefault(Function(fileSpec) fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal))

            ' Hide all existing portfolio fields except 'Size'.
            For Each portfolioFieldEntry In portfolio.Fields
                portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name <> "Size"
            Next

            ' Add a new portfolio field with display name 'Full Name' and it should be in the first column.
            Dim portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName")
            Dim portfolioField = portfolioFieldKeyAndValue.Value
            portfolioField.Name = "Full Name"
            portfolioField.Order = 0

            ' For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
            SetFullNameFieldValue(portfolio.Files, portfolio.Folders, String.Empty, portfolioFieldKeyAndValue.Key)

            document.Save("Portfolio from file system.pdf")
        End Using
    End Sub

    Sub CreatePortfolioFromStreams()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("PortfolioTemplate.pdf")

            ' Make the document a PDF portfolio (a collection of file attachments).
            Dim portfolio = document.SetPortfolio()

            ' Add all files and folders from the zip archive to the portfolio.
            Using archiveStream = File.OpenRead("Attachments.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen:=True)
                    For Each entry In archive.Entries

                        ' Get or create portfolio folder hierarchy from the zip entry full name.
                        Dim folder = GetOrAddFolder(portfolio, entry.FullName)

                        If Not String.IsNullOrEmpty(entry.Name) Then

                            ' Zip archive entry is a file.
                            Dim files = If(folder Is Nothing, portfolio.Files, folder.Files)

                            Dim embeddedFile = files.AddEmpty(entry.Name).EmbeddedFile

                            ' Set the portfolio file size and modification date.
                            If entry.Length < Integer.MaxValue Then embeddedFile.Size = CInt(entry.Length)
                            embeddedFile.ModificationDate = entry.LastWriteTime

                            ' Copy portfolio file contents from the zip archive entry.
                            ' Portfolio file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                            Using entryStream = entry.Open()
                                Using embeddedFileStream = embeddedFile.OpenWrite(compress:=entry.CompressedLength < entry.Length)
                                    entryStream.CopyTo(embeddedFileStream)
                                End Using
                            End Using
                        Else
                            ' Zip archive entry is a folder.
                            ' Set the portfolio folder modification date.
                            folder.ModificationDate = entry.LastWriteTime
                        End If
                    Next
                End Using
            End Using

            ' Set the first PDF file contained in the portfolio to be initially presented in the user interface.
            ' Note that all files contained in the portfolio are also contained in the PdfDocument.EmbeddedFiles.
            portfolio.InitialFile = document.EmbeddedFiles.Select(Function(entry) entry.Value).FirstOrDefault(Function(fileSpec) fileSpec.Name.EndsWith(".pdf", StringComparison.Ordinal))

            ' Hide all existing portfolio fields except 'Size'.
            For Each portfolioFieldEntry In portfolio.Fields
                portfolioFieldEntry.Value.Hidden = portfolioFieldEntry.Value.Name <> "Size"
            Next

            ' Add a new portfolio field with display name 'Full Name' and it should be in the first column.
            Dim portfolioFieldKeyAndValue = portfolio.Fields.Add(PdfPortfolioFieldDataType.String, "FullName")
            Dim portfolioField = portfolioFieldKeyAndValue.Value
            portfolioField.Name = "Full Name"
            portfolioField.Order = 0

            ' For each file and folder in the portfolio, set FullName field value to the relative path of the file/folder in the zip archive.
            SetFullNameFieldValue(portfolio.Files, portfolio.Folders, String.Empty, portfolioFieldKeyAndValue.Key)

            document.Save("Portfolio from Streams.pdf")
        End Using
    End Sub

    Sub ExtractFilesAndFoldersFromPortfolio()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Add to zip archive all files and folders from a PDF portfolio.
        Using document = PdfDocument.Load("Portfolio.pdf")
            Using archiveStream = File.Create("Portfolio Files and Folders.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen:=True)

                    Dim portfolio = document.Portfolio
                    If portfolio IsNot Nothing Then ExtractFilesAndFoldersToArchive(portfolio.Files, portfolio.Folders, archive, String.Empty, PdfName.Create("FullName"))
                End Using
            End Using
        End Using
    End Sub

    Sub SetFullNameFieldValue(ByVal files As PdfPortfolioFileCollection, ByVal folders As PdfPortfolioFolderCollection, ByVal parentFolderFullName As String, ByVal portfolioFieldKey As PdfName)

        ' Set FullName field value for all the fields.
        For Each fileSpecification In files
            fileSpecification.PortfolioFieldValues.Add(portfolioFieldKey, New PdfPortfolioFieldValue(parentFolderFullName & fileSpecification.Name))
        Next

        For Each folder In folders

            ' Set FullName field value for the folder.
            Dim folderFullName = parentFolderFullName & folder.Name + "/"c
            folder.PortfolioFieldValues.Add(portfolioFieldKey, New PdfPortfolioFieldValue(folderFullName))

            ' Recursively set FullName field value for all files and folders underneath the current portfolio folder.
            SetFullNameFieldValue(folder.Files, folder.Folders, folderFullName, portfolioFieldKey)
        Next
    End Sub

    Function GetOrAddFolder(ByVal portfolio As PdfPortfolio, ByVal fullName As String) As PdfPortfolioFolder

        Dim folderNames = fullName.Split("/"c)

        Dim folder As PdfPortfolioFolder = Nothing
        Dim folders = portfolio.Folders

        ' Last name is the name of the file, so it is skipped.
        For i As Integer = 0 To folderNames.Length - 1 - 1

            ' Get or add folder with the specific name.
            Dim folderName = folderNames(i)
            folder = folders.FirstOrDefault(Function(f) f.Name = folderName)
            If folder Is Nothing Then folder = folders.AddEmpty(folderName)

            folders = folder.Folders
        Next

        Return folder
    End Function

    Sub ExtractFilesAndFoldersToArchive(ByVal files As PdfPortfolioFileCollection, ByVal folders As PdfPortfolioFolderCollection, ByVal archive As ZipArchive, ByVal parentFolderFullName As String, ByVal portfolioFieldKey As PdfName)

        For Each fileSpecification In files

            ' Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            Dim entryFullName As String
            Dim fullNameValue As PdfPortfolioFieldValue
            If fileSpecification.PortfolioFieldValues.TryGet(portfolioFieldKey, fullNameValue) Then
                entryFullName = fullNameValue.ToString()
            Else
                entryFullName = parentFolderFullName & fileSpecification.Name
            End If

            Dim embeddedFile = fileSpecification.EmbeddedFile

            ' Create zip archive entry.
            ' Zip archive entry is compressed if the portfolio embedded file's compressed size is less than its uncompressed size.
            Dim compress As Boolean = embeddedFile.Size Is Nothing OrElse embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault()
            Dim entry = archive.CreateEntry(entryFullName, If(compress, CompressionLevel.Optimal, CompressionLevel.NoCompression))

            ' Set the modification date, if it is specified in the portfolio embedded file.
            Dim modificationDate = embeddedFile.ModificationDate
            If modificationDate IsNot Nothing Then entry.LastWriteTime = modificationDate.GetValueOrDefault()

            ' Copy embedded file contents to the zip archive entry.
            Using embeddedFileStream = embeddedFile.OpenRead()
                Using entryStream = entry.Open()
                    embeddedFileStream.CopyTo(entryStream)
                End Using
            End Using
        Next

        For Each folder In folders

            ' Use the FullName field value or the resolved full name as the relative path of the entry in the zip archive.
            Dim folderFullName As String
            Dim fullNameValue As PdfPortfolioFieldValue
            If folder.PortfolioFieldValues.TryGet(portfolioFieldKey, fullNameValue) Then
                folderFullName = fullNameValue.ToString()
            Else
                folderFullName = parentFolderFullName & folder.Name + "/"c
            End If

            ' Set the modification date, if it is specified in the portfolio folder.
            Dim modificationDate = folder.ModificationDate
            If modificationDate.HasValue Then archive.CreateEntry(folderFullName).LastWriteTime = modificationDate.GetValueOrDefault()

            ' Recursively add to zip archive all files and folders underneath the current portfolio folder.
            ExtractFilesAndFoldersToArchive(folder.Files, folder.Folders, archive, folderFullName, portfolioFieldKey)
        Next
    End Sub
End Module
