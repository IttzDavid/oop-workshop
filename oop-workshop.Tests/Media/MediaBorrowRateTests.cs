using System;
using FluentAssertions;
using oop_workshop.Domain.Medias;
using oop_workshop.Domain.Users;
using Xunit;

namespace oop_workshop.Tests.Media
{
    public class MediaBorrowRateTests
    {
        [Fact]
        public void Borrow_ShouldSetState()
        {
            var ebook = new EBook("Title", 2020, "Author", "EN", 100, "ISBN");
            var borrower = new Borrower("User", 20, "000000-0000");

            borrower.BorrowMedia(ebook, DateTime.Today.AddDays(7));

            ebook.IsBorrowed.Should().BeTrue();
            ebook.BorrowedById.Should().Be(borrower.Id);
            ebook.DueDate.Should().NotBeNull();
            borrower.CurrentlyBorrowed.Should().Contain(ebook);
        }

        [Fact]
        public void BorrowingAlreadyBorrowed_ShouldThrow()
        {
            var ebook = new EBook("Title", 2020, "Author", "EN", 100, "ISBN");
            var b1 = new Borrower("User1", 20, "000000-0000");
            var b2 = new Borrower("User2", 21, "111111-1111");

            b1.BorrowMedia(ebook, DateTime.Today.AddDays(7));
            Action act = () => b2.BorrowMedia(ebook, DateTime.Today.AddDays(7));

            act.Should().Throw<InvalidOperationException>().WithMessage("Media already borrowed.");
        }

        [Fact]
        public void Return_ShouldClearState()
        {
            var ebook = new EBook("Title", 2020, "Author", "EN", 100, "ISBN");
            var borrower = new Borrower("User", 20, "000000-0000");
            borrower.BorrowMedia(ebook, DateTime.Today.AddDays(7));

            borrower.ReturnMedia(ebook);

            ebook.IsBorrowed.Should().BeFalse();
            ebook.BorrowedById.Should().BeNull();
            ebook.DueDate.Should().BeNull();
            borrower.CurrentlyBorrowed.Should().BeEmpty();
        }

        [Fact]
        public void Rate_BeforeBorrow_ShouldThrow()
        {
            var ebook = new EBook("Title", 2020, "Author", "EN", 100, "ISBN");
            var borrower = new Borrower("User", 20, "000000-0000");

            Action act = () => borrower.RateMedia(ebook, 5);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Only a user who has borrowed this item may rate it.");
        }

        [Fact]
        public void Rate_AfterBorrow_ShouldStoreRatingAndComputeAverage()
        {
            var ebook = new EBook("Title", 2020, "Author", "EN", 100, "ISBN");
            var borrower = new Borrower("User", 20, "000000-0000");
            borrower.BorrowMedia(ebook, DateTime.Today.AddDays(7));
            borrower.RateMedia(ebook, 4);

            ebook.Ratings.Should().ContainKey(borrower.Id).WhoseValue.Should().Be(4);
            ebook.AverageRating.Should().Be(4);

            borrower.RateMedia(ebook, 5); // overwrite
            ebook.Ratings[borrower.Id].Should().Be(5);
            ebook.AverageRating.Should().Be(5);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public void Rate_InvalidScore_ShouldThrow(int invalidScore)
        {
            var ebook = new EBook("Title", 2020, "Author", "EN", 100, "ISBN");
            var borrower = new Borrower("User", 20, "000000-0000");
            borrower.BorrowMedia(ebook, DateTime.Today.AddDays(7));

            Action act = () => borrower.RateMedia(ebook, invalidScore);
            act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("*Rating must be between 1 and 5.*");
        }
    }
}