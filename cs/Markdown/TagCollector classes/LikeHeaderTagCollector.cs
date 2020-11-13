using System.Collections.Generic;

namespace Markdown
{
    public class LikeHeaderTagCollector<TTag> : TagCollector<TTag>
        where TTag : Tag, new()
    {
        public LikeHeaderTagCollector(TextWorker textWorker)
            : base(textWorker)
        {
        }

        public override List<TTag> CollectTags(string line)
        {
            return IsMdTag(line, 0) ?
                new List<TTag> {ConstructTag(0, line.Length)} :
                new List<TTag>();
        }
    }
}