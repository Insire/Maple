namespace Maple.Data
{
    public class DBConnection
    {
        public string Path { get; }
        public DBConnection(string path)
        {
            Path = path;
        }
    }
}
