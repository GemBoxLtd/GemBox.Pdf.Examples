import os
import win32com.client as COM

# Create ComHelper object.
comHelper = COM.Dispatch("GemBox.Pdf.ComHelper")
# If using the Professional version, put your serial key below.
comHelper.ComSetLicense("FREE-LIMITED-KEY")

fileNames = ["\\MergeFile01.pdf", "\\MergeFile02.pdf", "\\MergeFile03.pdf"]

################
### Read PDF ###
################

# Load PDF file.
document1 = comHelper.Load(os.getcwd() + fileNames[0])
pages1 = document1.Pages

# Read text content from each PDF page.
for i1 in range(pages1.Count):
    page = pages1.Item(i1)
    print(page.Content.ToString() + "\n")

document1.Dispose()

#################
### Merge PDF ###
#################

# Create PdfDocument object.
document2 = COM.Dispatch("GemBox.Pdf.PdfDocument")

# Merge multiple PDF files into a single PDF file.
for fileName in fileNames:
    sourceDocument = comHelper.Load(os.getcwd() + fileName)
    sourcePages = sourceDocument.Pages

    for i2 in range(sourcePages.Count):
        document2.Pages.AddClone(sourcePages.Item(i2))

    sourceDocument.Dispose()

comHelper.Save(document2, os.getcwd() + "\\Merge Files.pdf")
document2.Dispose()

#################
### Split PDF ###
#################

# Load PDF file.
document3 = comHelper.Load(os.getcwd() + "\\Merge Files.pdf")
pages3 = document3.Pages

# Split a single PDF file into multiple PDF files.
for i3 in range(pages3.Count):
    destinationDocument = COM.Dispatch("GemBox.Pdf.PdfDocument")
    destinationDocument.Pages.AddClone(pages3.Item(i3))
    
    comHelper.Save(destinationDocument, os.getcwd() + "\\Page" + str(i3) + ".pdf")
    destinationDocument.Dispose()

document3.Dispose()