using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITranslator
    {
        string Translate(IEnumerable<ITag> tags);
    }
}