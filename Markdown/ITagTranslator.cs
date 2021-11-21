using System;

namespace Markdown
{
    public interface ITagTranslator
    {
        Tag Translate(Tag tag);
    }
}