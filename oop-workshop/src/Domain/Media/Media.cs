using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Media
{
    public abstract class Media
    {
        public string Title { get; set; }
        public int Year { get; set; }

        protected Media(string title, int year)
        {
            Title = title;
            Year = year;
        }

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Year: {Year}");
        }
    }
}