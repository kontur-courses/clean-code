using System.Collections.Generic;

namespace Markdown
{
    public class TokenizerHelper
    {
        public bool isScreen { get; private set; }

        public TokenizerHelper()
        {
            isScreen = false;
        }

        public void ScreenNext() => isScreen = true;

        public TokenType GetTokenType(char symbol, Dictionary<char, MdElement> elementSigns)
        {
            TokenType result = TokenType.Text;
            if (symbol == ' ')
                result = TokenType.WhiteSpace;
            else if (symbol == '\\' && isScreen)
                result = TokenType.Text;
            else if (elementSigns.ContainsKey(symbol))
                result = isScreen ? TokenType.Text : TokenType.MdElement;
            isScreen = false;
            return result;
        }

        public void EncloseToken(Token token, Dictionary<char, Stack<Token>> enclosing)
        {
            enclosing[token.Value].Pop().IsClosed = true;
            token.IsClosed = true;
        }

        public void AddOpeningElementForEnclosing(Token token, Dictionary<char, Stack<Token>> enclosing)
        {
            if (enclosing.ContainsKey(token.Value))
                enclosing[token.Value].Push(token);
            else
            {
                enclosing.Add(token.Value, new Stack<Token>());
                enclosing[token.Value].Push(token);
            }
        }

        public bool IsMdElementOpening(int i, List<Token> tokens) =>
           i == 0 || tokens[i - 1].Type == TokenType.WhiteSpace
           || tokens[i - 1].MdPosition == MdPosition.Opening;

        public bool IsMdElementEnclosing(int i, List<Token> tokens) =>
            i == tokens.Count - 1 || tokens[i + 1].Type == TokenType.WhiteSpace
            || tokens[i + 1].MdPosition == MdPosition.Enclosing;

        public Token InspectToken(Token token, MdPosition inspectedPosition,
            bool isTokenOfInspectedPosition, Dictionary<char, MdElement> elementSigns)
        {
            if (token.Type == TokenType.MdElement && isTokenOfInspectedPosition)
            {
                token.MdType = elementSigns[token.Value];
                token.MdPosition = inspectedPosition;
            }
            return token;
        }
    }
}
