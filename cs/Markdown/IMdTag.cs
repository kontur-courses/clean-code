using System.Collections.Generic;

namespace Markdown
{
    public interface IMdTag
    {
        IEnumerable<StringChange> GetChanges { get; }
        bool IsOpened { get; }
        bool CheckTag(int index);
    }
}