using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Medias
{
    public class Movie(string title, int year, string director, string genre, string language, int duration)
        : Media(title, year), IDownloadable, IViewable
    {
        public string Director { get; set; } = director;
        public string Genre { get; set; } = genre;
        public string Language { get; set; } = language;
        public int Duration { get; set; } = duration;

        public void Download() => Console.WriteLine($"Downloading the movie: {Title}...");
        public void View() => Console.WriteLine($"Watching the movie: {Title}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Director: {Director}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Language: {Language}");
            Console.WriteLine($"Duration: {Duration} minutes");
        }
    }
}