using System;
using System.Globalization;
using System.Linq;
using oop_workshop.Domain;
using oop_workshop.Domain.Interfaces;
using oop_workshop.Domain.Medias;
using oop_workshop.Domain.Users;
using oop_workshop.Persistence;

var baseDir = AppContext.BaseDirectory;
var projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
var dataDir = Path.Combine(projectRoot, "var");
Directory.CreateDirectory(dataDir);

var mediaFile = Path.Combine(dataDir, "data.csv");
var usersFile = Path.Combine(dataDir, "users.csv");

Console.WriteLine("Welcome to Sønderborg Library's Digital Platform!");
var mediaItems = CsvLoader.LoadMedia(mediaFile);
var library = new Library(mediaItems);

var users = UsersCsvPersistence.LoadUsers(usersFile);

if (!users.GetAllUsers().Any())
{
    Console.WriteLine("No users found in CSV. Create initial Admin to proceed.");
    var name = PromptString("Admin Name (required)", required: true);
    var age = PromptInt("Admin Age (number)", allowZero: false);
    var cpr = PromptString("Admin CPR (required)", required: true);
    users.AddAdmin(new Admin(name, age, cpr));
    UsersCsvPersistence.SaveUsers(usersFile, users);
    Console.WriteLine("Initial Admin created and persisted.\n");
}

Console.WriteLine($"Loaded {library.GetAllMedia().Count()} media items.");
Console.WriteLine("Select role:");
int optionIndex = 1;
var roleOptions = users.GetAllUsers()
    .GroupBy(u => u.GetType().Name)
    .ToDictionary(g => g.Key, g => g.ToList());

string[] order = { "Borrower", "Employee", "Admin" };
var availableRoles = order.Where(r => roleOptions.ContainsKey(r)).ToList();

foreach (var r in availableRoles)
{
    Console.WriteLine($"{optionIndex}. {r} ({roleOptions[r].Count} available)");
    optionIndex++;
}
Console.WriteLine("0. Exit");
Console.Write("Choice: ");
var choice = Console.ReadLine();

if (choice == "0")
{
    Console.WriteLine("Goodbye!");
    return;
}

int chosenIdx = int.TryParse(choice, out var parsed) ? parsed : -1;
User activeUser;
if (chosenIdx >= 1 && chosenIdx <= availableRoles.Count)
{
    var roleName = availableRoles[chosenIdx - 1];
    activeUser = roleOptions[roleName].First();
}
else
{
    Console.WriteLine("Invalid selection. Defaulting to first available user.");
    activeUser = users.GetAllUsers().First();
}

Console.WriteLine($"\nLogged in as: {activeUser.Name} ({activeUser.GetType().Name})");

if (activeUser is Borrower borrower) BorrowerMenu(borrower, library, mediaFile);
else if (activeUser is Admin admin) AdminMenu(admin, library, users, mediaFile, usersFile);
else if (activeUser is Employee employee) EmployeeMenu(employee, library, mediaFile);

Console.WriteLine("Goodbye!");

