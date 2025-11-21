using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Media
{
    public class EBook(string title, int year, string author, string language, int pages, string isbn) : Media(title, year), IDownloadable, IReadable
    {
        public string Author { get; set; } = author;
        public string Language { get; set; } = language;
        public int Pages { get; set; } = pages;
        public string Isbn { get; set; } = isbn;

        public void Download() => Console.WriteLine($"Downloading the e-book: {Title}...");
        public void Read() => Console.WriteLine($"Reading the e-book: {Title}...");

        public override void DisplayDetails()
        {
            base.DisplayDetails();
            Console.WriteLine($"Author: {Author}");
            Console.WriteLine($"Language: {Language}");
            Console.WriteLine($"Pages: {Pages}");
            Console.WriteLine($"ISBN: {Isbn}");
        }
    }
}