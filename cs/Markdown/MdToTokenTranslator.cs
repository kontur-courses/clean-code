using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Markdown.Extensions;
using Markdown.Tokens;
using Microsoft.VisualBasic;

namespace Markdown
{
    public class MdToTokenTranslator : IStringTranslator
    {
        public IEnumerable<IToken> Translate(string markdown)
        {
            var tokens = new List<IToken>();
            var tokenBuilder = new TokenBuilder();
            //var isHeadingTokenOpened = false;
            var openedTokens = new HashSet<TokenType>();
            for (var i = 0; i < markdown.Length; i++)
            {
                switch (markdown[i])
                {
                    case HeadingToken.FirstSymbol:
                        var isNewParagraph = i == 0 || HeadingToken.NewParagraphSymbols.Any(s => markdown[i - 1] == s);
                        var isHeading = i + 1 < markdown.Length && markdown[i + 1] == HeadingToken.SecondSymbol;
                        var isHeadingOpened = isHeading && isNewParagraph;
                        AnalyzeTagSymbol(openedTokens, tokens, tokenBuilder, 
                            markdown, i,  isHeadingOpened,
                            condition => condition ? TokenType.Heading : TokenType.Content);
                        if (isHeadingOpened)
                            i++;
                        break;
                    case BoldToken.FirstSymbol:
                        var isBold = i + 1 < markdown.Length && markdown[i + 1] == BoldToken.SecondSymbol;
                        AnalyzeTagSymbol(openedTokens, tokens, tokenBuilder,
                            markdown, i, isBold,
                            condition => condition ? TokenType.Bold : TokenType.Italics);
                        if (isBold)
                            i++;
                        break;
                    case EscapeToken.FirstSymbol:
                        if (i + 1 < markdown.Length && markdown[i + 1].IsTagSymbol())
                        {
                            i++;
                            tokenBuilder.Append(markdown[i]);
                        }
                        else 
                            tokenBuilder.Append(markdown[i]);
                        break;
                    case '\r':
                    case '\n':
                        isHeadingOpened = openedTokens.Contains(TokenType.Heading);
                        AnalyzeTagSymbol(openedTokens, tokens, tokenBuilder, 
                            markdown, i, isHeadingOpened,
                            condition => condition ? TokenType.Heading : TokenType.Content);
                        if (isHeadingOpened)
                            tokenBuilder.Append(markdown[i]);
                        openedTokens.Clear();
                        break;
                    default:
                        tokenBuilder.Append(markdown[i]);
                        break;
                }
            }
            tokens.Add(tokenBuilder.Build());
            if (openedTokens.Contains(TokenType.Heading))
                tokens.Add(new HeadingToken(markdown.Length - 1, false));

            var unpairedTokens = new HashSet<IToken>();
            var pairedTokens = PairedToken.GetPairedTokens(tokens, unpairedTokens);
            tokens.RemoveAll(token => token.Length == 0);
            var forbiddenTokens = tokens.GetForbiddenTokens(pairedTokens, unpairedTokens);
            var resultTokens = SetSkipForTokens(tokens, forbiddenTokens).ToList();
            return resultTokens.OrderBy(token => token.Position);
        }

        private void AnalyzeTagSymbol(HashSet<TokenType> openedTokens,
            List<IToken> tokens,
            TokenBuilder tokenBuilder,
            string markdown,
            int i, 
            bool conditionForType,
            Func<bool, TokenType> typeCreator)
        {
            var type = typeCreator(conditionForType);
            var isOpening = IsOpeningToken(type, openedTokens);
            if (type != TokenType.Content)
            {
                tokens.Add(tokenBuilder.Build());
                tokenBuilder.Clear();
                tokenBuilder.SetType(type);
                tokenBuilder.SetOpening(isOpening);
                tokenBuilder.SetPosition(i);
                tokens.Add(tokenBuilder.Build());
                tokenBuilder.Clear();
                tokenBuilder.SetPosition(i + 1);
            }
            else 
                tokenBuilder.Append(markdown[i]);
        }

        private bool IsOpeningToken(TokenType type, HashSet<TokenType> openedTokens)
        {
            if (openedTokens.Contains(type))
            {
                openedTokens.Remove(type);
                return false;
            }
            openedTokens.Add(type);
            return true;
        }

        private IEnumerable<IToken> SetSkipForTokens(IEnumerable<IToken> tokens, HashSet<IToken> tokensForSkip)
        {
            var tokenBuilder = new TokenBuilder();
            foreach (var token in tokens)
            {
                if (tokensForSkip.Contains(token))
                    yield return tokenBuilder.SetSettingsByToken(token)
                        .SetSkip(true)
                        .Build();
                else
                    yield return token;
            }
        }
    }
}
