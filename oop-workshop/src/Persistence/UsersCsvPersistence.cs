using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using oop_workshop.Domain.Users;

namespace oop_workshop.Persistence
{
    public static class UsersCsvPersistence
    {
        private const string Header = "Role,Id,Name,Age,Cpr";

        public static UserManager LoadUsers(string path)
        {
            var manager = new UserManager();
            if (!File.Exists(path))
                return manager;

            var lines = File.ReadAllLines(path).Skip(1);
            int maxId = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length < 5) continue;

                var role = parts[0];
                var id = parts[1];
                var name = parts[2];
                var age = int.TryParse(parts[3], out var a) ? a : 0;
                var cpr = parts[4];

                // Track max numeric id (ignore non-numeric)
                if (int.TryParse(id, out var numericId))
                    maxId = Math.Max(maxId, numericId);

                User? user = role switch
                {
                    "Admin" => new Admin(id, name, age, cpr),
                    "Employee" => new Employee(id, name, age, cpr),
                    "Borrower" => new Borrower(id, name, age, cpr),
                    _ => null
                };
                if (user == null) continue;

                switch (user)
                {
                    case Admin aUser: manager.AddAdmin(aUser); break;
                    case Employee eUser: manager.AddEmployee(eUser); break;
                    case Borrower bUser: manager.AddBorrower(bUser); break;
                }
            }

            // Sync next auto id so newly created users continue sequence
            User.SyncNextIdAfterLoad(maxId);

            return manager;
        }

        public static void SaveUsers(string path, UserManager manager)
        {
            EnsureDirectory(path);

            var lines = new List<string> { Header };
            foreach (var u in manager.GetAllUsers())
            {
                var role = u switch
                {
                    Admin => "Admin",
                    Employee => "Employee",
                    Borrower => "Borrower",
                    _ => "User"
                };
                lines.Add($"{role},{u.Id},{Escape(u.Name)},{u.Age},{Escape(u.Cpr)}");
            }

            File.WriteAllLines(path, lines);
        }

        private static void EnsureDirectory(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath);
            if (string.IsNullOrWhiteSpace(dir)) return;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        private static string Escape(string s) => s.Replace(",", ";");
    }
}