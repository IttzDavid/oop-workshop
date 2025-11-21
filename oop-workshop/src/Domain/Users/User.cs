namespace oop_workshop.Domain.Users
{
    public abstract class User
    {
        // Internal technical identifier (not CPR) for relations
        public string Id { get; } = Guid.NewGuid().ToString("N");

        public string Name { get; set; }
        public int Age { get; set; }
        public string Cpr { get; set; } // Sensitive; placeholder only

        protected User(string name, int age, string cpr)
        {
            Name = name;
            Age = age;
            Cpr = cpr;
        }
    }
}