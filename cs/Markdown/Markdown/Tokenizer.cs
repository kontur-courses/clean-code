using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public enum TokenType
    {
        Text,
        MdElement,
        WhiteSpace
    }

    public enum MdPosition
    {
        None,
        Opening,
        Enclosing
    }

    public class Token
    {
        public char Value;
        public TokenType Type;
        public MdElement MdType;
        public MdPosition MdPosition;
        public bool IsClosed;

        public Token(char value, TokenType type)
        {
            Type = type;
            Value = value;
            MdPosition = MdPosition.None;
            IsClosed = false;
        }
    }

    public class Tokenizer
    {
        private Dictionary<char, MdElement> elementSigns;

        private bool isScreen = false;

        private TokenType GetTokenType(char symbol)
        {
            if (symbol == ' ')
                return TokenType.WhiteSpace;
            else if (symbol == '\\' && isScreen)
            {
                isScreen = false;
                return TokenType.Text;
            }
            else if (elementSigns.ContainsKey(symbol))
            {
                if (isScreen)
                {
                    isScreen = false;
                    return TokenType.Text;
                }
                return TokenType.MdElement;
            }
            else
                return TokenType.Text;
        }

        public Tokenizer(Dictionary<char, MdElement> elementSigns)
        {
            this.elementSigns = elementSigns;
        }

        public List<Token> Tokenize(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            var tokens = GetSimpleTokens(text);
            tokens = InspectMdTokensPosition(tokens);
            tokens = CheckMdTokensForEnclosing(tokens);
            return tokens;
        }

        private List<Token> GetSimpleTokens(string text)
        {
            var tokens = new List<Token>();
            foreach (var symbol in text)
            {
                if (symbol == '\\' && !isScreen)
                {
                    isScreen = true;
                    continue;
                }
                tokens.Add(new Token(symbol, GetTokenType(symbol)));
            }
            return tokens;
        }

        private List<Token> InspectMdTokensPosition(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; ++i)
            {
                tokens[i] = InspectToken(tokens[i], MdPosition.Opening, isMdElementOpening(i, tokens));
            }
            for (int i = tokens.Count - 1; i > -1; --i)
            {
                tokens[i] = InspectToken(tokens[i], MdPosition.Enclosing, isMdElementEnclosing(i, tokens));
            }
            for (int i = 0; i < tokens.Count; ++i)
                if (tokens[i].Type == TokenType.MdElement && tokens[i].MdPosition == MdPosition.None)
                    tokens[i].Type = TokenType.Text;
            return tokens;
        }

        private Token InspectToken(Token token, MdPosition inspectedPosition, bool isTokenOfInspectedPosition)
        {
            if (token.Type == TokenType.MdElement && isTokenOfInspectedPosition)
            {
                token.MdType = elementSigns[token.Value];
                token.MdPosition = inspectedPosition;
            }
            return token;
        }

        private bool isMdElementOpening(int i, List<Token> tokens) =>
            i == 0 || tokens[i - 1].Type == TokenType.WhiteSpace 
            || tokens[i - 1].MdPosition == MdPosition.Opening;

        private bool isMdElementEnclosing(int i, List<Token> tokens) =>
            i == tokens.Count - 1 || tokens[i + 1].Type == TokenType.WhiteSpace
            || tokens[i + 1].MdPosition == MdPosition.Enclosing;

        private List<Token> CheckMdTokensForEnclosing(List<Token> tokens)
        {
            var enclosing = new Dictionary<char, Stack<Token>>();
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.MdElement)
                {
                    if (!token.MdType.IsEnclosed)
                        token.IsClosed = true;
                    else if (token.MdPosition == MdPosition.Opening)
                    {
                        if (enclosing.ContainsKey(token.Value))
                            enclosing[token.Value].Push(token);
                        else
                        {
                            enclosing.Add(token.Value, new Stack<Token>());
                            enclosing[token.Value].Push(token);
                        }
                    }
                    else if (token.MdPosition == MdPosition.Enclosing && enclosing.ContainsKey(token.Value))
                    {
                        enclosing[token.Value].Pop().IsClosed = true;
                        token.IsClosed = true;
                    }
                }
            }
            return tokens;
        }
    }
}
