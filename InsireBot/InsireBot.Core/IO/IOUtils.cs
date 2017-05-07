using System.IO;
using System.Linq;

namespace Maple.Core
{
    public class IOUtils
    {
        public static bool IsLocalFile(string path)
        {
            if (ContainsInvalidChars(path))
                return false;

            return File.Exists(path);
        }

        private static bool ContainsInvalidChars(string path)
        {
            var result = false;
            var invalidChars = Path.GetInvalidPathChars()
                                    .Concat(Path.GetInvalidFileNameChars())
                                    .Distinct();

            for (var i = 0; i < path.Length; i++)
            {
                result = invalidChars.Contains(path[i]);
                if (result)
                    return true;
            }

            return result;
        }
    }
}
