using System;
using System.Globalization;
using System.IO;
using System.Linq;
using GemBox.Pdf;
using GemBox.Pdf.Forms;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        var writer = new StringWriter(CultureInfo.InvariantCulture);
        string format = "{0,-16}|{1,20}|{2,-20}|{3,-20}|{4,-20}", separator = new string('-', 100);

        // Write header.
        writer.WriteLine("Document contains the following form fields:");
        writer.WriteLine();
        writer.WriteLine(format,
            "Type",
            '"' + "Name" + '"',
            "Value",
            "ExportValue/Choice",
            "Checked/Selected");
        writer.WriteLine(separator);

        PdfFieldType? fieldType;
        string fieldName, fieldExportValueOrChoice;
        object fieldValue;
        bool? fieldCheckedOrSelected;

        using (var document = PdfDocument.Load("FormFilled.pdf"))
            // Group fields by name because all fields with the same name are actually different representations (widget annotations) of the same field.
            // Radio button fields are usually grouped. Other field types are rarely grouped.
            foreach (var fieldGroup in document.Form.Fields.GroupBy(field => field.Name))
            {
                var field = fieldGroup.First();

                fieldType = field.FieldType;
                fieldName = '"' + field.Name + '"';
                fieldValue = field.Value;

                foreach (var widgetField in fieldGroup)
                {
                    switch (widgetField.FieldType)
                    {
                        case PdfFieldType.CheckBox:
                        case PdfFieldType.RadioButton:
                            // Check box and radio button are toggle button fields.
                            var toggleField = (PdfToggleButtonField)widgetField;

                            fieldExportValueOrChoice = toggleField.FieldType == PdfFieldType.CheckBox ?
                                    ((PdfCheckBoxField)toggleField).ExportValue :
                                    ((PdfRadioButtonField)toggleField).Choice;
                            fieldCheckedOrSelected = toggleField.Checked;

                            writer.WriteLine(format,
                                fieldType,
                                fieldName,
                                fieldValue,
                                fieldExportValueOrChoice,
                                fieldCheckedOrSelected);
                            break;

                        case PdfFieldType.ListBox:
                        case PdfFieldType.Dropdown:
                            // List box and drop-down are choice fields.
                            var choiceField = (PdfChoiceField)widgetField;

                            // List box can have multiple values if multiple selection is enabled.
                            if (fieldValue is string[] fieldValues)
                                fieldValue = string.Join(", ", fieldValues);

                            for (int itemIndex = 0; itemIndex < choiceField.Items.Count; ++itemIndex)
                            {
                                fieldExportValueOrChoice = choiceField.Items[itemIndex].ExportValue ?? choiceField.Items[itemIndex].Value;
                                fieldCheckedOrSelected = choiceField.FieldType == PdfFieldType.ListBox ?
                                        ((PdfListBoxField)choiceField).SelectedIndices.Contains(itemIndex) :
                                        ((PdfDropdownField)choiceField).SelectedIndex == itemIndex;

                                writer.WriteLine(format,
                                fieldType,
                                fieldName,
                                fieldValue,
                                fieldExportValueOrChoice,
                                fieldCheckedOrSelected);

                                // Write field type, field name and field value just once for a field group.
                                fieldType = null;
                                fieldName = null;
                                fieldValue = null;
                            }
                            break;

                        default:
                            // Text field may contain multiple lines of text, if enabled.
                            if (widgetField.FieldType == PdfFieldType.Text && ((PdfTextField)widgetField).MultiLine && fieldValue != null)
                                fieldValue = ((string)fieldValue).Replace("\r", "\\r");
                            
                            fieldExportValueOrChoice = null;
                            fieldCheckedOrSelected = null;

                            writer.WriteLine(format,
                                fieldType,
                                fieldName,
                                fieldValue,
                                fieldExportValueOrChoice,
                                fieldCheckedOrSelected);
                            break;
                    }

                    // Write field type, field name and field value just once for a field group.
                    fieldType = null;
                    fieldName = null;
                    fieldValue = null;
                }

                writer.WriteLine(separator);
            }

        Console.Write(writer.ToString());
    }
}
