using System;
using System.Runtime.CompilerServices;

namespace Markdown
{
    internal class Tag
    {
        public string Start { get; }
        public string End { get; }

        private Tag() {}

        internal Tag(string openWord, string closeWord)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(openWord), openWord));
            
            Start = openWord;
            End = closeWord;
        }

        public override bool Equals(object obj)
        {
            return obj is Tag tag && Equals(tag);
        }

        private bool Equals(Tag other)
        {
            return Start == other.Start;
        }

        public override int GetHashCode()
        {
            return End is not null ? HashCode.Combine(Start, End) : Start.GetHashCode();
        }
    }
}