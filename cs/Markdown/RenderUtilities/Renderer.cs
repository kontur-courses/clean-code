using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public class Renderer
    {
        private readonly List<ITokenHandler> handlers;
        private readonly Dictionary<TokenType, ITokenHandler> typeToHandler;

        public Renderer(IEnumerable<ITokenHandler> handlers)
        {
            this.handlers = handlers.ToList();
            typeToHandler = handlers
                .SelectMany(h => h.GetAcceptedTokenTypes())
                .ToDictionary(tknType => tknType,
                tknType => handlers
                           .Where(h => h.GetAcceptedTokenTypes().Contains(tknType)).FirstOrDefault());
        }

        public string RenderText(List<Token> tokens)
        {
            for (var i = 0; i < tokens.Count; i++)
                typeToHandler[tokens[i].TokenType].HandleToken(tokens, i);
            StringBuilder result = new StringBuilder();
            for (var i = 0; i < tokens.Count; i++)
            {
                string tokenString = null;
                if (typeToHandler[tokens[i].TokenType].TryGetTokenString(tokens, i, out tokenString))
                    result.Append(tokenString);
            }

            return result.ToString();
        }
    }
}
