using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown
{
    public class MdToTokenTranslator : IStringTranslator
    {
        public IEnumerable<Token> Translate(string markup)
        {
            var paragraphs = markup.Split('\n');
            var tokens = new List<Token>();
            var tokenBuilder = new TokenBuilder();
            var currentParagraph = 0;
            foreach (var paragraph in paragraphs)
            {
                var openedTokens = new Stack<Token>();
                for (var i = 0; i < paragraph.Length; i++)
                {
                    var currentPositionInText = i + PreviousParagraphLengthSum(paragraphs, currentParagraph);
                    switch (paragraph[i])
                    {
                        case '#':
                            if (i == 0)
                                AddHeaderToken(tokens, tokenBuilder, paragraph[i], currentPositionInText);
                            else
                                tokenBuilder.Append(paragraph[i]);
                            break;
                        case '_':
                            BuildPreviousToken(tokens, tokenBuilder, i);
                            SetTypeForBuilder(tokenBuilder, paragraph, 
                                currentPositionInText, i);
                            if (tokenBuilder.Type == TokenType.Bold)
                                i++;
                            SetOpeningForBuilder(tokenBuilder, openedTokens);
                            tokens.Add(tokenBuilder.Build());
                            tokenBuilder.Clear();
                            break;
                        case '\\':
                            if (i + 1 < paragraph.Length && paragraph[i + 1].IsTagSymbol())
                            {
                                i++;
                                tokenBuilder.Append(paragraph[i]);
                            }
                            else
                                tokenBuilder.Append(paragraph[i]);
                            break;
                        default:
                            if (i != 0 && paragraph[i - 1].IsTagSymbol() || i == 0)
                                tokenBuilder.SetPosition(currentPositionInText);
                            tokenBuilder.Append(paragraph[i]);
                            break;
                    }
                }
                currentParagraph++;
                BuildPreviousToken(tokens, tokenBuilder, currentParagraph);
                CloseHeadingToken(tokens, tokenBuilder, paragraph);
                tokens.Add(currentParagraph < paragraphs.Length
                    ? tokenBuilder.Append('\n').SetPosition(paragraph.Length).Build()
                    : tokenBuilder.Build());
                tokenBuilder.Clear();
            }

            tokens.RemoveAll(token => token.Length == 0 && token.Type != TokenType.Heading);
            return tokens.OrderBy(token => token.Position);
        }

        private void CloseHeadingToken(List<Token> tokens, TokenBuilder tokenBuilder, string paragraph)
        {
            var first = paragraph.FirstOrDefault();
            if (first == '#')
            {
                tokens.Add(tokenBuilder.Clear()
                    .SetPosition(paragraph.Length - 1)
                    .SetType(TokenType.Heading)
                    .SetOpening(false)
                    .Build());
                tokenBuilder.Clear();
            }
        }

        private void SetOpeningForBuilder(TokenBuilder tokenBuilder, Stack<Token> openedTokens)
        {
            if (openedTokens.TryPop(out var openedToken))
                if (openedToken.Type == tokenBuilder.Type)
                    tokenBuilder.SetOpening(false);
                else
                    SetOpeningWhenTokensIntersect(tokenBuilder, openedTokens, openedToken);
            else
            {
                tokenBuilder.SetOpening(true);
                openedTokens.Push(tokenBuilder.Build());
            }
        }

        private void SetOpeningWhenTokensIntersect(TokenBuilder tokenBuilder, 
            Stack<Token> openedTokens,
            Token openedToken)
        {
            if (openedTokens.Count == 0)
            {
                tokenBuilder.SetOpening(true);
                openedTokens.Push(openedToken);
                openedTokens.Push(tokenBuilder.Build());
            }
            else
            {
                tokenBuilder.SetOpening(false);
                openedTokens.Pop();
                openedTokens.Push(openedToken);
            }
        }

        private void SetTypeForBuilder(TokenBuilder tokenBuilder, 
            string paragraph,
            int currentPositionInText, 
            int i)
        {
            if (i + 1 < paragraph.Length && paragraph[i + 1] == '_')
            {
                tokenBuilder.SetPosition(currentPositionInText)
                    .SetType(TokenType.Bold)
                    .Append(new string(paragraph[i], 2));
            }
            else
                tokenBuilder.SetPosition(currentPositionInText)
                    .SetType(TokenType.Italics)
                    .Append(paragraph[i]);
        }

        private void BuildPreviousToken(List<Token> tokens, TokenBuilder tokenBuilder, int currentIndex)
        {
            if (tokenBuilder.Type == TokenType.Content)
            {
                if (currentIndex != 0)
                    tokens.Add(tokenBuilder.Build());
                tokenBuilder.Clear();
            }
        }

        private void AddHeaderToken(List<Token> tokens, 
            TokenBuilder tokenBuilder,
            char symbol,
            int currentPositionInText)
        {
            tokens.Add(tokenBuilder.SetPosition(currentPositionInText)
                .SetType(TokenType.Heading)
                .Append(symbol)
                .SetOpening(true)
                .Build());
            tokenBuilder.Clear();
        }

        private int PreviousParagraphLengthSum(string[] paragraphs, int currentParagraph)
        {
            return paragraphs.Take(currentParagraph).Sum(p => p.Length);
        }
    }
}
