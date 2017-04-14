using Maple.Localization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maple.Core
{
    public class SequenceService : ISequenceProvider
    {
        private readonly IMapleLog _log;

        public SequenceService(IMapleLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
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
                        _log.Error(Resources.MaxPlaylistCountReachedException);
                        break;
                    }

                    result++;
                }
            }

            return result;
        }
    }
}
