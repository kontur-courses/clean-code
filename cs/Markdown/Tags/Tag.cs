using System.Collections.Generic;
using Markdown.Tags.Html;

namespace Markdown.Tags
{
    public class Tag : ITag
    {
        public static Tag Empty => new Tag("");
        public string Opening { get; }
        public string Closing { get; }
        public IEnumerable<Tag> IgnoredTags { get; set; }

        public Tag(string opening, string closing = "")
        {
            Opening = opening;
            Closing = closing;
            IgnoredTags = new List<Tag>();
        }
        
        public static bool IsHighlightingTag(ITag tag)
        {
            return tag.Equals(HtmlTags.Italics) || tag.Equals(HtmlTags.Bold);
        }

        public override int GetHashCode()
        {
            return Opening.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Tag tag)
                return string.Equals(Opening, tag.Opening);
            
            return false;
        }
    }
}