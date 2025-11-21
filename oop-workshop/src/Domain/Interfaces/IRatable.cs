    namespace oop_workshop.Domain.Interfaces
{
    public interface IRatable
    {
        double AverageRating { get; }
        IReadOnlyDictionary<string, int> Ratings { get; }
        void Rate(string borrowerId, int score);
    }
}