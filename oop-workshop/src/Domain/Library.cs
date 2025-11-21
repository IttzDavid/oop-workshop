using oop_workshop.Domain.Media;

namespace oop_workshop.Domain
{
    public class Library
    {
        private readonly List<Media.Media> _mediaItems;

        public Library(List<Media.Media> mediaItems)
        {
            _mediaItems = mediaItems;
        }

        public IEnumerable<T> GetMediaByType<T>() where T : Media.Media
        {
            return _mediaItems.OfType<T>();
        }

        public IEnumerable<Media.Media> GetAllMedia()
        {
            return _mediaItems;
        }
    }
}