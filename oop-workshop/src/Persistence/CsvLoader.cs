using oop_workshop.Domain.Medias;
using System.Globalization;

namespace oop_workshop.Persistence
{
    public static class CsvLoader
    {
        public static List<Media> LoadMedia(string filePath)
        {
            var mediaItems = new List<Media>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Data file not found: {filePath}");
                return mediaItems;
            }

            var lines = File.ReadAllLines(filePath).Skip(1); // Skip header

            foreach (var line in lines)
            {
                var values = line.Split(',');
                if (values.Length == 0) continue;

                var type = values[0];
                var title = values.Length > 1 ? values[1] : "Untitled";
                var year = int.TryParse(values.Length > 5 ? values[5] : null, out var y) ? y : 0;

                try
                {
                    switch (type)
                    {
                        case "EBook":
                            mediaItems.Add(new EBook(title, year, values[2], values[7], int.Parse(values[8]), values[6]));
                            break;
                        case "Movie":
                            mediaItems.Add(new Movie(title, year, values[3], values[4], values[7], int.Parse(values[9])));
                            break;
                        case "Song":
                            mediaItems.Add(new Song(title, year, values[11], values[10], values[4], values[12], int.Parse(values[9]), values[7]));
                            break;
                        case "VideoGame":
                            mediaItems.Add(new VideoGame(title, year, values[4], values[13], values[14].Split('|')));
                            break;
                        case "App":
                            mediaItems.Add(new App(title, year, values[15], values[13], values[14].Split('|'), double.Parse(values[16], CultureInfo.InvariantCulture)));
                            break;
                        case "Podcast":
                            mediaItems.Add(new Podcast(title, year, values[20].Split('|'), values[21].Split('|'), int.Parse(values[22]), values[7], int.Parse(values[9])));
                            break;
                        case "Image":
                            mediaItems.Add(new Image(title, year, values[17], values[18], double.Parse(values[16], CultureInfo.InvariantCulture),
                                DateTime.TryParse(values[19], out var dt) ? dt : DateTime.MinValue));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line for '{title}': {ex.Message}");
                }
            }

            return mediaItems;
        }
    }
}