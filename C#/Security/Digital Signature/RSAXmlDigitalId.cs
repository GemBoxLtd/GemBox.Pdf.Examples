using GemBox.Pdf.Security;

/// <summary>
/// Represents a digital ID that reads an RSA private key from an XML file.
/// </summary>
class RSAXmlDigitalId : PdfDigitalId
{
    private readonly string privateKeyXmlString;

    public RSAXmlDigitalId(string privateKeyXmlFileName, string certificateFileName) : base(new PdfCertificate(certificateFileName))
    {
        this.privateKeyXmlString = System.IO.File.ReadAllText(privateKeyXmlFileName);
    }

    protected override byte[] SignHash(byte[] hash, PdfHashAlgorithm hashAlgorithm, PdfRSASignaturePadding rsaSignaturePadding)
    {
        using (var rsa = System.Security.Cryptography.RSA.Create())
        {
            rsa.FromXmlString(this.privateKeyXmlString);

            return rsa.SignHash(
                hash,
                new System.Security.Cryptography.HashAlgorithmName(hashAlgorithm.ToString()),
                rsaSignaturePadding == PdfRSASignaturePadding.Pss ? System.Security.Cryptography.RSASignaturePadding.Pss : System.Security.Cryptography.RSASignaturePadding.Pkcs1);
        }
    }
}
