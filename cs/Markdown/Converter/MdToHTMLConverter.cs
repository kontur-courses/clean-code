using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal class MdToHTMLConverter : IConverter
    {
        private readonly Dictionary<int, Queue<(string, int)>> tagsForInserting;

        internal MdToHTMLConverter()
        {
            tagsForInserting = new Dictionary<int, Queue<(string, int)>>();
        }

        public string Convert(string mdText, IEnumerable<Token> mdTokens, TextInfo escapingInfo)
        {
            var htmlTokens = ConvertTokens(mdTokens);
            SetTagsForInserting(htmlTokens);
            return TokensToHTMLText(mdText, escapingInfo);
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
            foreach (var token in tokens.OrderBy(token => token.TokenStart))
            {
                if (!tagsForInserting.ContainsKey(token.TokenStart))
                    tagsForInserting[token.TokenStart] = new Queue<(string, int)>();
                tagsForInserting[token.TokenStart].Enqueue((token.TokenStyle.StartTag, token.ContentStart));
                if (!tagsForInserting.ContainsKey(token.ContentStart + token.ContentLength))
                    tagsForInserting[token.ContentStart + token.ContentLength] = new Queue<(string, int)>();
                tagsForInserting[token.ContentStart + token.ContentLength].Enqueue((token.TokenStyle.EndTag,
                    token.TokenStart + token.TokenLength));
            }
        }

        private string TokensToHTMLText(string mdText, TextInfo escapingInfo)
        {
            var htmlText = new StringBuilder();
            var ptr = 0;
            while (true)
            {
                if (tagsForInserting.ContainsKey(ptr))
                {
                    var (tag, nextPtr) = tagsForInserting[ptr].Dequeue();
                    if (tagsForInserting[ptr].Count == 0)
                        tagsForInserting.Remove(ptr);
                    htmlText.Append(tag);
                    ptr = nextPtr;
                    continue;
                }

                if (ptr >= mdText.Length && tagsForInserting.Count == 0)
                    break;
                if (!escapingInfo.EscapeCharPositions.Contains(ptr))
                    htmlText.Append(mdText[ptr]);
                ptr++;
            }

            return htmlText.ToString();
        }
    }
}