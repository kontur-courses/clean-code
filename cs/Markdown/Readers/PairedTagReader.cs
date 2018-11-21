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

            if (!CanReadTagSymbols(text, offset, inStart: true))
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
            } while (!CanReadTagSymbols(text, currentIdx, inStart: false));

            currentIdx += TagSymbols.Length;
            return (new PairedTagToken(TagName, innerTokens), currentIdx - offset);
        }
        
        private bool CanReadTagSymbols(string text, int offset, bool inStart)
        {
            var symbolAfterTagIdx = offset + TagSymbols.Length;
            var symbolBeforeTagIdx = offset - 1;
            return text.StartsWith(TagSymbols, offset) &&
                   (symbolAfterTagIdx >= text.Length || symbolBeforeTagIdx < 0 ||
                    !(char.IsLetterOrDigit(text[symbolBeforeTagIdx]) && char.IsDigit(text[symbolAfterTagIdx]))) &&
                   (inStart
                       ? symbolAfterTagIdx >= text.Length || !char.IsWhiteSpace(text[symbolAfterTagIdx])
                       : offset == 0 || !char.IsWhiteSpace(text[symbolBeforeTagIdx]));
        }
    }
}