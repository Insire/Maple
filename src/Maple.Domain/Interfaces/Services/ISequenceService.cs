using System.Collections.Generic;

namespace Maple.Domain
{
    public interface ISequenceService
    {
        int Get(IList<ISequence> items);
    }
}
