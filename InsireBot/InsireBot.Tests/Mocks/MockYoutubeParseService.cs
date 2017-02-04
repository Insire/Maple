using Maple.Youtube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maple.Tests
{
    public class MockYoutubeParseService : IYoutubeUrlParseService
    {
        public Task<UrlParseResult> Parse(string data, ParseResultType type)
        {
            throw new NotImplementedException();
        }
    }
}
