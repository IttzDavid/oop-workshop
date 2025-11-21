using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Media
{
    public class VideoGame(string title, int year, string genre, string publisher, string platform) : Media(title, year), IDownloadable, IPlayable, ICompletable
    {
        public string Genre { get; set; } = genre;
        public string Publisher { get; set; } = publisher;
        public string Platform { get; set; } = platform;

        public void Download() => Console.WriteLine($"Downloading the game: {Title}...");
        public void Play() => Console.WriteLine($"Playing the game: {Title} on {Platform}...");
        public void Complete() => Console.WriteLine($"You have completed the game: {Title}!");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Publisher: {Publisher}");
            Console.WriteLine($"Platform: {Platform}");
        }
    }
}