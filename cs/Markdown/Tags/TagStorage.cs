using System.Collections.Generic;

namespace Markdown.Tags
{
    public abstract class TagStorage
    {
        public List<ITag> Tags { get; protected set; }

        public string EscapeCharacter => "\\" ;
    }
}
