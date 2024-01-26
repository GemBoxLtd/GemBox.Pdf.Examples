Imports System
Imports GemBox.Pdf
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Example1()
        Example2()

    End Sub

    Sub Example1()
        Using document = PdfDocument.Load("FormFilled.pdf")
            Console.WriteLine(" Field Name      | Field Type      | Field Value ")
            Console.WriteLine(New String("-"c, 50))

            For Each field In document.Form.Fields
                Dim value As String = (If(field.Value, String.Empty)).ToString().Replace(vbCr, ", ")
                Console.WriteLine($" {field.Name,-15} | {field.FieldType,-15} | {value}")
            Next
        End Using
    End Sub

    Sub Example2()
        Using document = PdfDocument.Load("FormFilled.pdf")
            Console.WriteLine(" Field Name                         | Field Data ")
            Console.WriteLine(New String("-"c, 50))

            For Each field In document.Form.Fields

                Select Case field.FieldType

                    Case PdfFieldType.RadioButton
                        Dim radioButton = CType(field, PdfRadioButtonField)
                        Console.Write($" {radioButton.Name,12} [PdfRadioButtonField] | ")
                        Console.WriteLine($"{radioButton.Choice} [{If(radioButton.Checked, "Checked", "Unchecked")}]")

                    Case PdfFieldType.CheckBox
                        Dim checkBox = CType(field, PdfCheckBoxField)
                        Console.Write($" {checkBox.Name,15} [PdfCheckBoxField] | ")
                        Console.WriteLine($"{checkBox.ExportValue} [{If(checkBox.Checked, "Checked", "Unchecked")}]")

                    Case PdfFieldType.Dropdown
                        Dim dropdown = CType(field, PdfDropdownField)
                        Console.Write($" {dropdown.Name,15} [PdfDropdownField] | ")
                        Console.WriteLine(dropdown.SelectedItem)

                    Case PdfFieldType.ListBox
                        Dim listBox = CType(field, PdfListBoxField)
                        Console.Write($" {listBox.Name,16} [PdfListBoxField] | ")
                        Console.WriteLine(String.Join(", ", listBox.SelectedItems))

                End Select

            Next
        End Using
    End Sub

End Module
