using Markdown.Tags;
using Markdown.Tokens;
using System.Text;

namespace Markdown.Parsers
{
    public class MarkdownParser : IMarkingParser
    {
        private int currentPosition;
        private readonly HashSet<string> tagsSymbols;

        public MarkdownParser(HashSet<string> tagsSymbols)
        {
            this.tagsSymbols = tagsSymbols;
        }

        public IEnumerable<IToken> ParseText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text should not be empty string or null");
            }
            var tokens = SplitOnTokens(text);
            tokens = ChangeTypeForEscapedTags(tokens);
            tokens = ChangeTypeForIncorrectTags(tokens, text);
            tokens = ChangeTypeForNonPairTokens(tokens, text);
            tokens = CombineStrongTags(tokens, text);
            tokens = ChangeTypeForNonPairTokens(tokens, text);
            tokens = ChangeTypeForNestedTokens(tokens, TagType.Italic, TagType.Strong);
            return tokens;
        }

        private IEnumerable<IToken> SplitOnTokens(string text)
        {
            for (; currentPosition < text.Length; currentPosition++)
            {
                var tokenType = GetTokenType(text[currentPosition]);
                yield return CreateTokenAndMoveToNext(text, tokenType, currentPosition);
            }
        }

        private TokenType GetTokenType(char startSymbol)
        {
            if (startSymbol.ToString() == TagInfo.Escape) return TokenType.Escape;
            var markdownTag = tagsSymbols.FirstOrDefault(k => k.StartsWith(startSymbol));
            return markdownTag == null ? TokenType.Text : TokenType.Tag;
        }

        private IToken CreateTokenAndMoveToNext(string text, TokenType type, int startPosition)
        {
            var tokenContent = new StringBuilder();

            while (currentPosition < text.Length && !IsCanStopCreateToken(type, text[currentPosition], tokenContent.ToString()))
            {
                tokenContent.Append(text[currentPosition]);
                currentPosition++;
            }

            currentPosition--;

            return new Token(tokenContent.ToString(), type, startPosition);
        }

        private IEnumerable<IToken> ChangeTypeForEscapedTags(IEnumerable<IToken> tokens)
        {
            IToken? previousToken = null;
            var result = new List<IToken>();
            foreach (var token in tokens)
            {
                if (previousToken != null &&  previousToken.Type == TokenType.Escape)
                {
                    if (token.Type != TokenType.Text)
                    {
                        token.Type = TokenType.Text;
                        previousToken = token;
                        result.Add(token);
                    }
                    else
                    {
                        previousToken.Type = TokenType.Text;
                        result.Add(previousToken);
                        result.Add(token);
                        previousToken = token;
                    }
                }
                else if (token.Type == TokenType.Escape)
                {
                    previousToken = token;
                }
                else
                {
                    result.Add(token);
                    previousToken = token;
                }
            }
            if (previousToken != null && previousToken.Type == TokenType.Escape)
            {
                previousToken.Type = TokenType.Text;
                result.Add(previousToken);
            }
            return result;
        }

        private IEnumerable<IToken> ChangeTypeForIncorrectTags(IEnumerable<IToken> tokens, string text)
        {
            var result = new List<IToken>();
            foreach (var token in tokens)
            {
                result.Add(token);
                if (token.Type == TokenType.Text) continue;
                if (!TagInfo.IsValidTokenTag(token, text))
                    token.Type = TokenType.Text;
            }
            return result;
        }

        private IEnumerable<IToken> ChangeTypeForNonPairTokens(IEnumerable<IToken> tokens, string text)
        {
            var resultTokens = new List<IToken>();
            var openTags = new Stack<IToken>();
            var incorrectTags = new List<IToken>();
            foreach (var token in tokens)
            {
                resultTokens.Add(token);
                if (token.Type == TokenType.Text) continue;
                if (TagInfo.IsOpenTag(token, text))
                {
                    openTags.Push(token);
                }
                else
                {
                    if (openTags.TryPop(out var lastOpenToken))
                    {
                        SolveOpenAndCloseTags(lastOpenToken, token, incorrectTags, openTags);
                        AddIncorrectTagsIfHaveDigitsBetweenTokens(lastOpenToken, token,
                            text, incorrectTags);
                    }
                    else
                    {
                        incorrectTags.Add(token);
                    }
                }
            }
            incorrectTags.AddRange(openTags);
            ChangeTypesForIncorrectTokens(incorrectTags);
            return resultTokens;
        }

        private void SolveOpenAndCloseTags(IToken openToken, IToken closeToken, 
            List<IToken> incorrectTags, Stack<IToken> openTags)
        {
            var openTagType = GetTokenTagType(openToken);
            var closeTagType = GetTokenTagType(closeToken);
            if (openTagType == closeTagType) return;
            if (openTagType == TagType.Header)
            {
                incorrectTags.Add(closeToken);
                openTags.Push(openToken);
            }
            else if (closeTagType == TagType.Header && openTags.TryPeek(out var previousOpenToken)
                    && GetTokenTagType(previousOpenToken) == TagType.Header)
            {
                incorrectTags.Add(openToken);
                openTags.Pop();
            }
            else
            {
                incorrectTags.Add(openToken); 
                incorrectTags.Add(closeToken);
            }

        }

        private void AddIncorrectTagsIfHaveDigitsBetweenTokens(IToken firstToken, IToken secondToken, 
            string text, List<IToken> tokens)
        {
            var firstTag = TagInfo.GetTagByMarkdownValue(firstToken.Content);
            var secondTag = TagInfo.GetTagByMarkdownValue(secondToken.Content);
            if (IsHaveDigits(firstToken.StartPosition, secondToken.StartPosition, text)
                && firstTag?.TagType == secondTag?.TagType && firstTag?.TagType == TagType.Italic)
            {
                tokens.Add(firstToken);
                tokens.Add(secondToken);
            }
        }

        private bool IsHaveDigits(int startPosition, int endPosition, string text)
        {
            return text.Substring(startPosition, endPosition - startPosition).Any(c => char.IsDigit(c));
        }

        private void ChangeTypesForIncorrectTokens(IEnumerable<IToken> tokens)
        {
            foreach (var token in tokens) 
            {
                token.Type = TokenType.Text;
            }
        }

        private IEnumerable<IToken> CombineStrongTags(IEnumerable<IToken> tokens, string text)
        {
            (IToken? Token, Tag? Tag) previousTokenAndTag = (null, null);
            IToken? prePreviousTag = null; 
            var resultTokens = new List<IToken>();
            foreach (var token in tokens)
            {
                var currentTokenAndTag = GetTokenAndTag(token);
                if (currentTokenAndTag.Token.Type == TokenType.Tag 
                    && previousTokenAndTag.Token?.Type == TokenType.Tag 
                    && currentTokenAndTag.Tag?.TagType == TagType.Italic 
                    && previousTokenAndTag.Tag?.TagType == TagType.Italic
                    && previousTokenAndTag.Token.Content != TagInfo.MarkdownStrongTag
                    && !IsPreviousTokenCloseAndPrePreviousOpen(previousTokenAndTag.Token, prePreviousTag, text))
                {

                    currentTokenAndTag.Token.Content = TagInfo.MarkdownStrongTag;
                    currentTokenAndTag.Token.StartPosition = previousTokenAndTag.Token.StartPosition;
                    resultTokens.Remove(previousTokenAndTag.Token);
                }
                resultTokens.Add(currentTokenAndTag.Token);
                if (previousTokenAndTag.Token?.Type != TokenType.Text)
                    prePreviousTag = previousTokenAndTag.Token;
                previousTokenAndTag = currentTokenAndTag;
            }
            return resultTokens;
        }

        private (IToken Token, Tag? Tag) GetTokenAndTag(IToken token)
        {
            (IToken Token, Tag? Tag) tokenAndTag = (token, null);
            if (token.Type == TokenType.Tag)
            {
                tokenAndTag.Tag = TagInfo.GetTagByMarkdownValue(token.Content);
            }
            else
            {
                tokenAndTag.Tag = null;
            }
            return tokenAndTag;
        }

        private bool IsPreviousTokenCloseAndPrePreviousOpen(IToken? previous, IToken? prePrevious, string text)
        {
            return !TagInfo.IsOpenTag(previous, text) && TagInfo.IsOpenTag(prePrevious, text)
                    && TagInfo.GetTagByMarkdownValue(previous?.Content)?.TagType 
                    == TagInfo.GetTagByMarkdownValue(prePrevious?.Content)?.TagType;
        }

        private IEnumerable<IToken> ChangeTypeForNestedTokens(IEnumerable<IToken> tokens,
            TagType outer, TagType nested)
        {
            var result = new List<IToken>(tokens);
            var tagTokens = GetAllTagTokens(tokens).ToList();
            for (var i = 0; i < tagTokens.Count; i++)
            {
                var token = tagTokens[i];
                var tokenType = GetTokenTagType(token);
                if (tokenType == outer)
                {
                    var nestedTokens = GetNestedTokens(tagTokens, ref i, tokenType);
                    ChangeTypesToTextForTagType(nestedTokens, nested);
                }
            }
            return result;
        }

        private IEnumerable<IToken> GetAllTagTokens(IEnumerable<IToken> tokens)
        {
            return tokens.Where(token => token.Type == TokenType.Tag);
        }

        private IEnumerable<IToken> GetNestedTokens(IList<IToken> tagTokens, ref int index, TagType? tokenType)
        {
            var result = new List<IToken>();
            for (var i = index + 1; i < tagTokens.Count; i++)
            {
                var token = tagTokens[i];
                var type = GetTokenTagType(token);
                if (type == tokenType)
                {
                    index = i + 1;
                    break;
                }
                else
                {
                    result.Add(token);
                }
            }

            return result;
        }

        private void ChangeTypesToTextForTagType(IEnumerable<IToken> tokens, TagType? type)
        {
            foreach (var token in tokens) 
            {
                if (GetTokenTagType(token) == type)
                    token.Type = TokenType.Text;
            }
        }

        private TagType? GetTokenTagType(IToken? token)
        {
            return TagInfo.GetTagByMarkdownValue(token?.Content)?.TagType;
        }

        private bool IsCanStopCreateToken(TokenType tokenType, char currentSymbol, string previousSymbols)
        {
            return tokenType switch
            {
                TokenType.Text => IsTextTokenEnd(currentSymbol),
                TokenType.Tag => IsTagTokenEnd(previousSymbols),
                TokenType.Escape => IsEscapeTokenEnd(previousSymbols),
                _ => throw new ArgumentException($"tag with type {tokenType} not supported")
            };
        }

        private bool IsTextTokenEnd(char currentSymbol)
        {
            return IsEscapeTokenEnd(currentSymbol.ToString()) || IsTagStartingWithSymbol(currentSymbol);
        }

        private bool IsTagTokenEnd(string previousSymbols)
        {
            return tagsSymbols.Any(k => k == previousSymbols);
        }

        private bool IsEscapeTokenEnd(string previousSymbols)
        {
            return previousSymbols == TagInfo.Escape;
        }

        private bool IsTagStartingWithSymbol(char symbol)
        {
            return tagsSymbols.Any(k => k.StartsWith(symbol));
        }
    }
}
