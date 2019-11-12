using System.Collections.Generic;
using System.Text;
using Markdown.Core.Tags;

namespace Markdown.Core.Processors
{
    abstract class BaseParser
    {
        public abstract IHtmlTag HtmlTag { get; }
        public abstract IMarkdownTag MarkdownTag { get; }
        protected abstract List<Token> FindMarkdownTags(string markdown);

        public string Parse(string markdown) => ReplaceTags(markdown, FindMarkdownTags(markdown));

        private string ReplaceTags(string markdown, IEnumerable<Token> tokens)
        {
            var stringBuilder = new StringBuilder(markdown);
            foreach (var token in tokens)
            {
                stringBuilder.Remove(token.StartPosition, MarkdownTag.Opening.Length);
                stringBuilder.Insert(token.StartPosition, HtmlTag.Opening);
                stringBuilder.Remove(token.EndPosition, MarkdownTag.Closing.Length);
                stringBuilder.Insert(token.EndPosition, HtmlTag.Closing);
            }

            return stringBuilder.ToString();
        }
    }
}