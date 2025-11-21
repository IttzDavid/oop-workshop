using System.Linq;
using FluentAssertions;
using oop_workshop.Domain.Users;
using Xunit;

namespace oop_workshop.Tests.Users
{
    public class UserManagerTests
    {
        [Fact]
        public void AddUsers_ShouldAppearInCollections()
        {
            var mgr = new UserManager();
            var admin = new Admin("Alice", 40, "000000-0000");
            var employee = new Employee("Bob", 30, "111111-1111");
            var borrower = new Borrower("Charlie", 20, "222222-2222");

            mgr.AddAdmin(admin);
            mgr.AddEmployee(employee);
            mgr.AddBorrower(borrower);

            mgr.Admins.Should().ContainSingle(a => a.Name == "Alice");
            mgr.Employees.Should().ContainSingle(e => e.Name == "Bob");
            mgr.Borrowers.Should().ContainSingle(b => b.Name == "Charlie");
            mgr.GetAllUsers().Should().HaveCount(3);
        }

        [Fact]
        public void RemoveEmployee_ShouldReturnTrue_WhenExists()
        {
            var mgr = new UserManager();
            var e = new Employee("Bob", 30, "111111-1111");
            mgr.AddEmployee(e);

            mgr.RemoveEmployee(e.Id).Should().BeTrue();
            mgr.Employees.Should().BeEmpty();
        }

        [Fact]
        public void RemoveBorrower_ShouldReturnFalse_WhenNotFound()
        {
            var mgr = new UserManager();
            mgr.RemoveBorrower("999").Should().BeFalse();
        }

        [Fact]
        public void FindUserById_ShouldReturnUser()
        {
            var mgr = new UserManager();
            var b = new Borrower("Charlie", 20, "222222-2222");
            mgr.AddBorrower(b);

            mgr.FindUserById(b.Id).Should().NotBeNull().And.BeSameAs(b);
        }

        [Fact]
        public void UserIds_ShouldIncrementSequentially()
        {
            var a = new Admin("A1", 50, "000000-0000");
            var b = new Employee("E1", 40, "111111-1111");
            var c = new Borrower("B1", 30, "222222-2222");

            int.Parse(a.Id).Should().BeLessThan(int.Parse(b.Id));
            int.Parse(b.Id).Should().BeLessThan(int.Parse(c.Id));
        }
    }
}