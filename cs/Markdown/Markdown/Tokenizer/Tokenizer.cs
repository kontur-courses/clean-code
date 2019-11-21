using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Tokenizer
    {
        private Dictionary<string, MdElement> elementSigns;

        private TokenizerHelper helper;

        public Tokenizer(Dictionary<string, MdElement> elementSigns)
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
            var encloser = new Encloser(elementSigns);
            tokens = encloser.Enclose(tokens);
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
                var tokenType = helper.GetTokenType(symbol.ToString(), elementSigns);
                var token = new Token(symbol.ToString(), tokenType);
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
    }
}
