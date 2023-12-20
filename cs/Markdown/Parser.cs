using System.Text;
using Markdown.Tags;

namespace Markdown;

public class Parser
{
    private readonly Dictionary<string, TagType> tagDict;
    private bool hasDigit;
    private bool hasSpaces;
    private int index;
    private List<Token> list;
    private string text;
    private Token previousToken;

    public Parser(Dictionary<string, TagType> tagDictionary)
    {
        this.tagDict =  tagDictionary;
    }

    public List<Token> Parse(string text)
    {
        this.text = text;
        list = new List<Token>();
        index = 0;
        for (; index < text.Length; index++)
            if (IsTagStart(text[index].ToString())) AddToken(TokenType.Tag);

            else if (text[index].ToString() == @"\") AddToken(TokenType.Escape);

            else AddToken(TokenType.Text);
        list.Add(new Token("\n", null, TokenType.LineBreaker));
        return list;
    }

    private void AddToken(TokenType status)
    {
        hasDigit = false;
        hasSpaces = false;
        var startIndex = index;
        var content = new StringBuilder();
        var loopStopCondition = GetLoopStopCondition(status, content);
        while (index < text.Length && loopStopCondition(text[index]))
        {
            hasSpaces = hasSpaces ? hasSpaces : text[index] == ' ';
            hasDigit = hasDigit && text[index] != ' ' ? hasDigit : char.IsDigit(text[index]);
            content.Append(text[index]);
            index++;
        }
        HandleEscapeTag(ref status);
        if (status == TokenType.Tag)
        {
            var nextChar = index == text.Length ? "" : text[index].ToString();
            var tag = Tag.CreateTag(tagDict[content.ToString()], content.ToString(), previousToken, nextChar);
            var token = new Token(content.ToString(), tag, status);
            previousToken = token;
            list.Add(token);
        }
        else
        {
            var token = new Token(content.ToString(), null, status);
            list.Add(new Token(content.ToString(), null, status));
            previousToken = token;
        }
        index--;
    }

    private Func<char, bool> GetLoopStopCondition(TokenType status, StringBuilder content)
    {
        return status switch
        {
            TokenType.Text => currentChar => !IsTagStart(currentChar.ToString()) && currentChar.ToString() != @"\",
            TokenType.Tag => currentChar => IsTagSequenceEnd(content.ToString(), currentChar),
            TokenType.Escape => _ => content.ToString() == string.Empty,
            _ => _ => true
        };
    }

    private bool IsTagStart(string content)
    {
        return tagDict.Any(tag => tag.Key.StartsWith(content));
    }

    private bool IsTag(string content)
    {
        return tagDict.Any(tag => tag.Key == content);
    }

    private bool IsTagSequenceEnd(string currentContent, char currentChar)
    {
        return IsTagStart(currentContent + currentChar) || IsTag(currentContent + currentChar);
    }

    private bool IsEscapeTag()
    {
        return list.Count > 0 && list.Last().Type == TokenType.Escape;
    }

    private void HandleEscapeTag(ref TokenType status)
    {
        if (!IsEscapeTag()) return;
        if (status is TokenType.Tag or TokenType.Escape)
            status = TokenType.Text;
        else
            list.Last().Type = TokenType.Text;
    }
}