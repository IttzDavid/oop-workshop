using oop_workshop.Domain;

namespace oop_workshop.Domain.Users
{
    public class Admin(string name, int age, string cpr) : Employee(name, age, cpr)
    {
        public Borrower CreateBorrower(UserManager userManager, string name, int age, string cpr)
        {
            var b = new Borrower(name, age, cpr);
            userManager.AddBorrower(b);
            return b;
        }

        public Employee CreateEmployee(UserManager userManager, string name, int age, string cpr)
        {
            var e = new Employee(name, age, cpr);
            userManager.AddEmployee(e);
            return e;
        }

        public bool DeleteBorrower(UserManager userManager, string id) => userManager.RemoveBorrower(id);
        public bool DeleteEmployee(UserManager userManager, string id) => userManager.RemoveEmployee(id);

        public bool UpdateUser(UserManager userManager, string id, string? newName = null, int? newAge = null)
        {
            var user = userManager.GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            if (!string.IsNullOrWhiteSpace(newName)) user.Name = newName!;
            if (newAge.HasValue && newAge.Value > 0) user.Age = newAge.Value;
            return true;
        }
    }
}