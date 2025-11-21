using oop_workshop.Domain.Interfaces;

namespace oop_workshop.Domain.Medias
{
    public abstract class Media : IBorrowable, IRatable
    {
        public string Title { get; set; }
        public int Year { get; set; }

        // Borrow state
        private readonly HashSet<string> _borrowerHistory = new();
        public bool IsBorrowed { get; private set; }
        public DateTime? DueDate { get; private set; }
        public string? BorrowedById { get; private set; }

        // Ratings
        private readonly Dictionary<string, int> _ratings = new();
        public IReadOnlyDictionary<string, int> Ratings => _ratings;
        public double AverageRating => _ratings.Count == 0 ? 0 : _ratings.Values.Average();

        protected Media(string title, int year)
        {
            Title = title;
            Year = year;
        }

        // Borrowing logic
        public void Borrow(string borrowerId, DateTime dueDate)
        {
            if (IsBorrowed)
                throw new InvalidOperationException("Media already borrowed.");

            IsBorrowed = true;
            BorrowedById = borrowerId;
            DueDate = dueDate;
            _borrowerHistory.Add(borrowerId);
        }

        public void Return(string borrowerId)
        {
            if (!IsBorrowed)
                throw new InvalidOperationException("Media is not currently borrowed.");
            if (BorrowedById != borrowerId)
                throw new InvalidOperationException("This borrower did not borrow the media.");

            IsBorrowed = false;
            BorrowedById = null;
            DueDate = null;
        }

        // Rating logic (only users who have borrowed at least once)
        public void Rate(string borrowerId, int score)
        {
            if (!_borrowerHistory.Contains(borrowerId))
                throw new InvalidOperationException("Only a user who has borrowed this item may rate it.");

            if (score < 1 || score > 5)
                throw new ArgumentOutOfRangeException(nameof(score), "Rating must be between 1 and 5.");

            _ratings[borrowerId] = score; // Overwrite previous rating from same borrower
        }

        // Persistence restore helpers
        internal void RestoreBorrowState(string? borrowerId, DateTime? dueDate)
        {
            if (!string.IsNullOrWhiteSpace(borrowerId) && dueDate.HasValue)
            {
                IsBorrowed = true;
                BorrowedById = borrowerId;
                DueDate = dueDate;
                _borrowerHistory.Add(borrowerId);
            }
        }

        internal void RestoreRatings(IEnumerable<(string borrowerId, int score)> ratings)
        {
            foreach (var (borrowerId, score) in ratings)
            {
                if (string.IsNullOrWhiteSpace(borrowerId)) continue;
                _borrowerHistory.Add(borrowerId);
                _ratings[borrowerId] = score;
            }
        }

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine($"Borrowed: {IsBorrowed}");
            if (IsBorrowed)
            {
                Console.WriteLine($"Due Date: {DueDate:yyyy-MM-dd}");
                Console.WriteLine($"Borrowed By: {BorrowedById}");
            }
            Console.WriteLine($"Average Rating: {AverageRating:F2} ({_ratings.Count} rating(s))");
        }
    }
}