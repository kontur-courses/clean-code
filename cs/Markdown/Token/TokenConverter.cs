using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenConverter
    {
        private TokenProvider tokenProvider;
        private string source;

        public IEnumerable<Token> GetTokens() => FilterTokens(tokenProvider.GetAllTokens());

        public string Build()
        {
            var builder = source.Take(source.Length - 1)
                .Select(symbol => symbol.ToString()).ToList();

            foreach (var token in FilterTokens(tokenProvider.GetAllTokens()))
                token.Tag.Replace(
                    builder,
                    token.StartPosition,
                    token.StartPosition + token.Length - 1
                );

            return string.Join("", builder);
        }

        public TokenConverter Initialize(string markup)
        {
            source = markup + ' ';
            tokenProvider = TokenProvider
                .Create()
                .InitTokenProvider(source);
            return this;
        }

        public TokenConverter FindTokens()
        {
            GetBuildsMachine()
                .Run(source);
            return this;
        }

        private Machine GetBuildsMachine()
        {
            var initializer = StateInitializer
                .Create(tokenProvider)
                .Initialize();

            return Machine.Create(initializer.GetStartState());
        }

        private IEnumerable<Token> FilterTokens(IEnumerable<Token> anyTokens)
        {
            return anyTokens.Where(t => !t.Tag.IsBrokenMarkup(source, t.StartPosition, t.Length));
        }
    }
}