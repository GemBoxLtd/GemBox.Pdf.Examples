<?php
  // Create ComHelper object.
  $comHelper = new Com("GemBox.Pdf.ComHelper", null, CP_UTF8);
  // If using the Professional version, put your serial key below.
  $comHelper->ComSetLicense("FREE-LIMITED-KEY");

  $fileNames = array("\\MergeFile01.pdf", "\\MergeFile02.pdf", "\\MergeFile03.pdf");

  // Create PdfDocument object.
  $document = new Com("GemBox.Pdf.PdfDocument", null, CP_UTF8);

  // Merge multiple PDF files into a single PDF file.
  foreach ($fileNames as $fileName) {
      $sourceDocument = $comHelper->Load(getcwd() . $fileName);
      $sourcePages = $sourceDocument->Pages;

      for ($i = 0; $i < $sourcePages->Count; $i++) {
          $document->Pages->AddClone($sourcePages->Item($i));
      }

      $sourceDocument->Dispose();
  }

  $comHelper->Save($document, getcwd() . "\\MergedFile.pdf");
  $document->Dispose();
?>