using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenParser
    {
        // void ParseParagraph(string paragraph);

        // Метод для валидации двух множеств отрезков токенов на пересечение и вложенность
        // возможно, стоит убрать в другое место
        public (IEnumerable<TokenSegment>, IEnumerable<TokenSegment>) ValidatePairSetsByRules((IEnumerable<TokenSegment>, IEnumerable<TokenSegment>) pair);

        string ReplaceTokens(IEnumerable<TokenSegment> tokens, ITokenTranslator translator);

        Dictionary<int, TokenInfo> FindAllTokens(string paragraph);
    }
}