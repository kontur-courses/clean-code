using Markdown.Extensions.HandlerExtensions;
using Markdown.Extensions.StringExtensions;
using Markdown.Tokens.HtmlTokens;

namespace Markdown.Parsers.MdParsers
{
    internal class ParserMd : IParser
    {
        private const string BoldMarkerStart = "__";
        private const string ItalicMarkerStart = "_";
        private const string HeaderMarkerStart = "# ";

        private static readonly HashSet<char> _markers = new HashSet<char>()
        {
            '_',
            '\n',
            '#'
        };

        public IList<IRenderable> Parse(string text)
        {
            return ParseTextPart(text);
        }

        private static List<IRenderable> ParseTextPart(string text)
        {
            var tokens = new List<IRenderable>();
            var index = 0;
            while (index < text.Length)
            {
                if (text.IsEscapeStart(index, _markers))
                {
                    index = ParseEscape(text, index, tokens);
                }
                else if (text.IsHeaderStart(index, HeaderMarkerStart))
                {
                    index = ParseHeader(text, index, tokens);
                }
                else if (text.IsBoldStart(index, ItalicMarkerStart))
                {
                    index = ParseBold(text, index, tokens);
                }
                else if (text.IsItalicStart(index, ItalicMarkerStart))
                {
                    index = ParseItalic(text, index, tokens);
                }
                else
                {
                    index = ParseText(text, index, tokens);
                }
            }
            return tokens;
        }

        private static int ParseEscape(string text, int index, List<IRenderable> tokens)
        {
            tokens.Add(new TextToken(text[index + 1].ToString()));
            return index + 2;
        }

        private static int ParseHeader(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index + HeaderMarkerStart.Length;
            var endIndex = text.FindEndOfLine(startIndex);
            var innerTokens = ParseTextPart(text.Substring(startIndex, endIndex - startIndex));
            endIndex += 1;
            tokens.Add(new HeaderToken(WrapInnerTokens(innerTokens)));
            return endIndex;
        }

        private static int ParseItalic(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index + 1;

            if (startIndex >= text.Length
                || text.IsSurroundedByDigits(index, 1)
                || char.IsWhiteSpace(text[startIndex]))
            {
                tokens.Add(new TextToken(ItalicMarkerStart));
                return startIndex;
            }

            var endIndex = text.FindClosingMarker(startIndex,
                ItalicMarkerStart);
            if (endIndex == -1)
            {
                tokens.Add(new TextToken(ItalicMarkerStart));
                return startIndex;
            }

            var innerTokens = ParseTextPart(text.Substring(startIndex, endIndex - startIndex));
            CorrectBoldTokensInsideItalicToken(innerTokens);
            tokens.Add(new ItalicToken(WrapInnerTokens(innerTokens)));
            return endIndex + ItalicMarkerStart.Length;
        }

        private static void CorrectBoldTokensInsideItalicToken(IList<IRenderable> innerTokens)
        {
            for (var i = 0; i < innerTokens.Count; i++)
            {
                if (innerTokens[i] is BoldToken boldToken)
                {
                    var innerTextToken = (TextToken)boldToken.InnerItem;
                    innerTokens[i] = new TextToken(
                        $"{BoldMarkerStart}{innerTextToken.Text}{BoldMarkerStart}");
                }
            }
        }

        private static int ParseBold(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index + BoldMarkerStart.Length;

            if (startIndex >= text.Length
                || text.IsSurroundedByDigits(index, BoldMarkerStart.Length)
                || char.IsWhiteSpace(text[startIndex]))
            {
                tokens.Add(new TextToken(BoldMarkerStart));
                return startIndex;
            }

            var endIndex = text.FindClosingMarker(startIndex, BoldMarkerStart);
            if (endIndex == -1)
            {
                tokens.Add(new TextToken(BoldMarkerStart));
                return startIndex;
            }

            var innerTokens = ParseTextPart(text.Substring(startIndex, endIndex - startIndex));
            tokens.Add(new BoldToken(WrapInnerTokens(innerTokens)));
            return endIndex + BoldMarkerStart.Length;
        }

        private static int ParseText(string text, int index, List<IRenderable> tokens)
        {
            var startIndex = index;

            while (index < text.Length && !_markers.Contains(text[index]))
            {
                if (text.IsEscapeStart(index, _markers))
                {
                    if (index + 1 < text.Length && char.IsLetterOrDigit(text[index + 1]))
                    {
                        index += 1;
                        continue;
                    }
                    break;
                }
                index += 1;
            }

            if (index == startIndex)
            {
                index += 1;
            }
            tokens.Add(new TextToken(text.Substring(startIndex, index - startIndex)));
            return index;
        }

        private static IRenderable WrapInnerTokens(IList<IRenderable> innerTokens)
        {
            if (innerTokens.Count == 1)
            {
                return innerTokens[0];
            }
            return new SetToken(innerTokens.ToArray());
        }
    }
}