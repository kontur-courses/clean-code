using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownTextTokenizer
    {
        public IEnumerable<Token> GetTokens(string rawText)
        {
            IReadingState currentState = new RawText();

            foreach (var currentSymbol in rawText)
            {
                var newState = currentState.ProcessSymbol(currentSymbol);
                if (newState.GetType() != currentState.GetType())
                {
                    foreach (var token in currentState.GetContentTokens())
                        yield return token;
                }
                currentState = newState;
            }

            foreach (var token in currentState.GetContentTokens())
                    yield return token;
        }
    }
}

