namespace oop_workshop.Domain.Interfaces
{
    public interface IBorrowable
    {
        bool IsBorrowed { get; }
        DateTime? DueDate { get; }
        string? BorrowedById { get; }
        void Borrow(string borrowerId, DateTime dueDate);
        void Return(string borrowerId);
    }
}