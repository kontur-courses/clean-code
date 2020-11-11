using System;

namespace Markdown
{
    public class TagToken
    {
        public int StartPosition;
        public int Length;
        public TagType Type;
        public int TagSignLength;
        public int EndPosition;

        public TagToken(int startPosition, int length, TagType type)
        {
            throw new NotImplementedException();
        }
    }
}