using System.Collections.Generic;
using Markdown.Tree;

namespace Markdown.Languages
{
    public class Html : ILanguage
    {
        public LanguageTagDict Tags { get; }

        public Html()
        {
            Tags = new LanguageTagDict(new Dictionary<TagType, Tag>()
            {
                {TagType.Em, new Tag("<em>", "</em>", new TagType[] { })},
                {TagType.Strong, new Tag("<strong>", "</strong>", new TagType[] { })}
            });
        }

        public bool IsTag(string line, int i, string tag)
        {
            throw new System.NotImplementedException();
        }

        public bool IsCloseTag(string line, int i)
        {
            throw new System.NotImplementedException();
        }

        public bool IsOpenTag(string line, int i, string tag)
        {
            throw new System.NotImplementedException();
        }

        public SyntaxTree RenderTree(string str)
        {
            throw new System.NotImplementedException();
        }
    }
}