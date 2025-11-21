using System.IO;
using System.Linq;
using System;
using oop_workshop.Domain.Users;
using oop_workshop.Domain.Medias;
using oop_workshop.Persistence;
using Xunit;

namespace oop_workshop.tests;

public class BasicPersistenceTests
{
    [Fact]
    public void UsersCsv_SaveLoad_RoundTrip()
    {
        var temp = Path.Combine(Path.GetTempPath(), "oop_ws_simple_users");
        Directory.CreateDirectory(temp);
        var file = Path.Combine(temp, "users.csv");
        if (File.Exists(file)) File.Delete(file);

        var mgr = new UserManager();
        var a = new Admin("Admin", 40, "000000-0000");
        var e = new Employee("Emp", 30, "111111-1111");
        var b = new Borrower("Borrower", 20, "222222-2222");
        mgr.AddAdmin(a); mgr.AddEmployee(e); mgr.AddBorrower(b);

        UsersCsvPersistence.SaveUsers(file, mgr);
        var loaded = UsersCsvPersistence.LoadUsers(file);

        Assert.Equal(3, loaded.GetAllUsers().Count());
        Assert.Contains(loaded.Admins, x => x.Name == "Admin");
    }

    [Fact]
    public void MediaCsv_SaveLoad_PreservesBorrowState()
    {
        var temp = Path.Combine(Path.GetTempPath(), "oop_ws_simple_media");
        Directory.CreateDirectory(temp);
        var file = Path.Combine(temp, "media.csv");
        if (File.Exists(file)) File.Delete(file);

        var borrower = new Borrower("Borrower", 20, "222222-2222");
        var ebook = new EBook("Persist", 2022, "Auth", "EN", 50, "ISBN1");
        borrower.BorrowMedia(ebook, DateTime.Today.AddDays(5));
        borrower.RateMedia(ebook, 4);

        CsvLoader.SaveMedia(file, new[] { ebook });
        var loaded = CsvLoader.LoadMedia(file);

        var loadedEBook = loaded.OfType<EBook>().Single();
        Assert.True(loadedEBook.IsBorrowed);
        Assert.Equal(ebook.BorrowedById, loadedEBook.BorrowedById);
        Assert.Equal(4, loadedEBook.Ratings[borrower.Id]);
    }
}