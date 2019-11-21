using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown.RenderUtilities
{
    public class Renderer
    {
        private readonly Func<List<ITokenProcessor>> getProcessors;

        public Renderer(Func<List<ITokenProcessor>> processorsGetter)
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
                               .FirstOrDefault(p => p.AcceptedTokenTypes.Contains(tknType)));

            var result = new StringBuilder();
            for (var i = 0; i < tokens.Count; i++)
            {
                if (typeToProcessor[tokens[i].TokenType].TryGetRenderedTokenText(tokens, i, out var tokenString))
                    result.Append(tokenString);
            }

            return result.ToString();
        }
    }
}
