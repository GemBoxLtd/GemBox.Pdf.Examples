using System.Linq;

namespace BlazorServerApp.Data
{
    public class FileModel
    {
        public string Header { get; set; } = "Header text from Blazor Server App";
        public string Body { get; set; } = string.Join(" ", Enumerable.Repeat("Lorem ipsum dolor sit amet, consectetuer adipiscing elit.", 4));
        public string Footer { get; set; } = "Page 1 of 1";
    }
}