using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class LinkReader : AbstractReader
    {
        private const string StartSymbols = "[";
        private const string MiddleSymbols = "](";
        private const string EndSymbols = ")";

        private readonly TextReader pureLinkReader = new TextReader(")");

        public override (IToken token, int length) ReadToken(string text, int offset, ReadingOptions options)
        {
            CheckArguments(text, offset);
            var innerTokens = new List<IToken>();
            var currentIdx = offset;

            if (!text.StartsWith(StartSymbols, currentIdx)) return (null, 0);
            currentIdx += StartSymbols.Length;
            var allowedInnerReaders = options.GetAvailableInnerReadersFor(this);
            do
            {
                var (innerToken, innerTokenLength) = allowedInnerReaders
                    .Select(reader => reader.ReadToken(text, currentIdx, options.WithAllowedReaders(allowedInnerReaders)))
                    .FirstOrDefault(readingResult => readingResult.token != null);
                if (innerTokenLength == 0) return (null, 0);
                innerTokens.Add(innerToken);
                currentIdx += innerTokenLength;
            } while (!text.StartsWith(MiddleSymbols, currentIdx));

            currentIdx += MiddleSymbols.Length;
            var (token, length) = pureLinkReader.ReadToken(text, currentIdx, options);
            currentIdx += length;
            return text.StartsWith(EndSymbols, currentIdx)
                ? (new LinkToken(innerTokens, (token as TextToken)?.Value ?? ""), currentIdx + EndSymbols.Length)
                : (null, 0);
        }
    }
}