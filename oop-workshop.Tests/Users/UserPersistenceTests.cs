using System.IO;
using FluentAssertions;
using oop_workshop.Domain.Users;
using oop_workshop.Persistence;
using Xunit;

namespace oop_workshop.Tests.Users
{
    public class UserPersistenceTests
    {
        [Fact]
        public void SaveAndLoadUsers_ShouldRoundTripAndPreserveIds()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "oop_workshop_tests_users");
            Directory.CreateDirectory(tempDir);
            var file = Path.Combine(tempDir, "users.csv");
            if (File.Exists(file)) File.Delete(file);

            var mgr = new UserManager();
            var admin = new Admin("Admin1", 45, "000000-0000");
            var emp = new Employee("Emp1", 33, "111111-1111");
            var bor = new Borrower("Borrower1", 22, "222222-2222");
            mgr.AddAdmin(admin);
            mgr.AddEmployee(emp);
            mgr.AddBorrower(bor);

            UsersCsvPersistence.SaveUsers(file, mgr);

            var loaded = UsersCsvPersistence.LoadUsers(file);
            loaded.GetAllUsers().Should().HaveCount(3);

            loaded.Admins.Should().ContainSingle(a => a.Name == "Admin1");
            loaded.Employees.Should().ContainSingle(e => e.Name == "Emp1");
            loaded.Borrowers.Should().ContainSingle(b => b.Name == "Borrower1");

            // IDs preserved
            loaded.GetAllUsers().Select(u => u.Id)
                .Should().BeEquivalentTo(mgr.GetAllUsers().Select(u => u.Id));

            // Next ID should advance (create new user)
            var newBorrower = new Borrower("NewB", 25, "333333-3333");
            int.Parse(newBorrower.Id).Should().BeGreaterThan(int.Parse(loaded.GetAllUsers().Max(u => u.Id)));
        }
    }
}