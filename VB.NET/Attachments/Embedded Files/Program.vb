Imports System
Imports System.IO
Imports System.IO.Compression
Imports GemBox.Pdf

Module Program

    Sub Main()

        CreateEmbeddedFilesFromFileSystem()

        CreateEmbeddedFilesFromStreams()

        ExtractEmbeddedFiles()
    End Sub

    Sub CreateEmbeddedFilesFromFileSystem()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Make Attachments panel visible.
            document.PageMode = PdfPageMode.UseAttachments

            ' Extract all the files in the zip archive to a directory on the file system.
            ZipFile.ExtractToDirectory("Attachments.zip", "Attachments")

            ' Embed in the PDF document all the files extracted from the zip archive.
            For Each filePath In Directory.GetFiles("Attachments", "*", SearchOption.AllDirectories)

                Dim fileSpecification = document.EmbeddedFiles.Add(filePath).Value

                ' Set embedded file description to the relative path of the file in the zip archive.
                fileSpecification.Description = filePath.Substring(filePath.IndexOf("\"c) + 1).Replace("\"c, "/"c)
            Next

            ' Delete the directory where zip archive files were extracted to.
            Directory.Delete("Attachments", recursive:=True)

            document.Save("Embedded Files from file system.pdf")
        End Using
    End Sub

    Sub CreateEmbeddedFilesFromStreams()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Using document = PdfDocument.Load("Reading.pdf")

            ' Make Attachments panel visible.
            document.PageMode = PdfPageMode.UseAttachments

            ' Embed in the PDF document all the files from the zip archive.
            Using archiveStream = File.OpenRead("Attachments.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Read, leaveOpen:=True)
                    For Each entry In archive.Entries
                        If Not String.IsNullOrEmpty(entry.Name) Then

                            Dim fileSpecification = document.EmbeddedFiles.AddEmpty(entry.Name).Value

                            ' Set embedded file description to the relative path of the file in the zip archive.
                            fileSpecification.Description = entry.FullName

                            Dim embeddedFile = fileSpecification.EmbeddedFile

                            ' Set the embedded file size and modification date.
                            If entry.Length < Integer.MaxValue Then embeddedFile.Size = CInt(entry.Length)
                            embeddedFile.ModificationDate = entry.LastWriteTime

                            ' Copy embedded file contents from the zip archive entry.
                            ' Embedded file is compressed if its compressed size in the zip archive is less than its uncompressed size.
                            Using entryStream = entry.Open()
                                Using embeddedFileStream = embeddedFile.OpenWrite(compress:=entry.CompressedLength < entry.Length)
                                    entryStream.CopyTo(embeddedFileStream)
                                End Using
                            End Using
                        End If
                    Next
                End Using
            End Using

            document.Save("Embedded Files from Streams.pdf")
        End Using
    End Sub

    Sub ExtractEmbeddedFiles()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Add to zip archive all files embedded in the PDF document.
        Using document = PdfDocument.Load("Embedded Files.pdf")
            Using archiveStream = File.Create("Embedded Files.zip")
                Using archive = New ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen:=True)
                    For Each keyFilePair In document.EmbeddedFiles

                        Dim fileSpecification = keyFilePair.Value

                        ' Use the description or the name as the relative path of the entry in the zip archive.
                        Dim entryFullName = fileSpecification.Description
                        If entryFullName Is Nothing OrElse Not entryFullName.EndsWith(fileSpecification.Name, StringComparison.Ordinal) Then entryFullName = fileSpecification.Name

                        Dim embeddedFile = fileSpecification.EmbeddedFile

                        ' Create zip archive entry.
                        ' Zip archive entry is compressed if the embedded file's compressed size is less than its uncompressed size.
                        Dim compress As Boolean = embeddedFile.Size Is Nothing OrElse embeddedFile.CompressedSize < embeddedFile.Size.GetValueOrDefault()
                        Dim entry = archive.CreateEntry(entryFullName, If(compress, CompressionLevel.Optimal, CompressionLevel.NoCompression))

                        ' Set the modification date, if it is specified in the embedded file.
                        Dim modificationDate = embeddedFile.ModificationDate
                        If modificationDate IsNot Nothing Then entry.LastWriteTime = modificationDate.GetValueOrDefault()

                        ' Copy embedded file contents to the zip archive entry.
                        Using embeddedFileStream = embeddedFile.OpenRead()
                            Using entryStream = entry.Open()
                                embeddedFileStream.CopyTo(entryStream)
                            End Using
                        End Using
                    Next
                End Using
            End Using
        End Using
    End Sub
End Module
