using System;
using Markdown.Interfaces;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenCreator tokenCreator;
        private readonly ITokenParser tokenParser;
        private readonly ITokenRenderer tokenRenderer;

        public Md(ITokenCreator tokenCreator, ITokenParser tokenParser, ITokenRenderer tokenRenderer)
        {
            this.tokenCreator = tokenCreator;
            this.tokenParser = tokenParser;
            this.tokenRenderer = tokenRenderer;
        }

        public string Render(string text) => throw new NotImplementedException();
    }
}