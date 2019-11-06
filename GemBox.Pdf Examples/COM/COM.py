# Create ComHelper object and set license.
# NOTE: If you're using a Professional version you'll need to put your serial key below.
import win32com.client as COM
comHelper = COM.Dispatch("GemBox.Pdf.ComHelper")
comHelper.ComSetLicense("FREE-LIMITED-KEY")

fileNames = ["\\%#MergeFile01.pdf%", "\\%#MergeFile02.pdf%", "\\%#MergeFile03.pdf%"]

# EXAMPLE 1: Load PDF file and read text content from each page.
import os
document1 = comHelper.Load(os.getcwd() + fileNames[0])
pages1 = document1.Pages

for i1 in range(pages1.Count):
    page = pages1.Item(i1)
    print(page.Content.ToString() + "\n")

document1.Dispose()

# EXAMPLE 2: Merge multiple PDF files into a single PDF file.
document2 = COM.Dispatch("GemBox.Pdf.PdfDocument")

for fileName in fileNames:
    sourceDocument = comHelper.Load(os.getcwd() + fileName)
    sourcePages = sourceDocument.Pages

    for i2 in range(sourcePages.Count):
        document2.Pages.AddClone(sourcePages.Item(i2))

    sourceDocument.Dispose()

comHelper.Save(document2, os.getcwd() + "\\Merge Files.pdf")
document2.Dispose()

# EXAMPLE 3: Split a single PDF file into multiple PDF files.
document3 = comHelper.Load(os.getcwd() + "\\Merge Files.pdf")
pages3 = document3.Pages

for i3 in range(pages3.Count):
    destinationDocument = COM.Dispatch("GemBox.Pdf.PdfDocument")
    destinationDocument.Pages.AddClone(pages3.Item(i3))
    
    comHelper.Save(destinationDocument, os.getcwd() + "\\Page" + str(i3) + ".pdf")
    destinationDocument.Dispose()

document3.Dispose()