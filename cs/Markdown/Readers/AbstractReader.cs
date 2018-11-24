using System;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public abstract class AbstractReader
    {
        public abstract (IToken token, int length) ReadToken(string text, int offset, ReadingOptions options);

        protected void CheckArguments(string text, int offset)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text should not be null or empty");
            if (offset < 0 || offset > text.Length)
                throw new ArgumentException("Offset should be non-negative number less than text' length");
        }
    }
}