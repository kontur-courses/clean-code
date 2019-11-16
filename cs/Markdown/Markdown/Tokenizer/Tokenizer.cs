using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Tokenizer
    {
        private Dictionary<char, MdElement> elementSigns;

        private TokenizerHelper helper;

        public Tokenizer(Dictionary<char, MdElement> elementSigns)
        {
            this.elementSigns = elementSigns;
            helper = new TokenizerHelper();
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
                if (symbol == '\\' && !helper.isScreen)
                {
                    helper.ScreenNext();
                    continue;
                }
                var tokenType = helper.GetTokenType(symbol, elementSigns);
                var token = new Token(symbol, tokenType);
                tokens.Add(token);
            }
            return tokens;
        }

        private List<Token> InspectMdTokensPosition(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; ++i)
            {
                var isOpening = helper.IsMdElementOpening(i, tokens);
                tokens[i] = helper.InspectToken(tokens[i], MdPosition.Opening, isOpening, elementSigns);
            }
            for (int i = tokens.Count - 1; i > -1; --i)
            {
                var isEnclosing = helper.IsMdElementEnclosing(i, tokens);
                tokens[i] = helper.InspectToken(tokens[i], MdPosition.Enclosing, isEnclosing, elementSigns);
            }
            for (int i = 0; i < tokens.Count; ++i)
                if (tokens[i].Type == TokenType.MdElement && tokens[i].MdPosition == MdPosition.None)
                    tokens[i].Type = TokenType.Text;
            return tokens;
        }

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
                        helper.AddOpeningElementForEnclosing(token, enclosing);
                    }
                    else if (token.MdPosition == MdPosition.Enclosing && enclosing.ContainsKey(token.Value))
                    {
                        helper.EncloseToken(token, enclosing);
                    }
                }
            }
            return tokens;
        }
    }
}
