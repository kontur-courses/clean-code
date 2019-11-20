using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.RenderUtilities
{
    public class Renderer
    {
        private readonly Func<IEnumerable<ITokenProcessor>> getProcessors;

        public Renderer(Func<IEnumerable<ITokenProcessor>> processorsGetter)
        {
            getProcessors = processorsGetter;
        }

        private List<ITokenProcessor> InitProcessors(List<Token> tokens)
        {
            var processors = getProcessors().ToList();
            processors.ForEach(processor => processor.ProcessTokens(tokens));

            return processors;
        }

        public string RenderText(List<Token> tokens)
        {
            var processors = InitProcessors(tokens);
            var typeToProcessor = processors
                .SelectMany(p => p.AcceptedTokenTypes)
                .ToDictionary(tknType => tknType,
                    tknType => processors
                               .Where(p => p.AcceptedTokenTypes.Contains(tknType)).FirstOrDefault());

            StringBuilder result = new StringBuilder();
            for (var i = 0; i < tokens.Count; i++)
            {
                string tokenString = null;
                if (typeToProcessor[tokens[i].TokenType].TryGetRenderedTokenText(tokens, i, out tokenString))
                    result.Append(tokenString);
            }

            return result.ToString();
        }
    }
}
