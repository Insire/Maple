using System;
using System.Collections.Generic;
using System.Linq;
using Maple.Domain;
using Maple.Properties;
using Microsoft.Extensions.Logging;

namespace Maple
{
    public sealed class SequenceService : ISequenceService
    {
        private readonly ILogger _log;

        public SequenceService(ILoggerFactory factory)
        {
            _log = factory.CreateLogger<SequenceService>() ?? throw new ArgumentNullException(nameof(factory));
        }

        public int Get(IList<ISequence> items)
        {
            var result = 0;

            if (items.Count > 0)
            {
                while (items.Any(p => p.Sequence == result))
                {
                    if (result == int.MaxValue)
                    {
                        _log.LogError(Resources.MaxPlaylistCountReachedException);
                        break;
                    }

                    result++;
                }
            }

            return result;
        }
    }
}
