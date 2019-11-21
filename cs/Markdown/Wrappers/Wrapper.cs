using System.Collections.Generic;
using Markdown.DTOs;
using Markdown.Tags;

namespace Markdown.Wrappers
{
    public abstract class Wrapper
    {
        protected Dictionary<string, string> tagMap;

        public Wrapper(Dictionary<string, string> map)
        {
            tagMap = map;
        }

        public abstract void SetDefaultMap();

        public abstract string WrapWithTag(ITag tag, Token token);
    }
}