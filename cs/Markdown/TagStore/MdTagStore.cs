using System;
using System.Linq;
using Markdown.Tags;

namespace Markdown.TagStore
{
    public class MdTagStore : TagStore
    {
        private static ITag[] tags = 
        {
            new Tag("_", TagType.Emphasized)
        };

        public MdTagStore()
        {
            foreach (var tag in tags)
            {
                base.Register(tag);
            }
        }
        
    }
}