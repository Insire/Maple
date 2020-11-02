using System.IO;

namespace Maple
{
    public interface IMediaItemCache
    {
        void Add<T>(Stream stream, int id);
        Stream Get<T>(int id);
    }
}