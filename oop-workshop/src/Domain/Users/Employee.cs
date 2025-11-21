using oop_workshop.Domain;
using oop_workshop.Domain.Medias;

namespace oop_workshop.Domain.Users
{
    public class Employee(string name, int age, string cpr) : User(name, age, cpr)
    {
        public void AddMedia(Library library, Media media) => library.AddMedia(media);
        public bool RemoveMedia(Library library, string title) => library.RemoveMedia(title);
    }
}