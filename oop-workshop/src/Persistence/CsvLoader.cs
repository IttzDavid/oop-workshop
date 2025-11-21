using oop_workshop.Domain.Medias;
using System.Globalization;

namespace oop_workshop.Persistence
{
    public static class CsvLoader
    {
        // Original base column count (without borrow/rating persistence)
        private const int BaseColumns = 23; // up to Episode
        private const int ExtendedColumns = 26; // + BorrowedBy, DueDate, Ratings

        public static List<Media> LoadMedia(string filePath)
        {
            var mediaItems = new List<Media>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Data file not found: {filePath}");
                return mediaItems;
            }

            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) return mediaItems;

            // Skip header
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var values = line.Split(',');
                var type = Safe(values, 0);
                var title = Safe(values, 1);
                var year = int.TryParse(Safe(values, 5), out var y) ? y : 0;

                try
                {
                    Media? created = type switch
                    {
                        "EBook" => new EBook(title, year, Safe(values, 2), Safe(values, 7),
                            ParseInt(Safe(values, 8)), Safe(values, 6)),
                        "Movie" => new Movie(title, year, Safe(values, 3), Safe(values, 4),
                            Safe(values, 7), ParseInt(Safe(values, 9))),
                        "Song" => new Song(title, year, Safe(values, 11), Safe(values, 10),
                            Safe(values, 4), Safe(values, 12), ParseInt(Safe(values, 9)), Safe(values, 7)),
                        "VideoGame" => new VideoGame(title, year, Safe(values, 4), Safe(values, 13),
                            SplitPipe(Safe(values, 14))),
                        "App" => new App(title, year, Safe(values, 15), Safe(values, 13),
                            SplitPipe(Safe(values, 14)), ParseDouble(Safe(values, 16))),
                        "Podcast" => new Podcast(title, year, SplitPipe(Safe(values, 20)),
                            SplitPipe(Safe(values, 21)), ParseInt(Safe(values, 22)), Safe(values, 7),
                            ParseInt(Safe(values, 9))),
                        "Image" => new Image(title, year, Safe(values, 17), Safe(values, 18),
                            ParseDouble(Safe(values, 16)), ParseDate(Safe(values, 19))),
                        _ => null
                    };

                    if (created == null)
                        continue;

                    // Extended persistence (BorrowedBy, DueDate, Ratings)
                    if (values.Length >= ExtendedColumns)
                    {
                        var borrowedBy = Safe(values, 23);
                        var dueDateRaw = Safe(values, 24);
                        var dueDate = DateTime.TryParse(dueDateRaw, out var dd) ? dd : (DateTime?)null;
                        created.RestoreBorrowState(string.IsNullOrWhiteSpace(borrowedBy) ? null : borrowedBy, dueDate);

                        var ratingsRaw = Safe(values, 25);
                        var ratings = ParseRatings(ratingsRaw);
                        created.RestoreRatings(ratings);
                    }

                    mediaItems.Add(created);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing line for '{title}': {ex.Message}");
                }
            }

            return mediaItems;
        }

        public static void SaveMedia(string filePath, IEnumerable<Media> mediaItems)
        {
            var header = "Type,Title,Author,Director,Genre,Year,ISBN,Language,Pages,Duration,Singer,Composer,FileType,Publisher,Platform,Version,FileSize,Resolution,FileFormat,DateTaken,Host,Guest,Episode,BorrowedBy,DueDate,Ratings";
            var lines = new List<string> { header };

            foreach (var m in mediaItems)
            {
                // Map base columns (fill unused with empty strings)
                var row = m switch
                {
                    EBook b => string.Join(",", new[]
                    {
                        "EBook", b.Title, b.Author, "", "", b.Year.ToString(),
                        b.Isbn, b.Language, b.Pages.ToString(), "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", // Episode
                        m.BorrowedById ?? "", m.DueDate?.ToString("yyyy-MM-dd") ?? "",
                        SerializeRatings(m.Ratings)
                    }),
                    Movie mv => string.Join(",", new[]
                    {
                        "Movie", mv.Title, "", mv.Director, mv.Genre, mv.Year.ToString(),
                        "", mv.Language, "", mv.Duration.ToString(), "", "", "", "",
                        "", "", "", "", "", "", "", "", "",
                        m.BorrowedById ?? "", m.DueDate?.ToString("yyyy-MM-dd") ?? "",
                        SerializeRatings(m.Ratings)
                    }),
                    Song s => string.Join(",", new[]
                    {
                        "Song", s.Title, "", "", s.Genre, s.Year.ToString(),
                        "", s.Language, "", s.DurationSeconds.ToString(), s.Singer, s.Composer,
                        s.FileType, "", "", "", "", "", "", "", "", "", "",
                        m.BorrowedById ?? "", m.DueDate?.ToString("yyyy-MM-dd") ?? "",
                        SerializeRatings(m.Ratings)
                    }),
                    VideoGame vg => string.Join(",", new[]
                    {
                        "VideoGame", vg.Title, "", "", vg.Genre, vg.Year.ToString(),
                        "", "", "", "", "", "", "", vg.Publisher, string.Join('|', vg.SupportedPlatforms),
                        "", "", "", "", "", "", "", "", "",
                        m.BorrowedById ?? "", m.DueDate?.ToString("yyyy-MM-dd") ?? "",
                        SerializeRatings(m.Ratings)
                    }),
                    App a => string.Join(",", new[]
                    {
                        "App", a.Title, "", "", "", a.Year.ToString(), "", "", "", "", "", "", "",
                        a.Publisher, string.Join('|', a.SupportedPlatforms), a.Version, a.FileSize.ToString(CultureInfo.InvariantCulture),
                        "", "", "", "", "", "",
                        m.BorrowedById ?? "", m.DueDate?.ToString("yyyy-MM-dd") ?? "",
                        SerializeRatings(m.Ratings)
                    }),
                    Podcast p => string.Join(",", new[]
                    {
                        "Podcast", p.Title, "", "", "", p.Year.ToString(), "", p.Language, "", p.DurationSeconds.ToString(),
                        "", "", "", "", "", "", "", "", "", "",
                        string.Join('|', p.Hosts), string.Join('|', p.Guests), p.EpisodeNumber.ToString(),
                        m.BorrowedById ?? "", m.DueDate?.ToString("yyyy-MM-dd") ?? "",
                        SerializeRatings(m.Ratings)
                    }),
                    Image im => string.Join(",", new[]
                    {
                        "Image", im.Title, "", "", "", im.Year.ToString(), "", "", "", "", "", "", "", "", "", "",
                        im.FileSize.ToString(CultureInfo.InvariantCulture), im.Resolution, im.FileFormat,
                        im.DateTaken.ToString("yyyy-MM-dd"), "", "", "",
                        m.BorrowedById ?? "", m.DueDate?.ToString("yyyy-MM-dd") ?? "",
                        SerializeRatings(m.Ratings)
                    }),
                    _ => ""
                };

                if (!string.IsNullOrEmpty(row))
                    lines.Add(row);
            }

            File.WriteAllLines(filePath, lines);
        }

        private static string Safe(string[] arr, int index) => index < arr.Length ? arr[index].Trim() : "";
        private static int ParseInt(string s) => int.TryParse(s, out var i) ? i : 0;
        private static double ParseDouble(string s) => double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d) ? d : 0.0;
        private static DateTime ParseDate(string s) => DateTime.TryParse(s, out var dt) ? dt : DateTime.MinValue;
        private static string[] SplitPipe(string s) => string.IsNullOrWhiteSpace(s) ? Array.Empty<string>() : s.Split('|', StringSplitOptions.RemoveEmptyEntries);
        private static IEnumerable<(string borrowerId, int score)> ParseRatings(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return Enumerable.Empty<(string, int)>();
            return s.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(part =>
                {
                    var pair = part.Split(':');
                    if (pair.Length != 2) return (borrowerId: "", score: 0);
                    return (borrowerId: pair[0], score: int.TryParse(pair[1], out var sc) ? sc : 0);
                })
                .Where(r => !string.IsNullOrWhiteSpace(r.borrowerId) && r.score > 0);
        }
        private static string SerializeRatings(IReadOnlyDictionary<string, int> ratings) =>
            ratings.Count == 0 ? "" : string.Join('|', ratings.Select(kv => $"{kv.Key}:{kv.Value}"));
    }
}