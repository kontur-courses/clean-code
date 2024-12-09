using System.Text;
using Markdown.Tags;
using Markdown.Tags.ConcreteTags;
using Markdown.TokenGeneratorClasses;
using Markdown.TokenParser.Interfaces;
using Markdown.TokenParser.TokenHandlers;
using Markdown.Tokens;

namespace Markdown.TokenParser.ConcreteParser;

public class LineParser : ITokenLineParser
{
    private static readonly Dictionary<TagType, uint> TagPriority = new()
    {
        { TagType.Header, 1000 },
        { TagType.BulletedListItem, 1000 },
        { TagType.Bold, 500 },
        { TagType.Italic, 300 },
        { TagType.UnDefined, 0 }
    };

    public ParsedLine ParseLine(string line)
    {
        if (line is null)
            throw new ArgumentNullException("String argument text must be not null");

        var lineTokens = GetTokensLine(line);
        var escapedTokens = ResetPositions(EscapeTags(lineTokens));
        var headerTags = new HeaderTokensHandler().HandleLine(escapedTokens);
        var bulletedLITags = new BulletedLIHandler().HandleLine(escapedTokens);
        var italicTags = new ItalicTokensHandler().HandleLine(escapedTokens);
        var boldTags = new BoldTokensHandler().HandleLine(escapedTokens);

        var merged = MergeTokens(escapedTokens, headerTags, boldTags, italicTags, bulletedLITags);
        ProcessTokensIntersecting(merged);
        ProcessInnerTags(merged);

        return GetTagsAndCleanText(merged);
    }

    private List<Token> GetTokensLine(string line)
    {
        var position = 0;
        var result = new List<Token?>();

        while (position < line.Length)
        {
            var token = TokenGenerator.GetToken(line, position);
            result.Add(token);
            position += token.Content.Length;
        }

        return result;
    }

    private List<Token> EscapeTags(List<Token> tokens)
    {
        Token? previousToken = null;
        var result = new List<Token>();

        foreach (var token in tokens)
            if (previousToken is { TokenType: TokenType.Escape })
            {
                if (token.TokenType is TokenType.MdTag or TokenType.Escape)
                {
                    token.TokenType = TokenType.Text;
                    token.TagType = TagType.UnDefined;
                    previousToken = token;
                    result.Add(token);
                }
                else
                {
                    previousToken.TokenType = TokenType.Text;
                    result.Add(previousToken);
                    result.Add(token);
                    previousToken = token;
                }
            }
            else if (token.TokenType == TokenType.Escape)
            {
                previousToken = token;
            }
            else
            {
                result.Add(token);
                previousToken = token;
            }

        if (previousToken is not { TokenType: TokenType.Escape })
            return result;

        previousToken.TokenType = TokenType.Text;
        result.Add(previousToken);
        return result;
    }

    private List<Token> ResetPositions(List<Token> tokens)
    {
        var position = 0;

        foreach (var token in tokens)
        {
            token.Position = position;
            position += token.Content.Length;
        }

        return tokens;
    }

    private List<Token> MergeTokens(List<Token> allTokens, params List<Token>[] tokenLists)
    {
        var positionMap = new Dictionary<int, Token>();

        foreach (var token in allTokens) positionMap[token.Position] = token;

        foreach (var tokenList in tokenLists)
        foreach (var token in tokenList)
            positionMap[token.Position] = token;

        // Преобразуем словарь обратно в список
        var combinedTokens = positionMap.Values.ToList();

        var combindedTokens = combinedTokens.OrderBy(token => token.Position)
            .ToList();

        return combinedTokens;
    }

    private void ProcessTokensIntersecting(List<Token> tokens)
    {
        var stack = new Stack<Token>();
        var process = new List<Token>();

        foreach (var token in tokens)
            if (token.TagType == TagType.Italic || token.TagType == TagType.Bold)
            {
                process.Add(token);
                if (token.IsCloseTag)
                {
                    stack.TryPeek(out var openToken);
                    if (openToken != null && !openToken.IsCloseTag
                                          && openToken.TagType == token.TagType)
                    {
                        process.Remove(stack.Pop());
                        process.Remove(token);
                    }
                    else
                    {
                        stack.Push(token);
                    }
                }
                else
                {
                    stack.Push(token);
                }
            }

        foreach (var token in stack)
        {
            token.TagType = TagType.UnDefined;
            token.TokenType = TokenType.Text;
        }
    }

    private void ProcessInnerTags(List<Token> tokens)
    {
        Token handleToken = null;
        var setAllToText = false;
        var startIndex = -1;
        var endIndex = -1;

        for (var i = 0; i < tokens.Count; i++)
            if (handleToken == null)
            {
                if (setAllToText)
                    ConvertToTextTags(startIndex, endIndex, tokens);

                handleToken = tokens[i];
                startIndex = i;
            }
            else if (TagPriority[tokens[i].TagType] > TagPriority[handleToken.TagType])
            {
                setAllToText = true;
            }
            else if (tokens[i].TagType == handleToken.TagType && tokens[i].IsCloseTag)
            {
                endIndex = i;
            }
    }

    private void ConvertToTextTags(int startIndex, int endIndex, List<Token> tokens)
    {
        if (startIndex == -1 || endIndex == -1)
            return;

        for (var i = startIndex; i <= endIndex; i++)
            if (tokens[i].TokenType == TokenType.MdTag)
                tokens[i].TokenType = TokenType.Text;
    }

    private List<Token> ConvertToTextTags(List<Token> tokens)
    {
        var result = new List<Token>();

        foreach (var token in tokens)
        {
            if (token.TokenType == TokenType.MdTag) token.TokenType = TokenType.Text;

            result.Add(token);
        }

        return result;
    }


    private ParsedLine GetTagsAndCleanText(List<Token> tokens)
    {
        var result = new List<ITag>();

        var lineBuilder = new StringBuilder();
        foreach (var token in tokens)
        {
            if (token.TokenType is not TokenType.MdTag)
            {
                lineBuilder.Append(token.Content);
                continue;
            }

            result.Add(GetNewTag(token, lineBuilder.Length));
        }

        //Мы можем вернуть либо пустой ParsedLine если Count == 0 или "стандартно"
        //заполненный, в ином случае если Length == 0 то значит внутри тегов пусто
        //И их надо превратить в текст
        return lineBuilder.Length > 0 || tokens.Count == 0
            ? new ParsedLine(lineBuilder.ToString(), result)
            : GetTagsAndCleanText(ConvertToTextTags(tokens));
    }

    private ITag GetNewTag(Token token, int position)
    {
        return token.TagType switch
        {
            TagType.Header => new HeaderTag(position, token.IsCloseTag),
            TagType.Italic => new ItalicTag(position, token.IsCloseTag),
            TagType.Bold => new BoldTag(position, token.IsCloseTag),
            TagType.BulletedListItem => new BulletTag(position, token.IsCloseTag),
            _ => throw new NotImplementedException()
        };
    }
}