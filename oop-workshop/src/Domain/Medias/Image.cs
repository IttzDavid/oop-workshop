using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Medias
{
    public class Image(string title, int year, string resolution, string fileFormat, double fileSize, DateTime dateTaken)
        : Media(title, year), IDownloadable, IViewable
    {
        public string Resolution { get; set; } = resolution;
        public string FileFormat { get; set; } = fileFormat;
        public double FileSize { get; set; } = fileSize;
        public DateTime DateTaken { get; set; } = dateTaken;

        public void Download() => Console.WriteLine($"Downloading the image: {Title}...");
        public void View() => Console.WriteLine($"Displaying the image: {Title}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Resolution: {Resolution}");
            Console.WriteLine($"File Format: {FileFormat}");
            Console.WriteLine($"File Size: {FileSize} MB");
            Console.WriteLine($"Date Taken: {DateTaken:yyyy-MM-dd}");
        }
    }
}