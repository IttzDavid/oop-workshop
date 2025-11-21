using System;

namespace oop_workshop.Domain.Users
{
    public abstract class User
    {
        private static int _nextId = 1;

        // Internal technical identifier (simple incremental numeric string)
        public string Id { get; }

        public string Name { get; set; }
        public int Age { get; set; }
        public string Cpr { get; set; } // Sensitive; placeholder only

        // Auto-id constructor
        protected User(string name, int age, string cpr)
            : this((_nextId++).ToString(), name, age, cpr) { }

        // Explicit id constructor (used when loading from CSV)
        protected User(string id, string name, int age, string cpr)
        {
            Id = id;
            Name = name;
            Age = age;
            Cpr = cpr;
        }

        // Adjust next id based on max loaded id after CSV load
        internal static void SyncNextIdAfterLoad(int maxExistingId)
        {
            _nextId = Math.Max(_nextId, maxExistingId + 1);
        }
    }
}