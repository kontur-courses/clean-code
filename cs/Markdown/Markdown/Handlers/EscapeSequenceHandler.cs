using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public class EscapeSequenceHandler : ITokenHandler
{
    public IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens)
    {
        IToken? previousToken = null;
        var handledTokens = new List<IToken>();
        for (var i = 0; i < tokens.Count; i++)
        {
            var currentToken = tokens[i];
            switch (previousToken)
            {
                case { Type: TokenType.Escape } when
                    currentToken.Type is TokenType.Tag or TokenType.Escape:
                    handledTokens.Add(MarkdownTokenCreator.CreateTextToken(currentToken.Content));
                    previousToken = null;
                    break;
                case { Type: TokenType.Escape } when
                    currentToken.Type is not TokenType.Tag && currentToken.Type is not TokenType.Escape:
                    handledTokens.Add(previousToken);
                    handledTokens.Add(currentToken);
                    previousToken = currentToken;
                    break;
                default:
                {
                    if (currentToken.Type is TokenType.Escape)
                    {
                        if (i == tokens.Count - 1)
                            handledTokens.Add(currentToken);
                    }
                    else
                    {
                        handledTokens.Add(currentToken);
                    }

                    previousToken = currentToken;
                    break;
                }
            }
        }

        return handledTokens;
    }
}