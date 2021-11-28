namespace Markdown
{
    public class Md
    {
        private readonly TokenCreator tokenCreator;
        private readonly Reducer reducer;
        private readonly TokenParser tokenParser;
        private readonly Renderer tokenRenderer;

        public Md(TokenCreator tokenCreator, Reducer reducer, TokenParser tokenParser, Renderer tokenRenderer)
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