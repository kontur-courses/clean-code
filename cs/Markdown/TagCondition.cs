using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown;

public class TagCondition : ITagCondition<TokenType>
{
    private readonly Dictionary<TokenType, bool> tagIsOpen = new()
    {
        [TokenType.Strong] = false,
        [TokenType.Italic] = false,
        [TokenType.Header] = false
    };

    private readonly Dictionary<TokenType, int> tagOpenIndex = new()
    {
        [TokenType.Italic] = 0,
        [TokenType.Strong] = 0,
        [TokenType.Header] = 0
    };

    private readonly Dictionary<TokenType, string> tagStrings = new()
    {
        [TokenType.Italic] = "_",
        [TokenType.Strong] = "__"
    };

    public int GetOpenIndex(TokenType type)
    {
        return tagOpenIndex[type];
    }

    public bool GetTagOpenStatus(TokenType type)
    {
        return tagIsOpen[type];
    }

    public void OpenTag(TokenType type, int index)
    {
        tagIsOpen[type] = true;
        tagOpenIndex[type] = index;
    }

    public void CloseTag(TokenType type)
    {
        tagIsOpen[type] = false;
    }

    public string GetTag(TokenType type)
    {
        return tagStrings[type];
    }

}