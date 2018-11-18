using System.Collections.Generic;
using System.Linq;
using MarkDown.TagTypes;

namespace MarkDown
{
    public class MarkDownParser
    {
        private readonly List<TagType> availableTagTypes;
        private readonly TextStream textStream;

        public MarkDownParser(TextStream textStream, IEnumerable<TagType> availableTagTypes)
        {
            this.textStream = textStream;
            this.availableTagTypes = availableTagTypes.ToList();
        }
        
        public IEnumerable<Token> GetTokens()
        {
            var textTokenStart = textStream.CurrentPosition;
            while (textStream.CurrentPosition < textStream.Length)
            {
                if (TryGetTagToken(out var tagToken))
                {
                    if (textTokenStart != textStream.CurrentPosition && textStream.TryGetSubstring(textTokenStart, 
                            textStream.CurrentPosition - textTokenStart, out var posContent))
                        yield return new Token(textTokenStart, posContent);
                    yield return tagToken;

                    textStream.TryMoveNext(tagToken.Length);
                    textTokenStart = textStream.CurrentPosition;

                    continue;
                }
                textStream.TryMoveNext();
            }
            if (textTokenStart == textStream.CurrentPosition) yield break;
            if (textStream.TryGetSubstring(textTokenStart, textStream.CurrentPosition - textTokenStart, out var content))
                yield return new Token(textTokenStart,content);
        }

        private bool TryGetTagType(out TagType tagType)
        {
            tagType = availableTagTypes
                .Where(t => t.SpecialSymbol.Length * 2 <= textStream.Length)
                .OrderByDescending(s => s.SpecialSymbol)
                .FirstOrDefault(t => textStream
                    .IsCurrentOpening(t.SpecialSymbol, availableTagTypes
                        .Where(s => s.SpecialSymbol != t.SpecialSymbol).Select(s => s.SpecialSymbol)));
            return tagType != null;
        }

        private bool TryGetTagToken(out Token token)
        {
            token = null;
            if (!TryGetTagType(out var tagType)) return false;
            var specialSymbol = tagType.SpecialSymbol;
            for (var i = textStream.CurrentPosition + 2; i < textStream.Length - specialSymbol.Length + 1; i++)
            {
                var symbols = availableTagTypes.Where(s => s.SpecialSymbol != specialSymbol).Select(s => s.SpecialSymbol);
                if (!textStream.IsSymbolAtPositionClosing(i, specialSymbol, symbols)) continue;
                var startPosition = textStream.CurrentPosition + specialSymbol.Length;
                var length = i - textStream.CurrentPosition - specialSymbol.Length;
                if (!textStream.TryGetSubstring(startPosition, length, out var content)) continue;
                if (textStream.IsTokenAtCurrentNumberLess(i))
                    token = new Token(textStream.CurrentPosition, content, tagType);
            }
            return token != null;
        }
    }
}
