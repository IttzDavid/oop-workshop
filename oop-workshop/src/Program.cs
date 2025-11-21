using System;
using System.Linq;
using oop_workshop.Domain;
using oop_workshop.Domain.Interfaces;
using oop_workshop.Domain.Medias;
using oop_workshop.Domain.Users;
using oop_workshop.Persistence;

Console.WriteLine("Welcome to Sønderborg Library's Digital Platform!");

var mediaItems = CsvLoader.LoadMedia(@"..\..\..\..\var\data.csv");
var library = new Library(mediaItems);

var users = new UserManager();
var defaultAdmin = new Admin("Admin", 40, "000000-0000");
users.AddAdmin(defaultAdmin);
var defaultEmployee = new Employee("Employee", 30, "111111-1111");
users.AddEmployee(defaultEmployee);
var defaultBorrower = new Borrower("Borrower", 20, "222222-2222");
users.AddBorrower(defaultBorrower);

Console.WriteLine($"Loaded {library.GetAllMedia().Count()} media items.\n");

Console.WriteLine("Select role:");
Console.WriteLine("1. Borrower");
Console.WriteLine("2. Employee");
Console.WriteLine("3. Admin");
Console.Write("Choice: ");
var roleChoice = Console.ReadLine();

User activeUser = roleChoice switch
{
    "1" => defaultBorrower,
    "2" => defaultEmployee,
    "3" => defaultAdmin,
    _ => defaultBorrower
};

Console.WriteLine($"\nLogged in as: {activeUser.Name} ({activeUser.GetType().Name})");

if (activeUser is Borrower borrower) BorrowerMenu(borrower, library);
else if (activeUser is Admin admin) AdminMenu(admin, library, users);
else if (activeUser is Employee employee) EmployeeMenu(employee, library);

Console.WriteLine("Goodbye!");

