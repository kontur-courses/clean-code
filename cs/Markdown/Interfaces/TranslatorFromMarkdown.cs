using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITranslatorFromMarkdown
    {
        string Translate(List<ITag> tags);
    }
}