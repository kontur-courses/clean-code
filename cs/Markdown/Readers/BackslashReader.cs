using Markdown.Tokens;

namespace Markdown.Readers
{
    public class BackslashReader : AbstractReader
    {
        public override (IToken token, int length) ReadToken(string text, int offset, ReadingOptions options)
        {
            CheckArguments(text, offset);
            var nextSymbolIdx = offset + 1;
            if (nextSymbolIdx < text.Length + 1 && text[offset] == '\\')
                return (new TextToken(text[nextSymbolIdx].ToString()), 2);
            return (null, 0);
        }
    }
}