using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class PairedTagReader : IReader
    {
        public string TagName { get; }
        public string TagSymbols { get; }

        public PairedTagReader(string tagName, string tagSymbols)
        {
            TagName = tagName;
            TagSymbols = tagSymbols;
        }

        public (IToken, int) ReadToken(string text, int idx, ReadingOptions options)
        {
            var innerTokens = new List<IToken>();
            var currentIdx = idx;

            if (!CanReadStart(text, idx))
                return (null, 0);
            currentIdx += TagSymbols.Length;
            var allowedInnerReaders = options.GetAvailableInnerReadersFor(this);
            do
            {
                (IToken token, int read) readingResult = (null, 0);
                foreach (var innerReader in allowedInnerReaders)
                {
                    readingResult = innerReader.ReadToken(
                        text, currentIdx, options.UpdateAllowedReaders(allowedInnerReaders));
                    if (readingResult.token == null) continue;
                    innerTokens.Add(readingResult.token);
                    currentIdx += readingResult.read;
                    break;
                }
                if (readingResult.token == null) return (null, 0);
            } while (!CanReadEnd(text, currentIdx));

            currentIdx += TagSymbols.Length;
            return (new PairedTagToken(TagName, innerTokens), currentIdx - idx);
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