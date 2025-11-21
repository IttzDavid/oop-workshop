namespace oop_workshop.Domain.Users
{
    public abstract class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Cpr { get; set; } // Should be handled securely in a real application

        protected User(string name, int age, string cpr)
        {
            Name = name;
            Age = age;
            Cpr = cpr;
        }
    }
}