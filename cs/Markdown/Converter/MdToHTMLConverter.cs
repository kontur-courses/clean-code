using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal class MdToHTMLConverter : IConverter
    {
        private readonly Dictionary<int, Stack<(string startTag, int nextPointer)>> endTagsForInserting;
        private readonly Dictionary<int, Queue<(string startTag, int nextPointer)>> startTagsForInserting;
        private IComparer<Token> comparer;

        internal MdToHTMLConverter()
        {
            startTagsForInserting = new Dictionary<int, Queue<(string startTag, int nextPointer)>>();
            endTagsForInserting = new Dictionary<int, Stack<(string startTag, int nextPointer)>>();
        }

        public string Convert(string mdText, IEnumerable<Token> mdTokens, TextInfo textInfo)
        {
            var htmlTokens = ConvertTokens(mdTokens);
            SetTagsForInserting(htmlTokens);
            return TokensToHTMLText(mdText, textInfo);
        }

        public MdToHTMLConverter UsingComparer(IComparer<Token> comparer)
        {
            this.comparer = comparer;
            return this;
        }

        private static IEnumerable<Token> ConvertTokens(IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
                yield return new Token(
                    HTML.HTMLStyles[token.TokenStyle.Type],
                    token.TokenStart,
                    token.TokenLength,
                    token.ContentStart,
                    token.ContentLength);
        }

        private void SetTagsForInserting(IEnumerable<Token> tokens)
        {
            tokens = comparer == null
                ? tokens.OrderBy(token => token.TokenStart)
                : tokens.OrderBy(token => token, comparer);
            foreach (var token in tokens)
            {
                if (!startTagsForInserting.ContainsKey(token.TokenStart))
                    startTagsForInserting[token.TokenStart] = new Queue<(string startTag, int nextPointer)>();
                startTagsForInserting[token.TokenStart].Enqueue((token.TokenStyle.StartTag, token.ContentStart));
                if (!endTagsForInserting.ContainsKey(token.ContentStart + token.ContentLength))
                    endTagsForInserting[token.ContentStart + token.ContentLength] =
                        new Stack<(string startTag, int nextPointer)>();
                endTagsForInserting[token.ContentStart + token.ContentLength].Push((token.TokenStyle.EndTag,
                    token.TokenStart + token.TokenLength));
            }
        }

        private string TokensToHTMLText(string mdText, TextInfo escapingInfo)
        {
            var htmlText = new StringBuilder();
            var ptr = 0;
            while (true)
            {
                if (endTagsForInserting.ContainsKey(ptr))
                {
                    var (tag, nextPtr) = endTagsForInserting[ptr].Pop();
                    if (endTagsForInserting[ptr].Count == 0)
                        endTagsForInserting.Remove(ptr);
                    htmlText.Append(tag);
                    ptr = nextPtr;
                    continue;
                }

                if (startTagsForInserting.ContainsKey(ptr))
                {
                    var (tag, nextPtr) = startTagsForInserting[ptr].Dequeue();
                    if (startTagsForInserting[ptr].Count == 0)
                        startTagsForInserting.Remove(ptr);
                    htmlText.Append(tag);
                    ptr = nextPtr;
                    continue;
                }

                if (ptr >= mdText.Length)
                    break;
                if (!escapingInfo.EscapeCharPositions.Contains(ptr))
                    htmlText.Append(mdText[ptr]);
                ptr++;
            }

            return htmlText.ToString();
        }
    }
}