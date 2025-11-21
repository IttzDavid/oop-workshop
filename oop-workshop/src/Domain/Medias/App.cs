using oop_workshop.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace oop_workshop.Domain.Medias
{
    public class App(string title, int year, string version, string publisher, IEnumerable<string> supportedPlatforms, double fileSize)
        : Media(title, year), IDownloadable, IExecutable
    {
        public string Version { get; set; } = version;
        public string Publisher { get; set; } = publisher;
        public List<string> SupportedPlatforms { get; set; } = supportedPlatforms.ToList();
        public double FileSize { get; set; } = fileSize;

        public void Download() => Console.WriteLine($"Downloading the app: {Title}...");
        public void Execute() => Console.WriteLine($"Executing the app: {Title} v{Version}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Publisher: {Publisher}");
            Console.WriteLine($"Version: {Version}");
            Console.WriteLine($"Supported Platforms: {string.Join(", ", SupportedPlatforms)}");
            Console.WriteLine($"File Size: {FileSize} MB");
        }
    }
}