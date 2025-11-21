using oop_workshop.Domain;
using oop_workshop.Domain.Media;
using oop_workshop.Persistence;

Console.WriteLine("Welcome to Sønderborg Library's Digital Platform!");

// 1. Load data from persistence layer
var mediaItems = CsvLoader.LoadMedia(@"..\..\..\..\var\data.csv");
var library = new Library(mediaItems);

Console.WriteLine($"Loaded {library.GetAllMedia().Count()} media items.");

// 2. Basic presentation layer menu
while (true)
{
    Console.WriteLine("\n--- Media Menu ---");
    Console.WriteLine("1. List E-Books");
    Console.WriteLine("2. List Movies");
    Console.WriteLine("3. List Songs");
    Console.WriteLine("4. List Video Games");
    Console.WriteLine("5. List Apps");
    Console.WriteLine("6. List Podcasts");
    Console.WriteLine("7. List Images");
    Console.WriteLine("0. Exit");
    Console.Write("Select an option: ");

    var input = Console.ReadLine();
    if (input == "0") break;

    Console.WriteLine();

    switch (input)
    {
        case "1":
            DisplayMedia(library.GetMediaByType<EBook>());
            break;
        case "2":
            DisplayMedia(library.GetMediaByType<Movie>());
            break;
        case "3":
            DisplayMedia(library.GetMediaByType<Song>());
            break;
        case "4":
            DisplayMedia(library.GetMediaByType<VideoGame>());
            break;
        case "5":
            DisplayMedia(library.GetMediaByType<App>());
            break;
        case "6":
            DisplayMedia(library.GetMediaByType<Podcast>());
            break;
        case "7":
            DisplayMedia(library.GetMediaByType<Image>());
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

static void DisplayMedia<T>(IEnumerable<T> mediaItems) where T : Media
{
    if (!mediaItems.Any())
    {
        Console.WriteLine("No items of this type found.");
        return;
    }

    foreach (var item in mediaItems)
    {
        item.DisplayDetails();
        Console.WriteLine("--------------------");
    }
}               