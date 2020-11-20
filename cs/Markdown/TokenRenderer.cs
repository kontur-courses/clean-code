using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenRenderer
    {
        public readonly string Text;

        private readonly Dictionary<Type, Func<TokenRenderer, Token, string>> tokens =
            new Dictionary<Type, Func<TokenRenderer, Token, string>>();

        protected TokenRenderer(string text)
        {
            Text = text;
            AddToken<RawTextToken>((r, t) => r.Text.Substring(t.StartPosition, t.Length));
        }

        public void AddToken<TToken>(Func<TokenRenderer, TToken, string> renderFunc) where TToken : Token
            => tokens[typeof(TToken)] = (r, t) => renderFunc(r, t as TToken);

        public void RemoveToken<TToken>() => tokens.Remove(typeof(TToken));

        public string Render(Token token) => tokens[token.GetType()](this, token);

        public string RenderAll(IEnumerable<Token> tokens) => string.Concat(tokens.Select(Render));

        public string RenderSubtokens(TokenWithSubTokens token)
            => string.Concat(token.EnumerateSubtokens().Select(Render));
    }
}