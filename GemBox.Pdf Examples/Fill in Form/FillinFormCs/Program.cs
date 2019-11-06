using System.Linq;
using GemBox.Pdf;
using GemBox.Pdf.Forms;

class Program
{
    static void Main()
    {
        // If using Professional version, put your serial key below.
        ComponentInfo.SetLicense("FREE-LIMITED-KEY");

        using (var document = PdfDocument.Load("Form.pdf"))
        {
            // Set check box field checked state.
            var marriedCheckBox = (PdfCheckBoxField)document.Form.Fields["Married"];
            marriedCheckBox.Checked = true;

            // Set check box field value.
            var drivingLicenseCheckBox = (PdfCheckBoxField)document.Form.Fields["Driving License"];
            drivingLicenseCheckBox.Value = "Yes";

            // Set radio button field checked state.
            // There are multiple radio button fields with name 'Gender' so
            // set checked state only to the radio button field whose choice is 'Male'.
            foreach (PdfRadioButtonField genderRadioButton in document.Form.Fields.Where(field => field.Name == "Gender"))
                genderRadioButton.Checked = genderRadioButton.Choice == "Male";

            // Set radio button field value.
            // It is enough to set value to only one radio button field 
            // from a group of radio button fields with name 'Age'.
            var ageRadioButton = (PdfRadioButtonField)document.Form.Fields["Age"];
            ageRadioButton.Value = "18 - 39";

            document.Save("Fill in Form.pdf");
        }
    }
}
