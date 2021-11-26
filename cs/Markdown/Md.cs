using Markdown.Interfaces;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenCreator tokenCreator;
        private readonly Reducer reducer;
        private readonly ITokenParser tokenParser;
        private readonly ITokenRenderer tokenRenderer;

        public Md(ITokenCreator tokenCreator, Reducer reducer, ITokenParser tokenParser, ITokenRenderer tokenRenderer)
        {
            this.tokenCreator = tokenCreator;
            this.reducer = reducer;
            this.tokenParser = tokenParser;
            this.tokenRenderer = tokenRenderer;
        }

        public string Render(string text)
        {
            var tokens = tokenCreator.Create(text);
            var reducedTokens = reducer.Reduce(tokens);
            var parsedTokens = tokenParser.Parse(reducedTokens);
            return tokenRenderer.Render(parsedTokens);
        }
    }
}