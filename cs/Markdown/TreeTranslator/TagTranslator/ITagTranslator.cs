using Markdown.Data;
using Markdown.Data.TagsInfo;

namespace Markdown.TreeTranslator.TagTranslator
{
    public interface ITagTranslator
    {
        TagTranslationResult Translate(ITagInfo tagInfo);
    }
}