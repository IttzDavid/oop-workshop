using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Media
{
    public class Image(string title, int year, string resolution, string format, double fileSize, string dateTaken) : Media(title, year), IDownloadable, IViewable
    {
        public string Resolution { get; set; } = resolution;
        public string Format { get; set; } = format;
        public double FileSize { get; set; } = fileSize;
        public string DateTaken { get; set; } = dateTaken;

        public void Download() => Console.WriteLine($"Downloading the image: {Title}...");
        public void View() => Console.WriteLine($"Displaying the image: {Title}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Resolution: {Resolution}");
            Console.WriteLine($"Format: {Format}");
            Console.WriteLine($"File Size: {FileSize} MB");
            Console.WriteLine($"Date Taken: {DateTaken}");
        }
    }
}