namespace Markdown;

public class MarkdownParser : IParser
{
    private static readonly Dictionary<TagType, MarkdownTag> markdownTagsByType = new()
    {
        { TagType.Header, new HeaderMarkdownTag() },
        { TagType.Italic, new ItalicMarkdownTag() },
        { TagType.Bold, new BoldMarkdownTag() },
        { TagType.Escaping, new EscapingMarkdownTag() },
        { TagType.EndOfLine, new EndOfLineMarkdownTag() },
        { TagType.None, new NoneMarkdownTag() },
        { TagType.Link, new LinkMarkdownTag() }
    };

    private readonly List<MarkdownTag> possibleTags = [];

    public IEnumerable<Token> Parse(string text)
    {
        var result = new List<Token>();
        var tokensWithOpenTag = new Stack<OpenToken>();

        OpenToken? openTokenWithEmptyTag = null;
        int currentTagLength;

        for (var i = 0; i < text.Length; i += currentTagLength)
        {
            var currentTag = GetTag(text, i, tokensWithOpenTag);

            switch (currentTag.TagType)
            {
                case TagType.Link when LinkMarkdownTag.TryProcessTag(text, i, out var resultLinkToken):
                    currentTagLength = resultLinkToken.lengthTag;
                    result.Add(resultLinkToken.token);
                    continue;
                case TagType.Link or TagType.None:
                    currentTagLength = 1;
                    openTokenWithEmptyTag ??= new OpenToken(markdownTagsByType[TagType.None], i); 
                    continue;
                case TagType.EndOfLine when tokensWithOpenTag.Any(a => a.OpenTag.TagType == TagType.Header):
                    openTokenWithEmptyTag = new OpenToken(markdownTagsByType[TagType.None], i);
                    break;
            }

            if (openTokenWithEmptyTag is not null && currentTag.TagType != TagType.EndOfLine)
            {
                var token = CreateToken(text, i, openTokenWithEmptyTag);
                AddToken(token, tokensWithOpenTag, result);
                openTokenWithEmptyTag = null;
            }

            if (currentTag.IsPairedTag)
                ProcessPairedTag(text, currentTag, tokensWithOpenTag, i, result);
            else
                ProcessUnpairedTag(text, currentTag, tokensWithOpenTag, i, result);

            currentTagLength = currentTag.TotalTagLength;
        }

        result = AddUnfinishedTags(text, openTokenWithEmptyTag, tokensWithOpenTag, result);

        return result;
    }

    #region Processing Tags

    private static void ProcessUnpairedTag(string text, MarkdownTag currentTag, Stack<OpenToken> tokensWithOpenTag,
        int position, List<Token> result)
    {
        if (currentTag.IsPairedTag)
            throw new ArgumentException("Unpaired tag was expected, but paired tag was received");

        var currentTagLength = currentTag.TagText.Length;

        switch (currentTag.TagType)
        {
            case TagType.Header:
            {
                var openToken = new OpenToken(currentTag, position + currentTagLength);
                tokensWithOpenTag.Push(openToken);
                break;
            }
            case TagType.EndOfLine:
                HeaderMarkdownTag.ProcessEndTag(text, tokensWithOpenTag, position, result);
                break;
            case TagType.Escaping:
            {
                var startPosition = position + currentTagLength;
                var openToken = new OpenToken(currentTag, startPosition);
                var token = CreateToken(text, startPosition + 1, openToken);
                AddToken(token, tokensWithOpenTag, result);
                break;
            }
        }
    }

    private static void ProcessPairedTag(string text, MarkdownTag currentTag, Stack<OpenToken> tokensWithOpenTag,
        int position, List<Token> result)
    {
        if (!currentTag.IsPairedTag)
            throw new ArgumentException("Paired tag was expected, but unpaired tag was received");

        if (tokensWithOpenTag.IsPeekEqual(currentTag.TagType))
        {
            var openToken = tokensWithOpenTag.Pop();
            AddToken(CreateToken(text, position, openToken), tokensWithOpenTag, result);
        }
        else if (tokensWithOpenTag.All(t => t.OpenTag.TagType != currentTag.TagType))
        {
            tokensWithOpenTag.Push(new OpenToken(currentTag, position + currentTag.TagText.Length));
        }
    }

