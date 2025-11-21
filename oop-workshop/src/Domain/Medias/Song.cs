using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Medias
{
    public class Song(string title, int year, string composer, string singer, string genre, string fileType, int durationSeconds, string language)
        : Media(title, year), IDownloadable, IPlayable
    {
        public string Composer { get; set; } = composer;
        public string Singer { get; set; } = singer;
        public string Genre { get; set; } = genre;
        public string FileType { get; set; } = fileType;
        public int DurationSeconds { get; set; } = durationSeconds;
        public string Language { get; set; } = language;

        public void Download() => Console.WriteLine($"Downloading the song: {Title}...");
        public void Play() => Console.WriteLine($"Playing song: {Title} by {Singer}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Composer: {Composer}");
            Console.WriteLine($"Singer: {Singer}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"File Type: {FileType}");
            Console.WriteLine($"Language: {Language}");
            Console.WriteLine($"Duration: {DurationSeconds} seconds");
        }
    }
}