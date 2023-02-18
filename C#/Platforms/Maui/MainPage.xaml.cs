using GemBox.Pdf;
using GemBox.Pdf.Content;
using System.Reflection;

namespace PdfMaui
{
    public partial class MainPage : ContentPage
    {
        static MainPage()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private async Task<string> CreateDocumentAsync()
        {
            using var document = new PdfDocument();

            for (int i = 0; i < pages.Value; ++i)
                document.Pages.Add();

            using (var formattedText = new PdfFormattedText())
            {
                // Set font family and size.
                // All text appended next uses the specified font family and size.
                formattedText.FontFamily = new PdfFontFamily("Times New Roman");
                formattedText.FontSize = 24;

                formattedText.AppendLine("Hello World");
                document.Pages[0].Content.DrawText(formattedText, new PdfPoint(100, 100));
            }
            
            var image = PdfImage.Load(await FileSystem.OpenAppPackageFileAsync("dices.png"));
            document.Pages[0].Content.DrawImage(image, new PdfPoint(100, 200));

            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Example.pdf");

            document.Save(filePath);

            return filePath;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            button.IsEnabled = false;
            activity.IsRunning = true;

            // In real apps the call to the method should be async (Task.Run(() => ....)
            var filePath = await CreateDocumentAsync();
            await Launcher.OpenAsync(new OpenFileRequest(Path.GetFileName(filePath), new ReadOnlyFile(filePath)));

            activity.IsRunning = false;
            button.IsEnabled = true;
        }
    }
}
