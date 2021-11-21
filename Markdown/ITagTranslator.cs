using System;

namespace Markdown
{
    internal interface ITagTranslator
    {
        Tag Translate(Tag tag);
    }
}