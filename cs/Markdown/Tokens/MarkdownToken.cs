using Markdown.Tags;

namespace Markdown.Tokens
{
    public class MarkdownToken
    {
        public MarkdownToken(string content, int start, int length, TagType tag)
        { 
            Content = content;
            Start = start;
            Length = length;
            Tag = tag;
        }

        public TagType Tag { get; }
        public string Content { get; }
        public int Start { get; }
        public int Length { get; }
    }
}
