using System.Collections.Generic;
using System.Linq;
using MarkDown.TagTypes;

namespace MarkDown
{
    public class MarkDownParser
    {
        private readonly List<TagType> availableTagTypes;
        private readonly TextStream textStream;

        public MarkDownParser(string textStream, IEnumerable<TagType> availableTagTypes)
        {
            this.textStream = new TextStream(textStream);
            this.availableTagTypes = availableTagTypes.ToList();
        }

        public IEnumerable<Token> GetTokens()
        {
            var textTokenStart = textStream.CurrentPosition;
            while (textStream.CurrentPosition < textStream.Length)
            {
                if (TryGetTagToken(out var tagToken))
                {
                    if (textTokenStart != tagToken.Position && textStream.TryGetSubstring(textTokenStart, 
                            tagToken.Position - textTokenStart, out var possibleContent))
                        yield return new Token(textTokenStart, possibleContent);
                    var nestedTagTypes = tagToken.TagType.GetNestedTagTypes(availableTagTypes);
                    tagToken.InnerTokens = nestedTagTypes.Any() ? new MarkDownParser(tagToken.Content, nestedTagTypes).GetTokens() 
                        : new[] {new Token(0, tagToken.Content)};
                    yield return tagToken;
                    if (tagToken.Content == "")
                    {
                        textTokenStart = textStream.CurrentPosition;
                        continue;
                    };
                    textStream.TryMoveNext(tagToken.Length);
                    textTokenStart = textStream.CurrentPosition;
                    continue;
                }
                textStream.TryMoveNext();
            }

            if (textTokenStart != textStream.CurrentPosition && textStream
                    .TryGetSubstring(textTokenStart, textStream.CurrentPosition - textTokenStart, out var content))
                yield return new Token(textTokenStart,content);
        }

        private bool IsCurrentParameter(out TagType param)
        {
            param = availableTagTypes
                .Where(t => t.Parameter != null && t.Parameter.OpeningSymbol.Length + t.Parameter.ClosingSymbol.Length <= textStream.Length)
                .FirstOrDefault(t => textStream
                    .IsCurrentOpening(t.Parameter.OpeningSymbol, availableTagTypes.Select(s => s.OpeningSymbol)));
            return param != null;
        }

        private bool TryGetTagType(out TagType tagType)
        {               
            tagType = availableTagTypes
                .Where(t => t.OpeningSymbol.Length + t.ClosingSymbol.Length <= textStream.Length)
                .OrderByDescending(t => t.OpeningSymbol)
                .FirstOrDefault(t => textStream
                    .IsCurrentOpening(t.OpeningSymbol, availableTagTypes.Select(s => s.OpeningSymbol)));
            return tagType != null;
        }

        private bool TryGetTagToken(out Token token)
        {
            if (!IsCurrentParameter(out var tagType))
            {
                if (!textStream.Contains("[") || !textStream.Contains("]"))
                    return TryGetParametrizedTagToken(out token);
                token = null;
                return false;
            }
            var paramSymbols = availableTagTypes.Where(s => s.ClosingSymbol != tagType.Parameter.ClosingSymbol)
                .Select(s => s.ClosingSymbol);
            return textStream.TryReadUntilClosing(tagType.Parameter.ClosingSymbol, paramSymbols, out var paramContent) 
                ? TryGetParametrizedTagToken(out token, tagType, paramContent) 
                : TryGetParametrizedTagToken(out token);
        }

        private bool TryGetParametrizedTagToken(out Token token, TagType parametrizedTagType = null, string paramContent = "")
        {
            token = null;
            var position = paramContent != "" 
                ? textStream.CurrentPosition - paramContent.Length - parametrizedTagType.Parameter.OpeningSymbol.Length 
                  - parametrizedTagType.Parameter.ClosingSymbol.Length
                : textStream.CurrentPosition;
            if (!TryGetTagType(out var tagType) && parametrizedTagType == null) return false;
            if (parametrizedTagType != null && (tagType == null || tagType.GetType() != parametrizedTagType.GetType()))
            {
                token = new Token(position, "", parametrizedTagType, paramContent);
                return true;
            }
            var closingSymbol = tagType.ClosingSymbol;
            var symbols = availableTagTypes.Where(s => s.ClosingSymbol != closingSymbol)
                .Select(s => s.ClosingSymbol);
            if (!textStream.TryReadUntilClosing(closingSymbol, symbols, out var tokenContent, true))
                return token != null;
            token = new Token(position, tokenContent, tagType, paramContent);
            return token != null;
        }
        
    }
}
