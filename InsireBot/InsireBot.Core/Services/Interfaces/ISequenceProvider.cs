using System.Collections.Generic;

namespace Maple.Core
{
    public interface ISequenceProvider
    {
        int Get(IList<ISequence> items);
    }
}
