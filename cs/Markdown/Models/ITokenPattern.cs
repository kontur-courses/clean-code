using System.Collections.Generic;

namespace Markdown.Models
{
    public interface ITokenPattern
    {
        public int StartTagLength { get; }
        public int EndTagLength { get; }
        public bool LastCloseSucceed { get; }
        public IEnumerable<TagType> ForbiddenChildren { get; }
        public bool TrySetStart(Context context);
        public bool TryContinue(Context context);
    }
}