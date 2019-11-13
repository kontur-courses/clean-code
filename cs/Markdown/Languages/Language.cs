using System.Collections.Generic;
using Markdown.Tree;

namespace Markdown.Languages
{
    public interface ILanguage
    {
        Dictionary<TagType, Tag> Tags { get; }

        SyntaxTree RenderTree(string str);
    }
}