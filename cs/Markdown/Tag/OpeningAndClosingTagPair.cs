using System;
using System.Collections.Generic;

namespace Markdown.Tag
{
    public sealed class OpeningAndClosingTagPair<TFirst, TSecond>
        : IEquatable<OpeningAndClosingTagPair<TFirst, TSecond>>
    {
        public OpeningAndClosingTagPair(TFirst opening, TSecond closing)
        {
            this.Opening = opening;
            this.Closing = closing;
        }

        public TFirst Opening { get; }

        public TSecond Closing { get; }

        public bool Equals(OpeningAndClosingTagPair<TFirst, TSecond> other)
        {
            if (other == null)
                return false;

            return EqualityComparer<TFirst>.Default.Equals(this.Opening, other.Opening) &&
                   EqualityComparer<TSecond>.Default.Equals(this.Closing, other.Closing);
        }

        public override bool Equals(object o)
        {
            return Equals(o as OpeningAndClosingTagPair<TFirst, TSecond>);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TFirst>.Default.GetHashCode(Opening) * 37 +
                   EqualityComparer<TSecond>.Default.GetHashCode(Closing);
        }
    }
}
