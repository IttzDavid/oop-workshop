        using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Media
{
    public class Song(string title, int year, string composer, string singer, string genre, string fileType, int duration, string language) : Media(title, year), IDownloadable, IPlayable
    {
        public string Composer { get; set; } = composer;
        public string Singer { get; set; } = singer;
        public string Genre { get; set; } = genre;
        public string FileType { get; set; } = fileType;
        public int Duration { get; set; } = duration;
        public string Language { get; set; } = language;

        public void Download() => Console.WriteLine($"Downloading the song: {Title}...");
        public void Play() => Console.WriteLine($"Playing the song: {Title} by {Singer}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Singer: {Singer}");
            Console.WriteLine($"Composer: {Composer}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Language: {Language}");
            Console.WriteLine($"Duration: {Duration} seconds");
            Console.WriteLine($"File Type: {FileType}");
        }
    }
}