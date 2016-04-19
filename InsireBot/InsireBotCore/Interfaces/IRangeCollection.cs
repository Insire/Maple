using System.Collections.Generic;

namespace InsireBotCore
{
    public interface IRangeCollection<T>
    {
        void AddRange(IEnumerable<T> items);
    }
}
