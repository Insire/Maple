using System.Collections.Generic;
using Maple.Interfaces;

namespace Maple.Interfaces
{
    public interface ISequenceService
    {
        int Get(IList<ISequence> items);
    }
}
