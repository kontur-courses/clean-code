using System.Collections.Generic;

namespace Markdown.Models
{
    public interface ITokenPattern
    {
        public string StartTag { get; }
        public string EndTag { get; }
        public bool LastCloseSucceed { get; }
        public IEnumerable<TagType> ForbiddenChildren { get; }
        public bool TrySetStart(Context context);
        public bool TryContinue(Context context);
    }
}