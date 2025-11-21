using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using oop_workshop.Domain.Medias;
using oop_workshop.Domain.Users;
using oop_workshop.Persistence;
using Xunit;

namespace oop_workshop.Tests.Media
{
    public class MediaPersistenceTests
    {
        [Fact]
        public void SaveAndLoadMedia_ShouldRoundTripBasicFields()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "oop_workshop_tests_media");
            Directory.CreateDirectory(tempDir);
            var file = Path.Combine(tempDir, "media.csv");
            if (File.Exists(file)) File.Delete(file);

            var items = new Media[]
            {
                new EBook("EBookTitle", 2021, "AuthorX", "EN", 120, "ISBN123"),
                new Movie("MovieTitle", 2022, "DirectorY", "GenreZ", "EN", 95),
                new Song("SongTitle", 2023, "ComposerA", "SingerB", "GenreC", "mp3", 180, "EN")
            };

            CsvLoader.SaveMedia(file, items);
            var loaded = CsvLoader.LoadMedia(file);

            loaded.Should().HaveCount(3);
            loaded.Select(m => m.Title).Should().Contain(new[] { "EBookTitle", "MovieTitle", "SongTitle" });
        }

        [Fact]
        public void SaveAndLoadMedia_ShouldPreserveBorrowStateAndRatings()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "oop_workshop_tests_media_borrow");
            Directory.CreateDirectory(tempDir);
            var file = Path.Combine(tempDir, "media_ext.csv");
            if (File.Exists(file)) File.Delete(file);

            var borrower = new Borrower("Borrower1", 25, "000000-0000");
            var ebook = new EBook("PersistedEBook", 2020, "Author", "EN", 100, "ISBN1");
            borrower.BorrowMedia(ebook, DateTime.Today.AddDays(10));
            borrower.RateMedia(ebook, 5);

            CsvLoader.SaveMedia(file, new[] { ebook });
            var loaded = CsvLoader.LoadMedia(file).OfType<EBook>().Single();

            loaded.Title.Should().Be("PersistedEBook");
            loaded.IsBorrowed.Should().BeTrue();
            loaded.DueDate.Should().NotBeNull();
            loaded.BorrowedById.Should().Be(borrower.Id);
            loaded.Ratings.Should().ContainKey(borrower.Id).WhoseValue.Should().Be(5);
            loaded.AverageRating.Should().Be(5);
        }

        [Fact]
        public void LoadMedia_WithMissingFile_ShouldReturnEmptyAndNotThrow()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "oop_workshop_tests_media_missing");
            Directory.CreateDirectory(tempDir);
            var file = Path.Combine(tempDir, "does_not_exist.csv");
            var loaded = CsvLoader.LoadMedia(file);
            loaded.Should().BeEmpty();
        }
    }
}