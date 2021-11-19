using System;
using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenParser
    {
        void ParseParagraph(string paragraph);

        // Метод для валидации двух множеств отрезков токенов на пересечение и вложенность
        // Убрать в метод расширения
        public (IEnumerable<ITokenSegment>, IEnumerable<ITokenSegment>) ValidatePairSets((IEnumerable<ITokenSegment>, IEnumerable<ITokenSegment>) pair);
        
        bool IsIntersectProhibitedOf(Token firstToken, Token secondToken);
        
        bool IsNestingProhibitedOf(Token outsideToken, Token insideToken);
        
        string ReplaceTokens(IEnumerable<ITokenSegment> tokens, ITokenTranslator translator);
        
        void SetShieldingSymbol(char symbol);
        
        void AddIntersectRule(Token firstToken, Token secondToken, bool canIntersect);

        void AddNestingRule(Token outsideToken, Token insideToken, bool canNested);

        IEnumerable<ITokenSegment> GetTokensSegments(Dictionary<int, TokenInfo> tokens);

        Dictionary<int, TokenInfo> FindAllTokens();
    }
}