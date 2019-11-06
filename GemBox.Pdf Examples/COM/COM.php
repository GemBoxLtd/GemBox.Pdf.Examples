// Create ComHelper object and set license. 
// NOTE: If you're using a Professional version you'll need to put your serial key below.
$comHelper = new Com("GemBox.Pdf.ComHelper", null, CP_UTF8);
$comHelper->ComSetLicense("FREE-LIMITED-KEY");

$fileNames = array("\\%#MergeFile01.pdf%", "\\%#MergeFile02.pdf%", "\\%#MergeFile03.pdf%");

// EXAMPLE 1: Load PDF file and read text content from each page.
$document1 = $comHelper->Load(getcwd() . $fileNames[0]);
$pages1 = $document1->Pages;

for ($i1 = 0; $i1 < $pages1->Count; $i1++) {
    $page = $pages1->Item($i1);
    echo $page->Content->ToString() . "<br>";
}

$document1.Dispose();

// EXAMPLE 2: Merge multiple PDF files into a single PDF file.
$document2 = new Com("GemBox.Pdf.PdfDocument", null, CP_UTF8);

foreach ($fileNames as $fileName) {
    $sourceDocument = $comHelper->Load(getcwd() . $fileName);
    $sourcePages = $sourceDocument->Pages;
    
    for ($i2 = 0; $i2 < $sourcePages->Count; $i2++) {
        $document2->Pages->AddClone($sourcePages->Item($i2));
    }
    
    $sourceDocument->Dispose();
}

$comHelper->Save($document2, getcwd() . "\\Merge Files.pdf");
$document2->Dispose();

// EXAMPLE 3: Split a single PDF file into multiple PDF files.
$document3 = $comHelper->Load(getcwd() . "\\Merge Files.pdf");
$pages3 = $document3->Pages;

for ($i3 = 0; $i3 < $pages3->Count; $i3++) {
    $destinationDocument = new Com("GemBox.Pdf.PdfDocument", null, CP_UTF8);
    $destinationDocument->Pages->AddClone($pages3->Item($i3));
    
    $comHelper->Save($destinationDocument, getcwd() . "\\Page" . $i3 . ".pdf");
    $destinationDocument->Dispose();
}

$document3->Dispose();