using System.Collections.Generic;
using Markdown.Tree;

namespace Markdown.Languages
{
    public class Html : ILanguage
    {
        public Dictionary<TagType, Tag> Tags { get; }

        public Html()
        {
            this.Tags = new Dictionary<TagType, Tag>()
            {
                {TagType.Em, new Tag("<em>", "</em>", new TagType[] { })},
                {TagType.Strong, new Tag("<strong>", "</strong>", new TagType[] { })}
            };
        }

        public SyntaxTree RenderTree(string str)
        {
            throw new System.NotImplementedException();
        }
    }
}