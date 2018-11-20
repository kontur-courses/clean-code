using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class PairedTagReader : AbstractReader
    {
        public string TagName { get; }
        public string TagSymbols { get; }

        public PairedTagReader(string tagName, string tagSymbols)
        {
            TagName = tagName;
            TagSymbols = tagSymbols;
        }

        public override (IToken token, int read) ReadToken(string text, int offset, ReadingOptions options)
        {
            CheckArguments(text, offset);
            var innerTokens = new List<IToken>();
            var currentIdx = offset;

            if (!CanReadStart(text, offset))
                return (null, 0);
            currentIdx += TagSymbols.Length;
            var allowedInnerReaders = options.GetAvailableInnerReadersFor(this);
            do
            {
                var (innerToken, innerRead) = allowedInnerReaders
                    .Select(reader => reader.ReadToken(text, currentIdx, options.WithNewAllowedReaders(allowedInnerReaders)))
                    .FirstOrDefault(readingResult => readingResult.token != null);
                if (innerRead == 0) return (null, 0);
                innerTokens.Add(innerToken);
                currentIdx += innerRead;
            } while (!CanReadEnd(text, currentIdx));

            currentIdx += TagSymbols.Length;
            return (new PairedTagToken(TagName, innerTokens), currentIdx - offset);
        }

        private bool CanReadStart(string text, int idx)
        {
            var symbolAfterTagIdx = idx + TagSymbols.Length;
            var symbolBeforeTagIdx = idx - 1;
            return text.StartsWith(TagSymbols, idx) &&
                   (symbolAfterTagIdx >= text.Length || !char.IsWhiteSpace(text[symbolAfterTagIdx])) &&
                   (symbolAfterTagIdx >= text.Length || symbolBeforeTagIdx < 0 ||
                    !(char.IsLetterOrDigit(text[symbolBeforeTagIdx]) && char.IsDigit(text[symbolAfterTagIdx])));
        }

        private bool CanReadEnd(string text, int idx)
        {
            var symbolAfterTagIdx = idx + TagSymbols.Length;
            var symbolBeforeTagIdx = idx - 1;
            return text.StartsWith(TagSymbols, idx) &&
                   (idx == 0 || !char.IsWhiteSpace(text[symbolBeforeTagIdx])) &&
                   (symbolAfterTagIdx >= text.Length ||
                    !(char.IsLetterOrDigit(text[symbolBeforeTagIdx]) && char.IsDigit(text[symbolAfterTagIdx])));
        }
    }
}