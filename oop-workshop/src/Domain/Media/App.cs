using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Media
{
    public class App(string title, int year, string version, string publisher, string platform, double fileSize) : Media(title, year), IDownloadable, IExecutable
    {
        public string Version { get; set; } = version;
        public string Publisher { get; set; } = publisher;
        public string Platform { get; set; } = platform;
        public double FileSize { get; set; } = fileSize;

        public void Download() => Console.WriteLine($"Downloading the app: {Title}...");
        public void Execute() => Console.WriteLine($"Executing the app: {Title} v{Version}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Publisher: {Publisher}");
            Console.WriteLine($"Version: {Version}");
            Console.WriteLine($"Platform: {Platform}");
            Console.WriteLine($"File Size: {FileSize} MB");
        }
    }
}