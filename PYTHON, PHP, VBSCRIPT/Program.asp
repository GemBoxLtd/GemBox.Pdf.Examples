<%
  ' Create ComHelper object.
  Set comHelper = Server.CreateObject("GemBox.Pdf.ComHelper")
  ' If using the Professional version, put your serial key below.
  comHelper.SetLicense("FREE-LIMITED-KEY")

  Dim fileNames : fileNames = Array("\MergeFile01.pdf", "\MergeFile02.pdf", "\MergeFile03.pdf")

  ''''''''''''''''
  ''' Read PDF '''
  ''''''''''''''''

  ' Load PDF file.
  Set document1 = comHelper.Load(Server.MapPath(".") & fileNames(0))
  Set pages1 = document1.Pages

  ' Read text content from each PDF page.
  For i1 = 0 To pages1.Count - 1
      Set page = pages1.Item(i1)
      Response.write(page.Content.ToString() & "<br>")
  Next

  document1.Dispose()

  '''''''''''''''''
  ''' Merge PDF '''
  '''''''''''''''''

  ' Create PdfDocument object.
  Set document2 = Server.CreateObject("GemBox.Pdf.PdfDocument")

  ' Merge multiple PDF files into a single PDF file.
  For i2 = 0 To UBound(fileNames)
      Set sourceDocument = comHelper.Load(Server.MapPath(".") & fileNames(i2))
      Set sourcePages = sourceDocument.Pages

      For j2 = 0 To sourcePages.Count - 1
          document2.Pages.AddClone(sourcePages.Item(j2))
      Next

      sourceDocument.Dispose()
  Next

  comHelper.Save document2, Server.MapPath(".") & "\Merge Files.pdf"
  document2.Dispose()

  '''''''''''''''''
  ''' Split PDF '''
  '''''''''''''''''

  ' Load PDF file.
  Set document3 = comHelper.Load(Server.MapPath(".") & "\Merge Files.pdf")
  Set pages3 = document3.Pages

  ' Split a single PDF file into multiple PDF files.
  For i3 = 0 To pages3.Count - 1
      Set destinationDocument = Server.CreateObject("GemBox.Pdf.PdfDocument")
      destinationDocument.Pages.AddClone(pages3.Item(i3))

      comHelper.Save destinationDocument, Server.MapPath(".") & "\Page" & i3 & ".pdf"
      destinationDocument.Dispose()
  Next

  document3.Dispose()
%>