using oop_workshop.Domain.Medias;

namespace oop_workshop.Domain
{
    public class Library
    {
        private readonly List<Media> _mediaItems;

        public Library(List<Media> mediaItems) => _mediaItems = mediaItems ?? throw new ArgumentNullException(nameof(mediaItems));

        public IEnumerable<T> GetMediaByType<T>() where T : Media => _mediaItems.OfType<T>();
        public IEnumerable<Media> GetAllMedia() => _mediaItems;

        public void AddMedia(Media item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _mediaItems.Add(item);
        }

        public bool RemoveMedia(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            var item = _mediaItems.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (item == null) return false;
            _mediaItems.Remove(item);
            return true;
        }

        public Media? FindByTitle(string title) =>
            _mediaItems.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
}