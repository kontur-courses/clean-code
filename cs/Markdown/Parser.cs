using System.Text;

namespace Markdown;

public class Parser
{
    private readonly Dictionary<string, Tag> tagDict;
    private bool hasDigit;
    private bool hasSpaces;
    private int index;
    private List<Token> list;
    private string text;

    public Parser(Dictionary<string, Tag> tagDict)
    {
        this.tagDict = tagDict;
        list = new List<Token>();
    }

    public List<Token> Parse(string text)
    {
        this.text = text;
        list = new List<Token>();
        index = 0;
        for (; index < text.Length; index++)
            if (IsTagStart(text[index].ToString())) AddToken(TokenStatus.Tag);

            else if (text[index].ToString() == @"\") AddToken(TokenStatus.EscapeTag);

            else AddToken(TokenStatus.Text);
        return list;
    }

    private void AddToken(TokenStatus status)
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

        index--;
        HandleEscapeTag(ref status);
        list.Add(GetToken(status, content, startIndex));
    }

    private Func<char, bool> GetLoopStopCondition(TokenStatus status, StringBuilder content)
    {
        return status switch
        {
            TokenStatus.Text => currentChar => !IsTagStart(currentChar.ToString()) && currentChar.ToString() != @"\",
            TokenStatus.Tag => currentChar => IsTagSequenceEnd(content.ToString(), currentChar),
            TokenStatus.EscapeTag => _ => content.ToString() == string.Empty,
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
        return list.Count > 0 && list.Last().Status == TokenStatus.EscapeTag;
    }

    private void HandleEscapeTag(ref TokenStatus status)
    {
        if (!IsEscapeTag()) return;
        if (status is TokenStatus.Tag or TokenStatus.EscapeTag)
            status = TokenStatus.Text;
        else
            list.Last().Status = TokenStatus.Text;
    }

    private Token GetToken(TokenStatus status, StringBuilder content, int startIndex)
    {
        var followingChar = index == text.Length - 1 || status != TokenStatus.Tag ? "" : text[index + 1].ToString();
        var precedingChar = startIndex == 0 || status != TokenStatus.Tag ? "" : text[startIndex - 1].ToString();
        var token = new Token(content.ToString(), status, precedingChar, followingChar)
        {
            isDigitText = hasDigit,
            isSpaces = hasSpaces
        };
        return token;
    }
}