// Borrower menu
static void BorrowerMenu(Borrower borrower, Library library)
{
    while (true)
    {
        Console.WriteLine("\n--- Borrower Menu ---");
        Console.WriteLine("1. List media by type");
        Console.WriteLine("2. View media details");
        Console.WriteLine("3. Borrow media");
        Console.WriteLine("4. Return media");
        Console.WriteLine("5. Rate media");
        Console.WriteLine("6. Perform action (capabilities)");
        Console.WriteLine("0. Exit");
        Console.Write("Select: ");
        var input = Console.ReadLine();
        if (input == "0") break;

        switch (input)
        {
            case "1":
                ListByType(library);
                break;
            case "2":
                SelectMedia(library)?.DisplayDetails();
                break;
            case "3":
                var m3 = SelectMedia(library);
                if (m3 != null)
                {
                    try
                    {
                        borrower.BorrowMedia(m3, DateTime.Today.AddDays(14));
                        Console.WriteLine("Borrowed.");
                    }
                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                }
                break;
            case "4":
                var m4 = SelectBorrowedMedia(borrower);
                if (m4 != null)
                {
                    try
                    {
                        borrower.ReturnMedia(m4);
                        Console.WriteLine("Returned.");
                    }
                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                }
                break;
            case "5":
                var m5 = SelectMedia(library);
                if (m5 != null)
                {
                    Console.Write("Score (1-5): ");
                    if (int.TryParse(Console.ReadLine(), out var score))
                    {
                        try
                        {
                            borrower.RateMedia(m5, score);
                            Console.WriteLine("Rated.");
                        }
                        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                    }
                    else Console.WriteLine("Invalid score.");
                }
                break;
            case "6":
                var m6 = SelectMedia(library);
                if (m6 != null) PerformCapabilities(m6);
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}

// Employee menu
static void EmployeeMenu(Employee employee, Library library)
{
    while (true)
    {
        Console.WriteLine("\n--- Employee Menu ---");
        Console.WriteLine("1. List all media");
        Console.WriteLine("2. Add media (simple)");
        Console.WriteLine("3. Remove media");
        Console.WriteLine("0. Exit");
        Console.Write("Select: ");
        var input = Console.ReadLine();
        if (input == "0") break;

        switch (input)
        {
            case "1":
                foreach (var m in library.GetAllMedia()) { m.DisplayDetails(); Console.WriteLine("-----"); }
                break;
            case "2":
                var newMedia = CreateMediaStub();
                if (newMedia != null)
                {
                    employee.AddMedia(library, newMedia);
                    Console.WriteLine("Media added.");
                }
                break;
            case "3":
                Console.Write("Title to remove: ");
                var title = Console.ReadLine();
                Console.WriteLine(library.RemoveMedia(title ?? "") ? "Removed." : "Not found.");
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}

// Admin menu
static void AdminMenu(Admin admin, Library library, UserManager users)
{
    while (true)
    {
        Console.WriteLine("\n--- Admin Menu ---");
        Console.WriteLine("1. Media management (Employee actions)");
        Console.WriteLine("2. List users");
        Console.WriteLine("3. Create borrower");
        Console.WriteLine("4. Create employee");
        Console.WriteLine("5. Update user (name/age)");
        Console.WriteLine("6. Delete borrower");
        Console.WriteLine("7. Delete employee");
        Console.WriteLine("0. Exit");
        Console.Write("Select: ");
        var input = Console.ReadLine();
        if (input == "0") break;

        switch (input)
        {
            case "1":
                EmployeeMenu(admin, library);
                break;
            case "2":
                foreach (var u in users.GetAllUsers())
                    Console.WriteLine($"{u.GetType().Name} - {u.Name} - Age {u.Age} - Id {u.Id}");
                break;
            case "3":
                Console.Write("Name: "); var bn = Console.ReadLine();
                Console.Write("Age: "); int.TryParse(Console.ReadLine(), out var ba);
                Console.Write("CPR: "); var bc = Console.ReadLine();
                admin.CreateBorrower(users, bn ?? "Borrower", ba, bc ?? "000000-0000");
                Console.WriteLine("Borrower created.");
                break;
            case "4":
                Console.Write("Name: "); var en = Console.ReadLine();
                Console.Write("Age: "); int.TryParse(Console.ReadLine(), out var ea);
                Console.Write("CPR: "); var ec = Console.ReadLine();
                admin.CreateEmployee(users, en ?? "Employee", ea, ec ?? "111111-1111");
                Console.WriteLine("Employee created.");
                break;
            case "5":
                Console.Write("User Id: "); var uid = Console.ReadLine();
                Console.Write("New Name (blank skip): "); var nn = Console.ReadLine();
                Console.Write("New Age (blank skip): "); var naStr = Console.ReadLine();
                int? newAge = int.TryParse(naStr, out var parsedAge) ? parsedAge : null;
                Console.WriteLine(admin.UpdateUser(users, uid ?? "", nn, newAge) ? "Updated." : "Not found.");
                break;
            case "6":
                Console.Write("Borrower Id: "); var bid = Console.ReadLine();
                Console.WriteLine(admin.DeleteBorrower(users, bid ?? "") ? "Deleted." : "Not found.");
                break;
            case "7":
                Console.Write("Employee Id: "); var eid = Console.ReadLine();
                Console.WriteLine(admin.DeleteEmployee(users, eid ?? "") ? "Deleted." : "Not found.");
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}

// Helpers
static void ListByType(Library library)
{
    Console.Write("Type (EBook/Movie/Song/VideoGame/App/Podcast/Image): ");
    var type = Console.ReadLine()?.ToLower();
    IEnumerable<Media> items = type switch
    {
        "ebook" => library.GetMediaByType<EBook>(),
        "movie" => library.GetMediaByType<Movie>(),
        "song" => library.GetMediaByType<Song>(),
        "videogame" => library.GetMediaByType<VideoGame>(),
        "app" => library.GetMediaByType<App>(),
        "podcast" => library.GetMediaByType<Podcast>(),
        "image" => library.GetMediaByType<Image>(),
        _ => Enumerable.Empty<Media>()
    };
    if (!items.Any()) { Console.WriteLine("No items."); return; }
    foreach (var m in items) { m.DisplayDetails(); Console.WriteLine("-----"); }
}

static Media? SelectMedia(Library library)
{
    var all = library.GetAllMedia().ToList();
    if (!all.Any()) { Console.WriteLine("No media."); return null; }
    for (int i = 0; i < all.Count; i++)
        Console.WriteLine($"{i + 1}. {all[i].Title} ({all[i].GetType().Name})");
    Console.Write("Select number: ");
    if (int.TryParse(Console.ReadLine(), out var idx) && idx >= 1 && idx <= all.Count)
        return all[idx - 1];
    Console.WriteLine("Invalid selection.");
    return null;
}

static Media? SelectBorrowedMedia(Borrower borrower)
{
    var list = borrower.CurrentlyBorrowed.ToList();
    if (!list.Any()) { Console.WriteLine("Nothing borrowed."); return null; }
    for (int i = 0; i < list.Count; i++)
        Console.WriteLine($"{i + 1}. {list[i].Title}");
    Console.Write("Select number: ");
    if (int.TryParse(Console.ReadLine(), out var idx) && idx >= 1 && idx <= list.Count)
        return list[idx - 1];
    Console.WriteLine("Invalid selection.");
    return null;
}

static void PerformCapabilities(Media media)
{
    if (media is IDownloadable d) d.Download();
    if (media is IPlayable p) p.Play();
    if (media is IReadable r) r.Read();
    if (media is IViewable v) v.View();
    if (media is IExecutable e) e.Execute();
}

static Media? CreateMediaStub()
{
    Console.Write("Type to create: ");
    var type = Console.ReadLine()?.Trim().ToLower();
    Console.Write("Title: "); var title = Console.ReadLine() ?? "Untitled";
    Console.Write("Year: "); int.TryParse(Console.ReadLine(), out var year);

    return type switch
    {
        "ebook" => new EBook(title, year, "Author", "EN", 100, "ISBN"),
        "movie" => new Movie(title, year, "Director", "Genre", "EN", 120),
        "song" => new Song(title, year, "Composer", "Singer", "Genre", "mp3", 180, "EN"),
        "videogame" => new VideoGame(title, year, "Genre", "Publisher", new[] { "PC" }),
        "app" => new App(title, year, "1.0", "Publisher", new[] { "Windows" }, 50.0),
        "podcast" => new Podcast(title, year, new[] { "Host" }, new[] { "Guest" }, 1, "EN", 3600),
        "image" => new Image(title, year, "1920x1080", "jpg", 2.5, DateTime.Today),
        _ => null
    };
}