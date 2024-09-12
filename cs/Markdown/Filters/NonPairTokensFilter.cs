using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Filters;

public class NonPairTokensFilter : IFilter
{
    private readonly string text;

    public NonPairTokensFilter(string text)
    {
        this.text = text;
    }

    public int Order { get; } = 2;

    public IList<IToken> Filter(IList<IToken> tokens)
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
}
