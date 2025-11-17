using System.Text;

namespace Markdown;

public class MarkdownParser : IMarkdownParser
{
    private readonly Dictionary<string, TokenType> _pairMarkdownToTag = new()
    {
        {"_", TokenType.Italic},
        {"__", TokenType.Bold},
        {"#", TokenType.Title},
        {"*", TokenType.ListItem},
    };

    private readonly HashSet<TokenType> _unpairTags = new() { TokenType.Title, TokenType.ListItem, TokenType.List };
    
    public string Render(string markdown)
    {
        var tokensList = TokenizeText(markdown);
        var htmlWithPairTags = BuildHTMLString(tokensList, markdown);
        return htmlWithPairTags;
    }

    public List<Token> TokenizeText(string markdown)
    {
        var stack = new Stack<RawToken>();
        var tokens = new List<Token>();
        var tagValidator = new MarkdownTagValidator(markdown);
        var isListOpened = false;
        for (var i = 0; i < markdown.Length; ++i)
        {
            if (!_pairMarkdownToTag.ContainsKey(markdown[i].ToString()))
            {
                var isNewLine = tagValidator.IsNewLineStarted(i);
                if (isNewLine && isListOpened && stack.Count > 0 && stack.Peek().Type == TokenType.ListItem
                    && i < markdown.Length - 1 && markdown[i + 1] != '*')
                {
                    var opening = stack.Pop();
                    if (tagValidator.HasTagContentInside(opening.StartIndex + 1, i - 1) &&
                        !tagValidator.HasTagDigitsInside(opening.StartIndex + 1, i - 1) &&
                        !tagValidator.IsTagPartsSplittingWord(opening.StartIndex, i))
                    {
                        tokens.Add(CreateToken(opening, i - 1));
                    }
                    tokens.Add(CreateToken(stack.Pop(), i));
                }
                else if (markdown[i] == '\n' && isListOpened && stack.Count > 0
                         && stack.Peek().Type == TokenType.ListItem)
                {
                    tokens.Add(CreateToken(stack.Pop(), i));
                }
                continue;
            }

            string currentTag = null;
            var tagLength = 1;
            if (markdown[i] == '_' && i + 1 < markdown.Length && markdown[i + 1] == '_')
            {
                currentTag = "__";
                tagLength = 2;
            }
            else
                currentTag = markdown[i].ToString();
            if (currentTag == "#" || currentTag == "*")
                tagLength = 2;
            
            if (currentTag == "__" && stack.Any(s => s.Type == TokenType.Italic))
            {
                i += tagLength - 1;
                continue;
            }

            var isOpening = stack.Count == 0 || stack.Peek().Type != _pairMarkdownToTag[currentTag];
            var isTagCorrect = tagValidator.IsTagPartCorrect(i, isOpening, tagLength)
                               || tagValidator.IsListOpening(i);
            if (!isTagCorrect)
            {
                i += tagLength - 1;
                continue;
            }
            if (stack.Count == 0 || stack.Peek().Type != _pairMarkdownToTag[currentTag]
                                 || _unpairTags.Contains(_pairMarkdownToTag[currentTag]))
            {
                if (!isListOpened && tagValidator.IsListOpening(i))
                {
                    var listStartInd = i;
                    if (i > 0)
                        --listStartInd;
                    // Длина марки 1 для отсеивания \n
                    stack.Push(new RawToken(TokenType.List, 1, listStartInd));
                    isListOpened = true;
                    stack.Push(new RawToken(_pairMarkdownToTag[currentTag], tagLength, i));
                    continue;
                }
                stack.Push(new RawToken(_pairMarkdownToTag[currentTag], tagLength, i));
            }
            else
            {
                if (_unpairTags.Contains(_pairMarkdownToTag[currentTag]))
                    continue;
                var opening = stack.Pop();
                if (tagValidator.HasTagContentInside(opening.StartIndex + tagLength, i - 1) &&
                    !tagValidator.HasTagDigitsInside(opening.StartIndex + tagLength, i - 1) &&
                    !tagValidator.IsTagPartsSplittingWord(opening.StartIndex, i))
                {
                    tokens.Add(CreateToken(opening, i));
                }
            }
            i += tagLength - 1;
        }

        var isLastCharUsed = false;
        while (stack.Count > 0)
        {
            if (_unpairTags.Contains(stack.Peek().Type)
                || stack.Peek().Type is TokenType.Italic && markdown[^1] == '_' && !isLastCharUsed)
            {
                tokens.Add(CreateToken(stack.Peek(), markdown.Length));
                isLastCharUsed = true;
            }

            stack.Pop();
        }
        return tokens;
    }

    public List<Token> AddTokenWrappers(List<Token> tokens)
    {
        var tagBuilder = new TagBuilder();
        foreach (var token in tokens)
        {
            var wrappers = tagBuilder.GetWrappers(token.Type.ToString());
            token.SetTokenWrappers(wrappers);
        }
        return tokens;
    }

    public string BuildHTMLString(List<Token> tokens, string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown) || tokens.Count == 0)
            return markdown;
        tokens = tokens
            .OrderBy(t => t.StartIndex)
            .ToList();
        tokens = AddTokenWrappers(tokens);
        var htmlString = new StringBuilder();
        var tagStartPositions = new Dictionary<int, Token>();
        var tagEndPositions = new Dictionary<int, Token>();
        var openedTags = new Stack<Token>();
        foreach (var token in tokens)
        {
            tagStartPositions.Add(token.StartIndex, token);
            tagEndPositions.TryAdd(token.StartIndex + token.TokenLength + token.TokenMarkLength, token);
        }

        for (var i = 0; i < markdown.Length; ++i)
        {
            if (tagStartPositions.ContainsKey(i))
            {
                htmlString.Append(tagStartPositions[i].TokenWrappers.TokenStart);
                openedTags.Push(tagStartPositions[i]);
                i += tagStartPositions[i].TokenMarkLength - 1;
                continue;
            }
            if (tagEndPositions.ContainsKey(i) && openedTags.Count > 0)
            {
                htmlString.Append(tagEndPositions[i].TokenWrappers.TokenEnd);
                openedTags.Pop();
                if (tagEndPositions[i].Type == TokenType.List)
                    i -= 1;
                else if (tagEndPositions[i].Type != TokenType.ListItem)
                    i += tagEndPositions[i].TokenMarkLength - 1;
                continue;
            }
            htmlString.Append(markdown[i]);
        }

        while (openedTags.Count > 0)
            htmlString.Append(openedTags.Pop().TokenWrappers.TokenEnd);
        return htmlString.ToString();
    }

    private Token CreateToken(RawToken rawToken, int endIndex)
    {
        var startIndex = rawToken.StartIndex;
        return new Token(startIndex, endIndex - rawToken.StartIndex - rawToken.TokenMarkLength, rawToken.Type, rawToken.TokenMarkLength);
    }
}