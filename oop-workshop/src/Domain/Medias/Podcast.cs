using oop_workshop.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace oop_workshop.Domain.Medias
{
    public class Podcast(string title, int year, IEnumerable<string> hosts, IEnumerable<string> guests, int episodeNumber, string language, int durationSeconds)
        : Media(title, year), IDownloadable, IPlayable
    {
        public List<string> Hosts { get; set; } = hosts.ToList();
        public List<string> Guests { get; set; } = guests.ToList();
        public int EpisodeNumber { get; set; } = episodeNumber;
        public string Language { get; set; } = language;
        public int DurationSeconds { get; set; } = durationSeconds;
        public bool IsCompleted { get; private set; }

        public void Download() => Console.WriteLine($"Downloading episode {EpisodeNumber} of {Title}...");
        public void Play() => Console.WriteLine($"Playing podcast episode {EpisodeNumber}: {Title}...");
        public void MarkCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                Console.WriteLine($"Podcast {Title} episode {EpisodeNumber} marked completed.");
            }
        }

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Episode: {EpisodeNumber}");
            Console.WriteLine($"Hosts: {string.Join(", ", Hosts)}");
            Console.WriteLine($"Guests: {string.Join(", ", Guests)}");
            Console.WriteLine($"Language: {Language}");
            Console.WriteLine($"Duration: {DurationSeconds} seconds");
            Console.WriteLine($"Completed: {IsCompleted}");
        }
    }
}