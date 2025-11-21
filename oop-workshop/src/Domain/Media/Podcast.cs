using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Media
{
    public class Podcast(string title, int year, string hosts, string guests, int episode, string language) : Media(title, year), IDownloadable, IPlayable, ICompletable
    {
        public string Hosts { get; set; } = hosts;
        public string Guests { get; set; } = guests;
        public int Episode { get; set; } = episode;
        public string Language { get; set; } = language;

        public void Download() => Console.WriteLine($"Downloading podcast episode: {Title}...");
        public void Play() => Console.WriteLine($"Listening to podcast: {Title} - Episode {Episode}...");
        public void Complete() => Console.WriteLine($"You have finished listening to podcast episode: {Title}.");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Episode: {Episode}");
            Console.WriteLine($"Host(s): {Hosts}");
            Console.WriteLine($"Guest(s): {Guests}");
            Console.WriteLine($"Language: {Language}");
        }
    }
}