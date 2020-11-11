using System;

namespace Markdown
{
    public class TagToken
    {
        public int Position;
        public int Length;
        public TagType Type;
        public string Value;

        public TagToken()
        {
            throw new NotImplementedException();
        }
    }
}