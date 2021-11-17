using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenParser
    {
        string ReplaceTokens(IEnumerable<Token> tokens, ITokenTranslator translator);
        
        void SetShieldingSymbol(char symbol);
        
        void AddIntersectRules(Token firstToken, Token secondToken, bool canIntersect);

        void AddNestingRules(Token outsideToken, Token insideToken, bool canNested);

        IEnumerable<ITokenSegment> GetTokensSegments(Dictionary<int, Token> tokens);

        Dictionary<int, Token> GetSymmetricTokens();
    }
}