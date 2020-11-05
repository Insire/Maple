using System.IO;
using System.Windows;

namespace Maple
{
    public interface IThumbnailCache
    {
        void Add<T>(Stream stream, int id, Size size);
        Stream Get<T>(int id, Size? size);
    }
}