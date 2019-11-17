using Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers;
using Markdown.TagsDataBase;

namespace Markdown.MdTags.PairTags
{
    class DoubleUnderline : MdPairTagBase
    {
        public DoubleUnderline() : 
            base(
                TagsDB.GetMdTagById("open__"),
                TagsDB.GetMdTagById("close__"),
                new DefaultPairTagAndTokenComparer())
        {}
    }
}