using Markdown.Tokens;

namespace Markdown.Readers
{
    public class AnyCharReader : AbstractReader
    {
        public override (IToken token, int length) ReadToken(string text, int offset, ReadingOptions options)
        {
            CheckArguments(text, offset);
            return offset == text.Length ? (null, 0) : (new TextToken(text[offset].ToString()), 1);
        }
    }
}