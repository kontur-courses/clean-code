using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Filters;

public class TokenFilter : ITokenFilter
{
    public IList<IToken> ChangeTypeForEscapedTags(IEnumerable<IToken> tokens)
    {
        IToken? previousToken = null;
        var result = new List<IToken>();
        foreach (var token in tokens)
        {
            if (previousToken != null && previousToken.Type == TokenType.Escape)
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

    public IList<IToken> ChangeTypeForIncorrectTags(IList<IToken> tokens, string text)
    {
        var result = new List<IToken>();
        foreach (var token in tokens)
        {
            result.Add(token);
            if (token.Type == TokenType.Text) continue;
            if (!SupportedTags.IsValidTokenTag(token, text))
                token.Type = TokenType.Text;
        }
        return result;
    }

    public IList<IToken> ChangeTypeForNonPairTokens(IList<IToken> tokens, string text)
    {
        var resultTokens = new List<IToken>();
        var openTags = new Stack<IToken>();
        var incorrectTags = new List<IToken>();
        foreach (var token in tokens)
        {
            resultTokens.Add(token);
            if (token.Type == TokenType.Text)
                continue;
            if (SupportedTags.IsOpenTag(token, text))
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

        CloseSingleTagsAndAddIncorrectTokens(openTags, resultTokens, incorrectTags);
        ChangeTypesForIncorrectTokens(incorrectTags);
        return resultTokens;
    }

    public void SolveOpenAndCloseTags(IToken openToken, IToken closeToken,
        List<IToken> incorrectTags, Stack<IToken> openTags)
    {
        var openTagType = TokenUtilities.GetTokenTagType(openToken);
        var closeTagType = TokenUtilities.GetTokenTagType(closeToken);
        openTags.TryPeek(out var previousOpenToken);
        var previousOpenTagType = TokenUtilities.GetTokenTagType(previousOpenToken);
        if (openTagType == closeTagType) return;
        if ((openTagType == TagType.Header || openTagType == TagType.Link || openTagType == TagType.LinkDescription)
            && closeTagType != TagType.Header && closeTagType != TagType.Link && closeTagType != TagType.LinkDescription)
        {
            incorrectTags.Add(closeToken);
            openTags.Push(openToken);
        }
        else if (closeTagType == TagType.Header && previousOpenTagType == closeTagType
                || (closeTagType == TagType.Link || closeTagType == TagType.LinkDescription)
                && previousOpenTagType == closeTagType)
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

    public void AddIncorrectTagsIfHaveDigitsBetweenTokens(IToken firstToken, IToken secondToken,
        string text, List<IToken> tokens)
    {
        SupportedTags.Tags.TryGetValue(firstToken.Content, out var firstTag);
        SupportedTags.Tags.TryGetValue(secondToken.Content, out var secondTag);
        if (TokenUtilities.IsHaveDigitsBetweenTokens(firstToken, secondToken, text)
            && firstTag?.TagType == secondTag?.TagType && firstTag?.TagType == TagType.Italic)
        {
            tokens.Add(firstToken);
            tokens.Add(secondToken);
        }
    }

    public void CloseSingleTagsAndAddIncorrectTokens(Stack<IToken> openTags,
        IList<IToken> resultTokens, IList<IToken> incorrectTokens)
    {
        var lastToken = resultTokens.Last();
        foreach (var tag in openTags)
        {
            if (SupportedTags.Tags[tag.Content].TagType == TagType.Header)
            {
                var tokenContent = SupportedTags.Tags.Values.First(t => t.TagType == TagType.Header && !t.IsOpen).Value;
                var newToken = new Token(tokenContent, TokenType.Tag, lastToken.StartPosition + lastToken.Content.Length);
                resultTokens.Add(newToken);
                lastToken = newToken;
            }
            else
            {
                incorrectTokens.Add(tag);
            }
        }
    }

    public void ChangeTypesForIncorrectTokens(IList<IToken> tokens)
    {
        foreach (var token in tokens)
        {
            token.Type = TokenType.Text;
        }
    }

    public IList<IToken> CombineStrongTags(IList<IToken> tokens, string text)
    {
        (IToken? Token, Tag? Tag) previousTokenAndTag = (null, null);
        IToken? prePreviousTag = null;
        var resultTokens = new List<IToken>();
        foreach (var token in tokens)
        {
            var currentTokenAndTag = TokenUtilities.GetTokenAndTag(token);
            if (currentTokenAndTag.Token.Type == TokenType.Tag
                && previousTokenAndTag.Token?.Type == TokenType.Tag
                && currentTokenAndTag.Tag?.TagType == TagType.Italic
                && previousTokenAndTag.Tag?.TagType == TagType.Italic
                && currentTokenAndTag.Tag.TagType != TagType.Strong
                && !TokenUtilities.IsPreviousTokenCloseAndPrePreviousOpen(previousTokenAndTag.Token, prePreviousTag, text))
            {

                currentTokenAndTag.Token.Content += previousTokenAndTag.Token.Content;
                currentTokenAndTag.Token.StartPosition = previousTokenAndTag.Token.StartPosition;
                currentTokenAndTag.Tag = SupportedTags.Tags[currentTokenAndTag.Token.Content];
                resultTokens.Remove(previousTokenAndTag.Token);
            }
            resultTokens.Add(currentTokenAndTag.Token);
            if (previousTokenAndTag.Token?.Type != TokenType.Text)
                prePreviousTag = previousTokenAndTag.Token;
            previousTokenAndTag = currentTokenAndTag;
        }
        return resultTokens;
    }

    public IList<IToken> ChangeTypeForNestedTokens(IList<IToken> tokens,
        TagType outer, HashSet<TagType> nested, IList<IToken> tagTokens)
    {
        var result = new List<IToken>(tokens);
        for (var i = 0; i < tagTokens.Count; i++)
        {
            var token = tagTokens[i];
            var tokenType = TokenUtilities.GetTokenTagType(token);
            if (tokenType == outer)
            {
                var nestedTokens = GetNestedTokens(tagTokens, ref i, tokenType);
                ChangeTypesToTextForTagType(nestedTokens, nested);
            }
        }
        return result;
    }

    public IEnumerable<IToken> GetNestedTokens(IList<IToken> tagTokens, ref int index, TagType? tokenType)
    {
        var result = new List<IToken>();
        for (var i = index + 1; i < tagTokens.Count; i++)
        {
            var token = tagTokens[i];
            var type = TokenUtilities.GetTokenTagType(token);
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

    public void ChangeTypesToTextForTagType(IEnumerable<IToken> tokens, HashSet<TagType> types)
    {
        foreach (var token in tokens)
        {
            var tokenTagType = TokenUtilities.GetTokenTagType(token);
            if (tokenTagType != null && types.Contains((TagType)tokenTagType))
                token.Type = TokenType.Text;
        }
    }
}
