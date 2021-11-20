using System;

namespace Markdown
{
    public interface ITokenTranslator
    {
        Tag Translate(Tag tag);
    }
}