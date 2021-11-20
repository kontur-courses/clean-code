using System;
using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenParser
    {
        // void ParseParagraph(string paragraph);

        // Метод для валидации двух множеств отрезков токенов на пересечение и вложенность
        // возможно, стоит убрать в другое место
        public (IEnumerable<ITokenSegment>, IEnumerable<ITokenSegment>) ValidatePairSets((IEnumerable<ITokenSegment>, IEnumerable<ITokenSegment>) pair);

        string ReplaceTokens(IEnumerable<ITokenSegment> tokens, ITokenTranslator translator);

        IEnumerable<ITokenSegment> GetTokensSegments(Dictionary<int, TokenInfo> tokens);

        Dictionary<int, TokenInfo> FindAllTokens(string paragraph);
    }
}