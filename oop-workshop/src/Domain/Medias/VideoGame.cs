using oop_workshop.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace oop_workshop.Domain.Medias
{
    public class VideoGame(string title, int year, string genre, string publisher, IEnumerable<string> supportedPlatforms)
        : Media(title, year), IDownloadable, IPlayable
    {
        public string Genre { get; set; } = genre;
        public string Publisher { get; set; } = publisher;
        public List<string> SupportedPlatforms { get; set; } = supportedPlatforms.ToList();
        public bool IsCompleted { get; private set; }

        public void Download() => Console.WriteLine($"Downloading the game: {Title}...");
        public void Play() => Console.WriteLine($"Launching game: {Title}...");
        public void MarkCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                Console.WriteLine($"{Title} marked as completed.");
            }
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Publisher: {Publisher}");
            Console.WriteLine($"Supported Platforms: {string.Join(", ", SupportedPlatforms)}");
            Console.WriteLine($"Completed: {IsCompleted}");
        }
    }
}