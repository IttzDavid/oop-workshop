namespace oop_workshop.Domain.Users
{
    public class UserManager
    {
        private readonly List<Admin> _admins = new();
        private readonly List<Employee> _employees = new();
        private readonly List<Borrower> _borrowers = new();

        public IReadOnlyCollection<Admin> Admins => _admins;
        public IReadOnlyCollection<Employee> Employees => _employees;
        public IReadOnlyCollection<Borrower> Borrowers => _borrowers;

        public IEnumerable<User> GetAllUsers() => _admins.Cast<User>()
            .Concat(_employees)
            .Concat(_borrowers);

        public void AddAdmin(Admin admin) => _admins.Add(admin);
        public void AddEmployee(Employee employee) => _employees.Add(employee);
        public void AddBorrower(Borrower borrower) => _borrowers.Add(borrower);

        public bool RemoveEmployee(string id)
        {
            var e = _employees.FirstOrDefault(x => x.Id == id);
            if (e == null) return false;
            _employees.Remove(e);
            return true;
        }

        public bool RemoveBorrower(string id)
        {
            var b = _borrowers.FirstOrDefault(x => x.Id == id);
            if (b == null) return false;
            _borrowers.Remove(b);
            return true;
        }

        public User? FindUserById(string id) => GetAllUsers().FirstOrDefault(u => u.Id == id);
    }
}