// Borrower menu
static void BorrowerMenu(Borrower borrower, Library library, string mediaFile)
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
                        CsvLoader.SaveMedia(mediaFile, library.GetAllMedia());
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
                        CsvLoader.SaveMedia(mediaFile, library.GetAllMedia());
                    }
                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
                }
                break;
            case "5":
                var m5 = SelectMedia(library);
                if (m5 != null)
                {
                    var score = PromptInt("Score (1-5)", min: 1, max: 5);
                    try
                    {
                        borrower.RateMedia(m5, score);
                        Console.WriteLine("Rated.");
                        CsvLoader.SaveMedia(mediaFile, library.GetAllMedia());
                    }
                    catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
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
static void EmployeeMenu(Employee employee, Library library, string mediaFile)
{
    while (true)
    {
        Console.WriteLine("\n--- Employee Menu ---");
        Console.WriteLine("1. List all media");
        Console.WriteLine("2. Add media");
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
                var newMedia = CreateMediaInteractive();
                if (newMedia != null)
                {
                    employee.AddMedia(library, newMedia);
                    Console.WriteLine("Media added.");
                    CsvLoader.SaveMedia(mediaFile, library.GetAllMedia());
                }
                break;
            case "3":
                var title = PromptString("Title to remove", required: true);
                if (library.RemoveMedia(title))
                {
                    Console.WriteLine("Removed.");
                    CsvLoader.SaveMedia(mediaFile, library.GetAllMedia());
                }
                else Console.WriteLine("Not found.");
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}

// Admin menu
static void AdminMenu(Admin admin, Library library, UserManager users, string mediaFile, string usersFile)
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
                EmployeeMenu(admin, library, mediaFile);
                break;
            case "2":
                foreach (var u in users.GetAllUsers())
                    Console.WriteLine($"{u.GetType().Name} - {u.Name} - Age {u.Age} - Id {u.Id}");
                break;
            case "3":
                var bn = PromptString("Borrower Name", required: true);
                var ba = PromptInt("Borrower Age", allowZero: false);
                var bc = PromptString("Borrower CPR", required: true);
                admin.CreateBorrower(users, bn, ba, bc);
                UsersCsvPersistence.SaveUsers(usersFile, users);
                Console.WriteLine("Borrower created.");
                break;
            case "4":
                var en = PromptString("Employee Name", required: true);
                var ea = PromptInt("Employee Age", allowZero: false);
                var ec = PromptString("Employee CPR", required: true);
                admin.CreateEmployee(users, en, ea, ec);
                UsersCsvPersistence.SaveUsers(usersFile, users);
                Console.WriteLine("Employee created.");
                break;
            case "5":
                var uid = PromptString("User Id", required: true);
                var nn = PromptString("New Name (blank skip)", required: false);
                Console.Write("New Age (blank skip): ");
                var naStr = Console.ReadLine();
                int? newAge = int.TryParse(naStr, out var parsedAge) && parsedAge > 0 ? parsedAge : null;
                Console.WriteLine(admin.UpdateUser(users, uid, string.IsNullOrWhiteSpace(nn) ? null : nn, newAge) ? "Updated." : "Not found.");
                UsersCsvPersistence.SaveUsers(usersFile, users);
                break;
            case "6":
                var bid = PromptString("Borrower Id", required: true);
                Console.WriteLine(admin.DeleteBorrower(users, bid) ? "Deleted." : "Not found.");
                UsersCsvPersistence.SaveUsers(usersFile, users);
                break;
            case "7":
                var eid = PromptString("Employee Id", required: true);
                Console.WriteLine(admin.DeleteEmployee(users, eid) ? "Deleted." : "Not found.");
                UsersCsvPersistence.SaveUsers(usersFile, users);
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}

// Helpers for listing and selection
static void ListByType(Library library)
{
    var type = PromptString("Type (EBook/Movie/Song/VideoGame/App/Podcast/Image)", required: true).ToLower();
    var items = type switch
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
    var idx = PromptInt("Select number", min: 1, max: all.Count);
    return all[idx - 1];
}

static Media? SelectBorrowedMedia(Borrower borrower)
{
    var list = borrower.CurrentlyBorrowed.ToList();
    if (!list.Any()) { Console.WriteLine("Nothing borrowed."); return null; }
    for (int i = 0; i < list.Count; i++)
        Console.WriteLine($"{i + 1}. {list[i].Title}");
    var idx = PromptInt("Select number", min: 1, max: list.Count);
    return list[idx - 1];
}

static void PerformCapabilities(Media media)
{
    if (media is IDownloadable d) d.Download();
    if (media is IPlayable p) p.Play();
    if (media is IReadable r) r.Read();
    if (media is IViewable v) v.View();
    if (media is IExecutable e) e.Execute();
}

// Interactive media creation
static Media? CreateMediaInteractive()
{
    Console.WriteLine("\n--- Create Media ---");
    var type = PromptString("Type (EBook/Movie/Song/VideoGame/App/Podcast/Image)", required: true).Trim().ToLower();
    var title = PromptString("Title", required: true);
    var year = PromptInt("Year", min: 0, max: DateTime.Now.Year);

    Media? media = type switch
    {
        "ebook" => new EBook(
            title,
            year,
            PromptString("Author", required: true),
            PromptString("Language", required: true),
            PromptInt("Pages", min: 1),
            PromptString("ISBN", required: true)
        ),
        "movie" => new Movie(
            title,
            year,
            PromptString("Director", required: true),
            PromptString("Genre", required: true),
            PromptString("Language", required: true),
            PromptInt("Duration (minutes)", min: 1)
        ),
        "song" => new Song(
            title,
            year,
            PromptString("Composer", required: true),
            PromptString("Singer", required: true),
            PromptString("Genre", required: true),
            PromptString("File Type (e.g. mp3)", required: true),
            PromptInt("Duration (seconds)", min: 1),
            PromptString("Language", required: true)
        ),
        "videogame" => new VideoGame(
            title,
            year,
            PromptString("Genre", required: true),
            PromptString("Publisher", required: true),
            PromptList("Supported Platforms (comma separated)", required: true)
        ),
        "app" => new App(
            title,
            year,
            PromptString("Version", required: true),
            PromptString("Publisher", required: true),
            PromptList("Supported Platforms (comma separated)", required: true),
            PromptDouble("File Size (MB)", min: 0.01)
        ),
        "podcast" => new Podcast(
            title,
            year,
            PromptList("Hosts (comma separated)", required: true),
            PromptList("Guests (comma separated, blank if none)", required: false),
            PromptInt("Episode Number", min: 1),
            PromptString("Language", required: true),
            PromptInt("Duration (seconds)", min: 1)
        ),
        "image" => new Image(
            title,
            year,
            PromptString("Resolution (e.g. 1920x1080)", required: true),
            PromptString("File Format (e.g. jpg)", required: true),
            PromptDouble("File Size (MB)", min: 0.01),
            PromptDate("Date Taken (yyyy-MM-dd)")
        ),
        _ => null
    };

    if (media == null)
        Console.WriteLine("Unsupported type.");

    return media;
}

// Input helpers
static string PromptString(string label, bool required)
{
    while (true)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine() ?? "";
        if (!required || !string.IsNullOrWhiteSpace(input))
            return input.Trim();
        Console.WriteLine("Value required.");
    }
}

