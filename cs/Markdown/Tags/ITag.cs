using System.Collections.Generic;

namespace Markdown.Tags
{
    public interface ITag
    {
        public string Opening { get; }
        public string Closing { get; }
        
        public IEnumerable<Tag> IgnoredTags { get; set; }
    }
}