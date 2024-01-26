using System;
using GemBox.Pdf;
using GemBox.Pdf.Forms;

class Program
{
    static void Main()
    {
        // If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        Example1();
        Example2();
    }

    static void Example1()
    {
        using (var document = PdfDocument.Load("FormFilled.pdf"))
        {
            Console.WriteLine(" Field Name      | Field Type      | Field Value ");
            Console.WriteLine(new string('-', 50));

            foreach (var field in document.Form.Fields)
            {
                string value = (field.Value ?? string.Empty).ToString().Replace("\r", ", ");
                Console.WriteLine($" {field.Name,-15} | {field.FieldType,-15} | {value}");
            }
        }
    }

    static void Example2()
    {
        using (var document = PdfDocument.Load("FormFilled.pdf"))
        {
            Console.WriteLine(" Field Name                         | Field Data ");
            Console.WriteLine(new string('-', 50));

            foreach (var field in document.Form.Fields)
            {
                switch (field.FieldType)
                {
                    case PdfFieldType.RadioButton:
                        var radioButton = (PdfRadioButtonField)field;
                        Console.Write($" {radioButton.Name,12} [PdfRadioButtonField] | ");
                        Console.WriteLine($"{radioButton.Choice} [{(radioButton.Checked ? "Checked" : "Unchecked")}]");
                        break;

                    case PdfFieldType.CheckBox:
                        var checkBox = (PdfCheckBoxField)field;
                        Console.Write($" {checkBox.Name,15} [PdfCheckBoxField] | ");
                        Console.WriteLine($"{checkBox.ExportValue} [{(checkBox.Checked ? "Checked" : "Unchecked")}]");
                        break;

                    case PdfFieldType.Dropdown:
                        var dropdown = (PdfDropdownField)field;
                        Console.Write($" {dropdown.Name,15} [PdfDropdownField] | ");
                        Console.WriteLine(dropdown.SelectedItem);
                        break;

                    case PdfFieldType.ListBox:
                        var listBox = (PdfListBoxField)field;
                        Console.Write($" {listBox.Name,16} [PdfListBoxField] | ");
                        Console.WriteLine(string.Join(", ", listBox.SelectedItems));
                        break;
                }
            }
        }
    }
}
