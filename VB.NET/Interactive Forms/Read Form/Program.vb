Imports System
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports GemBox.Pdf
Imports GemBox.Pdf.Forms

Module Program

    Sub Main()

        ' If using the Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY")

        Dim writer = New StringWriter(CultureInfo.InvariantCulture)
        Dim format As String = "{0,-16}|{1,20}|{2,-20}|{3,-20}|{4,-20}", separator As String = New String("-"c, 100)

        ' Write the header.
        writer.WriteLine("Document contains the following form fields:")
        writer.WriteLine()
        writer.WriteLine(format,
            "Type",
            """"c & "Name" & """"c,
            "Value",
            "ExportValue/Choice",
            "Checked/Selected")
        writer.WriteLine(separator)

        Dim fieldType As PdfFieldType?
        Dim fieldName, fieldExportValueOrChoice As String
        Dim fieldValue As Object
        Dim fieldCheckedOrSelected As Boolean?

        Using document = PdfDocument.Load("FormFilled.pdf")
            ' Group fields by name because all fields with the same name are actually different representations (widget annotations) of the same field.
            ' Radio button fields are usually grouped. Other field types are rarely grouped.
            For Each fieldGroup In document.Form.Fields.GroupBy(Function(field) field.Name)

                Dim field = fieldGroup.First()

                fieldType = field.FieldType
                fieldName = """"c + field.Name + """"c
                fieldValue = field.Value

                For Each widgetField In fieldGroup

                    Select Case widgetField.FieldType

                        Case PdfFieldType.CheckBox,
                             PdfFieldType.RadioButton
                            ' Check box and radio button are toggle button fields.
                            Dim toggleField = CType(widgetField, PdfToggleButtonField)

                            fieldExportValueOrChoice = If(toggleField.FieldType = PdfFieldType.CheckBox,
                                CType(toggleField, PdfCheckBoxField).ExportValue,
                                CType(toggleField, PdfRadioButtonField).Choice)
                            fieldCheckedOrSelected = toggleField.Checked

                            writer.WriteLine(format,
                                fieldType,
                                fieldName,
                                fieldValue,
                                fieldExportValueOrChoice,
                                fieldCheckedOrSelected)


                        Case PdfFieldType.ListBox,
                             PdfFieldType.Dropdown
                            ' List box and drop-down are choice fields.
                            Dim choiceField = CType(widgetField, PdfChoiceField)

                            ' List box can have multiple values if multiple selection is enabled.
                            Dim fieldValues = TryCast(fieldValue, String())
                            If fieldValues IsNot Nothing Then fieldValue = String.Join(", ", fieldValues)

                            For itemIndex As Integer = 0 To choiceField.Items.Count - 1

                                fieldExportValueOrChoice = If(choiceField.Items(itemIndex).ExportValue, choiceField.Items(itemIndex).Value)
                                fieldCheckedOrSelected = If(choiceField.FieldType = PdfFieldType.ListBox,
                                    CType(choiceField, PdfListBoxField).SelectedIndices.Contains(itemIndex),
                                    CType(choiceField, PdfDropdownField).SelectedIndex = itemIndex)

                                writer.WriteLine(format,
                                    fieldType,
                                    fieldName,
                                    fieldValue,
                                    fieldExportValueOrChoice,
                                    fieldCheckedOrSelected)

                                ' Write field type, field name and field value just once for a field group.
                                fieldType = Nothing
                                fieldName = Nothing
                                fieldValue = Nothing
                            Next


                        Case Else
                            ' Text field may contain multiple lines of text, if enabled.
                            If widgetField.FieldType = PdfFieldType.Text AndAlso (CType(widgetField, PdfTextField)).MultiLine AndAlso fieldValue IsNot Nothing Then fieldValue = (CStr(fieldValue)).Replace(vbCr, "\r")

                            fieldExportValueOrChoice = Nothing
                            fieldCheckedOrSelected = Nothing

                            writer.WriteLine(format,
                                fieldType,
                                fieldName,
                                fieldValue,
                                fieldExportValueOrChoice,
                                fieldCheckedOrSelected)

                    End Select

                    ' Write field type, field name and field value just once for a field group.
                    fieldType = Nothing
                    fieldName = Nothing
                    fieldValue = Nothing
                Next

                writer.WriteLine(separator)
            Next
        End Using

        Console.Write(writer.ToString())
    End Sub
End Module
