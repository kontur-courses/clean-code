using System.Collections.Generic;
using Markdown.Tree;

namespace Markdown.Languages
{
    public interface ILanguage
    {
        Dictionary<TagType, Tag> Tags { get; }

        bool IsTag(string line, int i, string tag);
        bool IsCloseTag(string line, int i);
        bool IsOpenTag(string line, int i, string tag);
    }
}