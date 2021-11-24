using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown
{
    public class MdToTokenTranslator : IStringTranslator
    {
        public IEnumerable<IToken> Translate(string markdown)
        {
            var tokens = new List<IToken>();
            var tokenBuilder = new TokenBuilder();
            var isHeadingTokenOpened = false;
            var openedTokens = new Stack<IToken>();
            for (var i = 0; i < markdown.Length; i++)
            {
                switch (markdown[i])
                {
                    case '#':
                        var isNewParagraph = i == 0 || markdown[i - 1] == '\r' || markdown[i - 1] == '\n';
                        var isNextSymbolSpace = i + 1 < markdown.Length && markdown[i + 1] == ' ';
                        if (isNewParagraph && isNextSymbolSpace)
                        {
                            isHeadingTokenOpened = true;
                            tokens.Add(new HeadingToken(i, true));
                            i++;
                        }
                        else
                            tokenBuilder.Append(markdown[i]);
                        break;
                    case '_':
                        BuildPreviousToken(tokens, tokenBuilder, i);
                        tokens.Add(GetBoldOrItalicToken(openedTokens, markdown, i, out var isBold));
                        if (isBold)
                            i++;
                        break;
                    case '\\':
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
                        AnalyzeNewLineSymbol(tokens, openedTokens, tokenBuilder, markdown, i, isHeadingTokenOpened);
                        isHeadingTokenOpened = false;
                        break;
                    default:
                        if (i != 0 && markdown[i - 1].IsTagSymbol() || i == 0)
                            tokenBuilder.SetPosition(i);
                        tokenBuilder.Append(markdown[i]);
                        break;
                }
            }
            tokens.Add(tokenBuilder.Build());
            if (isHeadingTokenOpened)
                tokens.Add(new HeadingToken(markdown.Length - 1, false));

            var unpairedTokens = new HashSet<IToken>();
            var pairedTokens = PairedToken.GetPairedTokens(tokens, unpairedTokens);
            var forbiddenTokens = tokens.GetForbiddenTokens(pairedTokens, unpairedTokens);
            var resultTokens = SetSkipForTokens(tokens, forbiddenTokens).ToList();
            resultTokens.RemoveAll(token => token.Length == 0 && token.Type != TokenType.Heading);
            return resultTokens.OrderBy(token => token.Position);
        }

        private void AnalyzeNewLineSymbol(List<IToken> tokens,
            Stack<IToken> openedTokens,
            TokenBuilder tokenBuilder, 
            string markdown, 
            int i,
            bool isHeadingTokenOpened)
        {
            if (isHeadingTokenOpened)
            {
                tokens.Add(tokenBuilder.Build());
                var token = new HeadingToken(i - 1, false);
                tokens.Add(token);
                tokenBuilder.Clear();
                tokenBuilder.SetPosition(i).Append(markdown[i]);
            }
            else
            {
                openedTokens.Clear();
                tokenBuilder.Append(markdown[i]);
            }
        }

        private IToken GetBoldOrItalicToken(Stack<IToken> openedTokens, string markdown, int i, out bool isBold)
        {
            isBold = false;
            TokenType type;
            BoldToken boldToken = null;
            if (i + 1 < markdown.Length && markdown[i + 1] == '_')
            {
                boldToken = new BoldToken(i);
                type = boldToken.Type;
                isBold = true;
            }
            else
                type = TokenType.Italics;

            var isOpening = IsOpeningToken(type, openedTokens);

            var token = boldToken != null 
                ? (IToken) new BoldToken(i, isOpening, false) 
                : new ItalicToken(i, isOpening, false);
            openedTokens.Push(token);
            return token;
        }

        private bool IsOpeningToken(TokenType type, Stack<IToken> openedTokens)
        {
            //IToken openedToken = null;
            if (!openedTokens.TryPop(out var openedToken)) return true;
            if (openedToken.Type == type) return false;
            if (openedTokens.Count == 0)
            {
                openedTokens.Push(openedToken);
                return true;
            }
            openedTokens.Pop();
            openedTokens.Push(openedToken);
            return false;
        }

        //private void CloseHeadingToken(List<Token> tokens, TokenBuilder tokenBuilder, string paragraph)
        //{
        //    var first = paragraph.FirstOrDefault();
        //    if (first == '#')
        //    {
        //        tokens.Add(tokenBuilder.Clear()
        //            .SetPosition(paragraph.Length - 1)
        //            .SetType(TokenType.Heading)
        //            .SetOpening(false)
        //            .Build());
        //        tokenBuilder.Clear();
        //    }
        //}

        //private void SetOpeningForBuilder(TokenBuilder tokenBuilder, Stack<IToken> openedTokens)
        //{
        //    if (openedTokens.TryPop(out var openedToken))
        //        if (openedToken.Type == tokenBuilder.Type)
        //            tokenBuilder.SetOpening(false);
        //        else
        //            SetOpeningWhenTokensIntersect(tokenBuilder, openedTokens, openedToken);
        //    else
        //    {
        //        tokenBuilder.SetOpening(true);
        //        openedTokens.Push(tokenBuilder.Build());
        //    }
        //}

        //private void SetOpeningWhenTokensIntersect(TokenBuilder tokenBuilder, 
        //    Stack<IToken> openedTokens,
        //    IToken openedToken)
        //{
        //    if (openedTokens.Count == 0)
        //    {
        //        tokenBuilder.SetOpening(true);
        //        openedTokens.Push(openedToken);
        //        openedTokens.Push(tokenBuilder.Build());
        //    }
        //    else
        //    {
        //        tokenBuilder.SetOpening(false);
        //        openedTokens.Pop();
        //        openedTokens.Push(openedToken);
        //    }
        //}

        //private void SetTypeForBuilder(TokenBuilder tokenBuilder, 
        //    string paragraph, int i)
        //{
        //    if (i + 1 < paragraph.Length && paragraph[i + 1] == '_')
        //    {
        //        tokenBuilder.SetPosition(i)
        //            .SetType(TokenType.Bold)
        //            .Append(new string(paragraph[i], 2));
        //    }
        //    else
        //        tokenBuilder.SetPosition(i)
        //            .SetType(TokenType.Italics)
        //            .Append(paragraph[i]);
        //}

        private void BuildPreviousToken(List<IToken> tokens, TokenBuilder tokenBuilder, int currentIndex)
        {
            if (tokenBuilder.Type == TokenType.Content)
            {
                if (currentIndex != 0)
                    tokens.Add(tokenBuilder.Build());
                tokenBuilder.Clear();
            }
        }

        //private void AddHeaderToken(List<Token> tokens, 
        //    TokenBuilder tokenBuilder,
        //    char symbol,
        //    int currentPositionInText)
        //{
        //    tokens.Add(tokenBuilder.SetPosition(currentPositionInText)
        //        .SetType(TokenType.Heading)
        //        .Append(symbol)
        //        .SetOpening(true)
        //        .Build());
        //    tokenBuilder.Clear();
        //}

        //private int PreviousParagraphLengthSum(string[] paragraphs, int currentParagraph)
        //{
        //    return paragraphs.Take(currentParagraph).Sum(p => p.Length);
        //}

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