static int PromptInt(string label, int? min = null, int? max = null, bool allowZero = true)
{
    while (true)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine();
        if (int.TryParse(input, out var value))
        {
            if (!allowZero && value == 0) { Console.WriteLine("Zero not allowed."); continue; }
            if (min.HasValue && value < min.Value) { Console.WriteLine($"Minimum {min.Value}."); continue; }
            if (max.HasValue && value > max.Value) { Console.WriteLine($"Maximum {max.Value}."); continue; }
            return value;
        }
        Console.WriteLine("Invalid number.");
    }
}

static double PromptDouble(string label, double? min = null)
{
    while (true)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine();
        if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
        {
            if (min.HasValue && value < min.Value) { Console.WriteLine($"Minimum {min.Value}."); continue; }
            return value;
        }
        Console.WriteLine("Invalid number.");
    }
}

static string[] PromptList(string label, bool required)
{
    while (true)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(input) && required)
        {
            Console.WriteLine("At least one value required.");
            continue;
        }
        if (string.IsNullOrWhiteSpace(input))
            return Array.Empty<string>();
        return input.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToArray();
    }
}

static DateTime PromptDate(string label)
{
    while (true)
    {
        Console.Write($"{label}: ");
        var input = Console.ReadLine();
        if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return dt;
        Console.WriteLine("Invalid date format. Use yyyy-MM-dd.");
    }
}