using System;

namespace Markdown.DataStructures
{
    public class EmTag : ITag
    {
        public string OpeningTag => "<em>";
        public string ClosingTag => "</em>";
        public string MarkdownName => "_";
    }
}