    #endregion

    #region BorderlineCases

    private static OpenToken ConvertTagBoldInsideTagItalic(OpenToken openToken)
    {
        if (openToken.OpenTag.TagType == TagType.Italic && openToken.NestedTokens.Count > 0)
            for (var i = 0; i < openToken.NestedTokens.Count; i++)
                openToken.NestedTokens[i] = ConvertTagBoldToTagNone(openToken.NestedTokens[i]);
        return openToken;
    }

    private static Token ConvertTagBoldToTagNone(Token token)
    {
        if (token.TagType == TagType.Bold)
        {
            var tagText = markdownTagsByType[TagType.Bold].TagText;
            var newText = $"{tagText}{token.Content}{tagText}";
            var children = token.Children?.Select(ConvertTagBoldToTagNone).ToList();
            return new Token(TagType.None, newText, children);
        }

        if (token.Children != null)
        {
            var children = token.Children.Select(ConvertTagBoldToTagNone).ToList();
            return new Token(token.TagType, token.Content, children);
        }

        return token;
    }

    #endregion

    private static List<Token> AddUnfinishedTags(string text, OpenToken? openTokenWithEmptyTag,
        Stack<OpenToken> tokensWithOpenTag, List<Token> listTokens)
    {
        if (openTokenWithEmptyTag is not null && tokensWithOpenTag.Count == 0)
        {
            var token = CreateToken(text, text.Length, openTokenWithEmptyTag);
            AddToken(token, tokensWithOpenTag, listTokens);
        }

        while (tokensWithOpenTag.Count > 0)
        {
            var openToken = tokensWithOpenTag.Pop();
            if (openToken.OpenTag.TagType != TagType.Header)
            {
                var token = CreateTokenForPairedTagWithoutPair(text, text.Length, openToken);
                AddToken(token, tokensWithOpenTag, listTokens);
            }
            else
            {
                var token = CreateToken(text, text.Length, openToken);
                AddToken(token, tokensWithOpenTag, listTokens);
            }
        }

        return listTokens;
    }

    public static void AddToken(Token token, Stack<OpenToken> tokensWithOpenTag, List<Token> result)
    {
        if (tokensWithOpenTag.Count == 0)
            result.Add(token);
        else
            tokensWithOpenTag.Peek().NestedTokens.Add(token);
    }

    public static Token CreateToken(string text, int endPosition, OpenToken openToken)
    {
        var length = endPosition - openToken.TextStartPosition;

        if (openToken.OpenTag.TagType == TagType.Italic) openToken = ConvertTagBoldInsideTagItalic(openToken);

        if (!markdownTagsByType[openToken.OpenTag.TagType].IsTagCorrect(text, endPosition, openToken))
        {
            var tagText = openToken.OpenTag.TagText;
            var content = $"{tagText}{text.Substring(openToken.TextStartPosition, length)}{tagText}";
            return new Token(TagType.None, content);
        }

        if (openToken.NestedTokens is [{ TagType: TagType.None }] || openToken.NestedTokens.Count == 0)
            return new Token(openToken.OpenTag.TagType, text.Substring(openToken.TextStartPosition, length));

        return new Token(openToken.OpenTag.TagType, text.Substring(openToken.TextStartPosition, length),
            openToken.NestedTokens);
    }

    public static TokenTagLink CreateLinkToken(string content, string linkText, string? tooltipText)
    {
        return new TokenTagLink(content, linkText, tooltipText);
    }

    private static Token CreateTokenForPairedTagWithoutPair(string text, int endPosition, OpenToken openToken)
    {
        var startPosition = openToken.TextStartPosition - openToken.OpenTag.TagText.Length;
        return new Token(TagType.None, text.Substring(startPosition, endPosition - startPosition));
    }

    private MarkdownTag GetTag(string text, int position, Stack<OpenToken> tokensWithOpenTag)
    {
        possibleTags.Clear();

        foreach (var tag in markdownTagsByType.Values)
            if (tag.IsStartOfTag(text, position, tokensWithOpenTag.Any(t => t.OpenTag.TagType == tag.TagType)))
                possibleTags.Add(tag);

        return possibleTags.OrderByDescending(x => x.TagText.Length).First();
    }
}