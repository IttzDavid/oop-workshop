using System;
using oop_workshop.Domain.Users;
using oop_workshop.Domain.Medias;
using Xunit;

namespace oop_workshop.tests;

public class BasicBorrowRateTests
{
    [Fact]
    public void BorrowMedia_ShouldSetIsBorrowed()
    {
        var borrower = new Borrower("Bob", 25, "111111-1111");
        var ebook = new EBook("Title", 2023, "Author", "EN", 100, "ISBN");
        borrower.BorrowMedia(ebook, DateTime.Today.AddDays(7));
        Assert.True(ebook.IsBorrowed);
        Assert.Equal(borrower.Id, ebook.BorrowedById);
    }

    [Fact]
    public void RateWithoutBorrow_ShouldThrow()
    {
        var borrower = new Borrower("Bob", 25, "111111-1111");
        var ebook = new EBook("Title", 2023, "Author", "EN", 100, "ISBN");
        Assert.Throws<InvalidOperationException>(() => borrower.RateMedia(ebook, 5));
    }
}