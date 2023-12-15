using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Markdown;


public class Md
{

    private Dictionary<string, Tag> tagDict = new Dictionary<string, Tag>
    {
        { "__", new Tag(true,  TagType.Bold, new []{"_"}) },
        { "_", new Tag(true,  TagType.Italic) },
        { "# ", new Tag(false,  TagType.Heading) },
        { "\n", new Tag(false,  TagType.LineBreaker) },
        { "\r\n", new Tag(false,  TagType.LineBreaker) }

    };

    private readonly Dictionary<string, string> htmlTagDict = new Dictionary<string, string>
    {
        { "_", "<em>" },
        { "__", "<strong>" },
        { "# ", "<h1>" },
        { "\n", "</h1>" },
        { "\r\n", "</h1>" },
        { @"\", "" }
    };

    private List<Token> list;
    private StringBuilder content = new StringBuilder();
    private string text;
    private int index;
    public Md()
    {
        list = new List<Token>();
    }

    public string Render(string text)
    {
        return "string";
    }

    public List<Token> Parse(string text)
    {
        this.text = text;
        for (; index < text.Length; index++)
        {
            if(IsTagStart(text[index].ToString()))  AddToken(TokenStatus.Tag);
            
            else if(text[index].ToString() == @"\") AddToken(TokenStatus.EscapeTag);
            
            else AddToken(TokenStatus.Text);
        }
        return list;
    }

    private void AddToken(TokenStatus status)
    {
        var isDigit = false;
        var idSpaces = false;
        var prev = index == 0 || status != TokenStatus.Tag ? "" : text[index - 1].ToString();
        var content = new StringBuilder();
        while (index < text.Length && (status == TokenStatus.Text
                   ? !IsTagStart(text[index].ToString()) && text[index].ToString() != @"\"
                   : IsTagSequenceEnd(content.ToString(), text[index])))
        {
            idSpaces = idSpaces ? idSpaces : text[index] == ' ';
            isDigit = isDigit && text[index] != ' ' ? isDigit : char.IsDigit(text[index]);
            content.Append(text[index]);
            index++;
        }
        if (status == TokenStatus.EscapeTag)
        {
            content.Append(text[index]);
            index++;
        }
        index--;
        var last = index == text.Length - 1 || status != TokenStatus.Tag ? "" : text[index + 1].ToString();
        if (list.Count > 0 && list.Last().Status == TokenStatus.EscapeTag)
        {
            if (status is TokenStatus.Tag or TokenStatus.EscapeTag)
                status = TokenStatus.Text;
            else
            {
                list.Last().Status = TokenStatus.Text;

            }
            
        }
        var token = new Token(content.ToString(), status, prev, last)
        {
            isDigitText = isDigit,
            isSpaces = idSpaces
        };
        list.Add(token);
    }

    private bool IsTagStart(string content) => tagDict.Any(tag => tag.Key.StartsWith(content));

    private bool IsTag(string content) => tagDict.Any(tag => tag.Key == content);

    bool IsTagSequenceEnd(string currentContent, char currentChar)
        => IsTagStart(currentContent + currentChar) || IsTag(currentContent + currentChar);

    public List<Token> Checker_Token_(List<Token> ff)
    {
        var isSpaces = false;
        var isDigitText = false;
        var stack = new Stack<Token>();
        if(ff.All(x => x.Status != TokenStatus.Text)) return ff;
        foreach (var token in ff)
        {
            if (token.Status == TokenStatus.Text)
            {
                if (!isSpaces) isSpaces = token.isSpaces;
                if (!isDigitText) isDigitText= token.isDigitText;
                continue;
            }
            if (token.Status == TokenStatus.EscapeTag || !tagDict[token.Content].IsPaired)
            {
                token.Content = htmlTagDict[token.Content];
                continue;
            }

            if (!tagDict[token.Content].IsOpen)
            {
                tagDict[token.Content].IsOpen = true;
                stack.Push(token);
            }
            else
            {
                stack.TryPeek(out var tokenPeek);
                if (tokenPeek != null && tokenPeek.Content == token.Content && (!isSpaces || (token.Last is "" or " " && tokenPeek.Last != " " && token.Prev != " ")))
                {
                    tokenPeek = stack.Pop();
                    stack.TryPeek(out var prevtokenPeek);
                    if (prevtokenPeek != null && tagDict[token.Content].BlockTags.Contains(prevtokenPeek.Content) || isDigitText)
                    {
                        isDigitText = false;
                        tagDict[token.Content].IsOpen = false;
                        continue;
                    }
                    tagDict[token.Content].IsOpen = false;
                    tokenPeek.Content = htmlTagDict[token.Content];
                    token.Content = $"</{tokenPeek.Content[1..]}";
                    isSpaces = false;
                }
                else if(tokenPeek != null && tokenPeek.Content != token.Content)
                {
                    stack.Clear();
                    stack.Push(token);
                }
            }
        }

        return ff;
    }
    
}