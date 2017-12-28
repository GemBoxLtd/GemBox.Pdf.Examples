<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MergeFilesCs.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Merge PDF files in C# and VB.NET</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            Upload ZIP file with PDF files that need to be merged:<br />
            <br />
            <asp:FileUpload ID="zipFileUpload" runat="server" Width="314px" />
            <br />
            <br />
            <asp:Button ID="generatePdfButton" runat="server" OnClick="generatePdfButton_Click" Text="Generate PDF file" Width="126px" />
            <br />
        </div>
    </form>
</body>
</html>
