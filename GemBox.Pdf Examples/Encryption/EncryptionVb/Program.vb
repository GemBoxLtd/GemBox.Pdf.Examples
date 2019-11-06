Imports GemBox.Pdf
Imports GemBox.Pdf.Security

Module Program

    Sub Main()

        Example1()

        Example2()

        Example3()

        Example4()
    End Sub

    Sub Example1()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load PDF document from an unencrypted PDF file.
        Using document = PdfDocument.Load("Reading.pdf")

            ' Set password-based encryption with password required to open a PDF document.
            document.SaveOptions.SetPasswordEncryption().DocumentOpenPassword = "user"

            ' Save PDF document to an encrypted PDF file.
            document.Save("Encryption.pdf")
        End Using
    End Sub

    Sub Example2()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Try

            ' Load PDF document from a potentially encrypted PDF file.
            Using document = PdfDocument.Load("Encryption.pdf",
                New PdfLoadOptions() With {.Password = "user"})

                ' Remove encryption from an output PDF file.
                document.SaveOptions.Encryption = Nothing

                'Save PDF document to an unencrypted PDF file.
                document.Save("Decryption.pdf")
            End Using

        Catch ex As InvalidPdfPasswordException

            ' Gracefully handle the case when input PDF file is encrypted
            ' and provided password is invalid.
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Sub Example3()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load PDF document from an unencrypted PDF file.
        Using document = PdfDocument.Load("Reading.pdf")

            ' Set password-based encryption to an output PDF file.
            Dim encryption = document.SaveOptions.SetPasswordEncryption()

            ' Specify password required to edit a PDF document.
            encryption.PermissionsPassword = "owner"

            ' User will be able to print PDF and fill-in PDF form
            ' without requiring a password.
            encryption.Permissions =
                PdfUserAccessPermissions.Print Or
                PdfUserAccessPermissions.FillForm Or
                PdfUserAccessPermissions.CopyContentForAccessibility Or
                PdfUserAccessPermissions.PrintHighResolution

            ' Save PDF document to an encrypted PDF file.
            document.Save("Restrict Editing.pdf")
        End Using
    End Sub

    Sub Example4()

        ' If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        ' Load PDF document from an unencrypted PDF file.
        Using document = PdfDocument.Load("Reading.pdf")

            ' Set password-based encryption to an output PDF file.
            Dim encryption = document.SaveOptions.SetPasswordEncryption()

            ' Specify password required to open a PDF document.
            encryption.DocumentOpenPassword = "user"

            ' Specify password required to edit a PDF document.
            encryption.PermissionsPassword = "owner"

            ' User will be able to print PDF and fill-in PDF form
            ' without requiring a password.
            encryption.Permissions =
                PdfUserAccessPermissions.Print Or
                PdfUserAccessPermissions.FillForm Or
                PdfUserAccessPermissions.CopyContentForAccessibility Or
                PdfUserAccessPermissions.PrintHighResolution

            ' Specify 256-bit AES encryption level (supported in Acrobat X and later).
            encryption.EncryptionLevel = New PdfEncryptionLevel(PdfEncryptionAlgorithm.AES, 256)

            ' Encrypt content and embedded files but do not encrypt document's metadata.
            encryption.Options = PdfEncryptionOptions.EncryptContent Or PdfEncryptionOptions.EncryptEmbeddedFiles

            ' Save PDF document to an encrypted PDF file.
            document.Save("Encryption Settings.pdf")
        End Using
    End Sub
End Module
