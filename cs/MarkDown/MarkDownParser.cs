using System.Collections.Generic;
using System.Linq;
using MarkDown.TagTypes;

namespace MarkDown
{
    public class MarkDownParser
    {
        private int currentPosition;
        private readonly List<TagType> availableTagTypes;
        private readonly string text;

        public MarkDownParser(string text, IEnumerable<TagType> availableTagTypes)
        {
            this.text = text;
            this.availableTagTypes = availableTagTypes.ToList();
        }
        public IEnumerable<Token> GetTokens()
        {
            currentPosition = 0;
            var textTokenStart = currentPosition;
            var result = new List<Token>();
            while (currentPosition < text.Length)
            {
                var tagToken = GetTagToken();
                if (tagToken != null)
                {
                    var possibleTextTag = new Token(textTokenStart, text.Substring(textTokenStart, currentPosition - textTokenStart));
                    result.ConditionalAdd(textTokenStart != currentPosition, possibleTextTag);
                    result.Add(tagToken);

                    currentPosition += tagToken.Length;
                    textTokenStart = currentPosition;

                    continue;
                }
                currentPosition++;
            }

            if(!result.Any() || textTokenStart != currentPosition)
                result.Add(new Token(textTokenStart, text.Substring(textTokenStart, currentPosition - textTokenStart)));

            return result;
        }

        private TagType GetTagType() =>
            availableTagTypes
                .Where(t => t.SpecialSymbol.Length * 2 <= text.Length)
                .FirstOrDefault(t => text.IsOpeningTag(currentPosition, t.SpecialSymbol));

        private Token GetTagToken()
        {
            var tagType = GetTagType();
            if (tagType == null) return null;
            var specialSymbol = tagType.SpecialSymbol;
            for (var i = currentPosition + 2; i < text.Length - specialSymbol.Length + 1; i++)
            {
                if (!text.IsClosingTag(i, specialSymbol)) continue;
                var content = text.Substring(currentPosition + specialSymbol.Length,
                    i - currentPosition - specialSymbol.Length);
                
                return IsNumberLessToken(i) ? new Token(currentPosition, content, tagType) : null;
            }
            
            return null;
        }

        private bool IsNumberLessToken(int endPosition)
        {
            return !(text.Substring(0, currentPosition).Split().Last().Any(char.IsDigit)
                && text.Substring(endPosition).Split().First().Any(char.IsDigit));
        }
    }
}
