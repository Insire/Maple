using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsireBot.Data
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
