using Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers;
using Markdown.TagsDataBase;

namespace Markdown.MdTags.PairTags
{
    class SingleBlockquote : MdPairTagBase
    {
        public SingleBlockquote() : 
            base(
                TagsDB.GetMdTagById("open>"),
                TagsDB.GetMdTagById("close>"),
                new BlockquotesAndTokenComparer())
        { }
    }
}