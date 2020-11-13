using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public abstract class TagCollector<TTag>
        where TTag : Tag, new()
    {
        private TTag tagUnderConstruction;
        protected readonly TextWorker textWorker;

        public string MdTag => tagUnderConstruction.MdTag;

        public TagCollector(TextWorker textWorker)
        {
            this.textWorker = textWorker;
            tagUnderConstruction = new TTag();
        }

        public abstract List<TTag> CollectTags(string line);

        protected TTag ConstructTag(int start, int end)
        {
            tagUnderConstruction.StartOfOpeningTag = start;
            tagUnderConstruction.StartOfClosingTag = end;

            var constructedTag = tagUnderConstruction;
            tagUnderConstruction = new TTag();

            return constructedTag;
        }

        protected bool IsMdTag(string line, int startOfTag)
        {
            var endOfTag = startOfTag + MdTag.Length - 1;

            return line[startOfTag] == MdTag[0] &&
                   !textWorker.IsEscapedChar(line, startOfTag) &&
                   endOfTag < line.Length &&
                   textWorker.GoThroughText(line, startOfTag, endOfTag)
                       .SequenceEqual(MdTag);
        }
    }
}