using System.Collections.Generic;

namespace Maple.Core
{
    public interface ISequenceService
    {
        int Get(IList<ISequence> items);
    }
}
