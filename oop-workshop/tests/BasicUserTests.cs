using oop_workshop.Domain.Users;
using Xunit;

namespace oop_workshop.tests;

public class BasicUserTests
{
    [Fact]
    public void AddAdmin_ShouldIncreaseAdminCount()
    {
        var mgr = new UserManager();
        mgr.AddAdmin(new Admin("Alice", 40, "000000-0000"));
        Assert.Single(mgr.Admins);
    }

    [Fact]
    public void RemoveBorrower_NonExisting_ShouldReturnFalse()
    {
        var mgr = new UserManager();
        var result = mgr.RemoveBorrower("999");
        Assert.False(result);
    }
}