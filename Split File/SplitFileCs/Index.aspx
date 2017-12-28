<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SplitFileCs.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Split PDF file in C# and VB.NET</title>
</head>
<body>
    <p>
        <br />
        Upload PDF file:</p>
    <form id="form1" runat="server">
        <p>
            <asp:FileUpload ID="pdfFileUpload" runat="server" Width="317px" />
        </p>
        <p>
            <asp:Button ID="generateZipButton" runat="server" OnClick="generateZipButton_Click" Text="Generate ZIP file" Width="128px" />
        </p>
        <div>
        </div>
    </form>
</body>
</html>
