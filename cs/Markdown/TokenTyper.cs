using Markdown.Enums;
using Markdown.Extensions;
using Markdown.Interfaces;

namespace Markdown;

public class TokenTyper : ITokenTyper<TokenType>
{
    private readonly string line;

    private readonly HashSet<char> serviceSymbols = new() { '\\', '_', '#' };

    private readonly ITagCondition<TokenType> tagCondition;

    public TokenTyper(string line, ITagCondition<TokenType> condition)
    {
        this.line = line;
        tagCondition = condition;
    }

    public TokenType GetSymbolType(int index)
    {
        switch (line[index])
        {
            case '_':
                if (IsDoubleUnderscore(index))
                    return !tagCondition.GetTagOpenStatus(TokenType.Strong) &&
                           tagCondition.GetTagOpenStatus(TokenType.Italic)
                        ? TokenType.Text
                        : TokenType.Strong;

                if (IsUnderscore(index))
                    return TokenType.Italic;

                break;
            case '\\':
                if (index + 1 < line.Length && serviceSymbols.Contains(line[index + 1]))
                    return TokenType.Slash;
                break;
            case '#':
                if (index == 0)
                    return TokenType.Header;
                break;
        }

        return TokenType.Text;
    }

    private bool IsDoubleUnderscore(int index)
    {
        if (!line.HasElementAt(index+1))
            return false;
        if (line.HasElementAt(index + 1) && line[index + 1] != '_')
            return false;

        if (TagInMiddleOfNumber(index, index + 1))
            return false;

        if (DoubleUnderscoreInMiddleOfWorld(index))
            return tagCondition.GetTagOpenStatus(TokenType.Strong)
                ? line.UntilEndOfWordHasChar(index - 2, '_', true)
                : line.UntilEndOfWordHasChar(index + 2, '_');



        return (!tagCondition.GetTagOpenStatus(TokenType.Strong) && line.IsOpenTag(index)) ||
                   (tagCondition.GetTagOpenStatus(TokenType.Strong) && line.IsCloseTag(index));
    }

    private bool IsUnderscore(int index)
    {
        if (TagInMiddleOfNumber(index, index))
            return false;

        if (tagCondition.GetTagOpenStatus(TokenType.Italic))
            return
                (UnderscoreInMiddleOfWorld(index) && line.UntilEndOfWordHasChar(index - 1, '_', true)) ||
                (line.IsCloseTag(index) && line[index-1] != '_' && !(line.HasElementAt(index+1) && line[index+1] != ' '));


        return
            (line.IsOpenTag(index) && (index == 0 || line[index - 1] == ' ' || line[index - 1] == '\\')) ||
            (line.CharInMiddleOfWord(index) && line.UntilEndOfWordHasChar(index + 1, '_'));
    }

    private bool TagInMiddleOfNumber(int start, int end)
    {
        return line.CharInMiddleOfWord(start)
               && char.IsDigit(line[start - 1])
               && char.IsDigit(line[end + 1]);
    }

    private bool DoubleUnderscoreInMiddleOfWorld(int index)
    {
        if(!line.CharInMiddleOfWord(index))
            return false;

        return !serviceSymbols.Contains(line[index - 1]) && (line.HasElementAt(index + 2) &&
                                                             !serviceSymbols.Contains(line[index + 2]) &&
                                                             line[index + 2] != ' ');
    }

    private bool UnderscoreInMiddleOfWorld(int index)
    {
        if (!line.CharInMiddleOfWord(index))
            return false;

        return !serviceSymbols.Contains(line[index - 1]) && !serviceSymbols.Contains(line[index + 1]);
    }
}