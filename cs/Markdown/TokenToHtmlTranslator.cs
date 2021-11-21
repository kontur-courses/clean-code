using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;

namespace Markdown
{
    public class TokenToHtmlTranslator : ITokenTranslator
    {
        private static readonly Dictionary<TokenType, string> tokenToHtml = new Dictionary<TokenType, string>()
        {
            {TokenType.Heading, "h1"},
            {TokenType.Bold, "strong"},
            {TokenType.Italics, "em"}
        };

        public string Translate(IEnumerable<Token> tokens)
        {
            var htmlMarkup = new StringBuilder();
            var unpairedTokens = new HashSet<Token>();
            var pairedTokens = PairedToken.GetPairedTokens(tokens, unpairedTokens);
            var forbiddenTokens = GetForbiddenTokens(tokens, pairedTokens, unpairedTokens);
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Content:
                        htmlMarkup.Append(token.Value);
                        break;
                    case TokenType.Heading:
                        htmlMarkup.Append(token.IsOpening
                            ? $"<{tokenToHtml[token.Type]}>"
                            : $"</{tokenToHtml[token.Type]}>");
                        break;
                    case TokenType.Bold:
                    case TokenType.Italics:
                        if (forbiddenTokens.Contains(token))
                            htmlMarkup.Append(token.Value);
                        else
                            htmlMarkup.Append(token.IsOpening
                                ? $"<{tokenToHtml[token.Type]}>"
                                : $"</{tokenToHtml[token.Type]}>");
                        break;
                }
            }
            return htmlMarkup.ToString();
        }

        private HashSet<Token> GetForbiddenTokens(IEnumerable<Token> tokens,
            IEnumerable<PairedToken> pairedTokens, HashSet<Token> unpairedTokens)
        {
            var forbiddenTokens = GetForbiddenTokens(pairedTokens,
                (i, b) => i.From.Position < b.From.Position
                          && i.To.Position > b.To.Position).ToHashSet();

            forbiddenTokens.UnionWith(GetForbiddenTokens(pairedTokens,
                (i, b) => i.From.Position > b.From.Position
                          && i.To.Position > b.To.Position));

            forbiddenTokens.UnionWith(GetForbiddenTokens(tokens));
            
            forbiddenTokens.UnionWith(unpairedTokens);
            forbiddenTokens.UnionWith(GetForbiddenTokens(tokens, pairedTokens,
                (x, y, z) => (x || z) && y,
                c => c != ' ',
                x => !x));

            forbiddenTokens.UnionWith(GetForbiddenTokens(tokens, pairedTokens,
                (x, y, z) => x && y && z,
                c => c != ' ',
                x => !x));

            forbiddenTokens.UnionWith(GetForbiddenTokens(tokens, pairedTokens,
                (x, y, z) => (x || z) && y,
                char.IsDigit));
            return forbiddenTokens;
        }

        private IEnumerable<Token> GetForbiddenTokens(IEnumerable<Token> tokens,
            IEnumerable<PairedToken> pairedTokens,
            Func<bool, bool, bool, bool> tokensCondition,
            Func<char, bool> predicate) =>
            GetForbiddenTokens(tokens, pairedTokens, tokensCondition, predicate, x => x);

        private IEnumerable<Token> GetForbiddenTokens(IEnumerable<Token> tokens, 
            IEnumerable<PairedToken> pairedTokens,
            Func<bool, bool, bool, bool> tokensCondition,
            Func<char, bool> predicate,
            Func<bool, bool> switchResult)
        {
            var previousContent = tokens.First();

            foreach (var pair in pairedTokens)
            {
                var isLeftTokenEndsOnSpace = false;
                var isMiddleTokenMatched = false;
                var isRightTokenBeginsOnSpace = false;
                var isLastToken = false;
                foreach (var token in tokens)
                {
                    if (token == pair.From)
                        isLeftTokenEndsOnSpace = previousContent.Value.LastOrDefault() != ' ';
                    if (token == pair.To)
                    {
                        isMiddleTokenMatched = switchResult(previousContent.Value.All(predicate));
                        isLastToken = true;
                    }

                    if (isLastToken && token != pair.To)
                        isRightTokenBeginsOnSpace = token.Value.FirstOrDefault() != ' ';
                    
                    if (tokensCondition(isLeftTokenEndsOnSpace, isMiddleTokenMatched, isRightTokenBeginsOnSpace))
                    {
                        yield return pair.From;
                        yield return pair.To;
                        break;
                    }
                    if (isLastToken && token != pair.To)
                        break;
                    previousContent = token;
                }
            }
        }

        private IEnumerable<Token> GetForbiddenTokens(IEnumerable<PairedToken> tokens,
            Func<PairedToken, PairedToken, bool> condition)
        {
            var boldTokens = tokens.Where(t => t.Type == TokenType.Bold);
            var italicTokens = tokens.Where(t => t.Type == TokenType.Italics);
            foreach (var bold in boldTokens)
            foreach (var italic in italicTokens)
                if (condition(italic, bold))
                {
                    yield return bold.From;
                    yield return bold.To;
                    yield return italic.From;
                    yield return italic.To;
                }
        }

        private IEnumerable<Token> GetForbiddenTokens(IEnumerable<Token> tokens)
        {
            Token previousToken = null;
            foreach (var token in tokens)
            {
                if (previousToken == null)
                {
                    previousToken = token;
                }
                else if (previousToken.Type == token.Type)
                {
                    yield return previousToken;
                    yield return token;
                }
                else
                    previousToken = token;
            }
        }
    }
}
