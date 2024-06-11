<?php
  // Create ComHelper object.
  $comHelper = new Com("GemBox.Pdf.ComHelper", null, CP_UTF8);
  // If using the Professional version, put your serial key below.
  $comHelper->ComSetLicense("FREE-LIMITED-KEY");

  // Load PDF file.
  $document = $comHelper->Load(getcwd() . "\\LoremIpsum.pdf");
  $pages = $document->Pages;

  // Split a single PDF file into multiple PDF files.
  for ($i = 0; $i < $pages->Count; $i++) {
      $destinationDocument = new Com("GemBox.Pdf.PdfDocument", null, CP_UTF8);
      $destinationDocument->Pages->AddClone($pages->Item($i));

      $comHelper->Save($destinationDocument, getcwd() . "\\LoremIpsum" . $i . ".pdf");
      $destinationDocument->Dispose();
  }

  $document->Dispose();
?>