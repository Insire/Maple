using System.Collections.Generic;

namespace Maple.Interfaces
{
    public interface ISequenceService
    {
        int Get(IList<ISequence> items);
    }
}
