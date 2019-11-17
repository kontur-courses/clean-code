using Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers;
using Markdown.TagsDataBase;

namespace Markdown.MdTags.PairTags
{
    class SingleUnderline : MdPairTagBase
    {
        public SingleUnderline() :
            base(
                TagsDB.GetMdTagById("open_"),
                TagsDB.GetMdTagById("close_"),
                new DefaultPairTagAndTokenComparer())
        {}
    }
}