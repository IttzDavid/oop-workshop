using oop_workshop.Domain.Interfaces;
using oop_workshop.Domain.Medias;

namespace oop_workshop.Domain.Users
{
    public class Borrower : User
    {
        private readonly List<Media> _currentlyBorrowed = new();
        public IReadOnlyCollection<Media> CurrentlyBorrowed => _currentlyBorrowed.AsReadOnly();

        public Borrower(string name, int age, string cpr) : base(name, age, cpr) { }

        public void BorrowMedia(Media media, DateTime dueDate)
        {
            if (media is not IBorrowable borrowable)
                throw new InvalidOperationException("Media is not borrowable.");

            if (borrowable.IsBorrowed)
                throw new InvalidOperationException("Media already borrowed.");

            borrowable.Borrow(Id, dueDate);
            _currentlyBorrowed.Add(media);
        }

        public void ReturnMedia(Media media)
        {
            if (media is not IBorrowable borrowable)
                throw new InvalidOperationException("Media is not borrowable.");

            if (!borrowable.IsBorrowed || borrowable.BorrowedById != Id)
                throw new InvalidOperationException("You cannot return media you did not borrow.");

            borrowable.Return(Id);
            _currentlyBorrowed.Remove(media);
        }

        public void RateMedia(Media media, int score)
        {
            if (media is not IRatable ratable)
                throw new InvalidOperationException("Media is not ratable.");

            ratable.Rate(Id, score);
        }
    }
}