using Markdown.Enums;
using Markdown.Extensions;
using Markdown.Interfaces;

namespace Markdown;

public class TokenTyper : ITokenTyper<TokenType>
{
    private readonly string _line;

    private static readonly IReadOnlySet<char> ServiceSymbols = new HashSet<char> { '\\', '_', '#' };

    private readonly ITagCondition<TokenType> tagCondition;

    public TokenTyper(string line, ITagCondition<TokenType> condition)
    {
        this._line = line;
        tagCondition = condition;
    }

    public TokenType GetSymbolType(int index)
    {
        switch (_line[index])
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
                if (index + 1 < _line.Length && ServiceSymbols.Contains(_line[index + 1]))
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
        if (!_line.HasElementAt(index + 1) || _line[index + 1] != '_')
            return false;

        if (TagInMiddleOfNumber(index, index + 1))
            return false;

        if (DoubleUnderscoreInMiddleOfWorld(index))
            return tagCondition.GetTagOpenStatus(TokenType.Strong)
                ? _line.UntilEndOfWordHasChar(index - 2, '_', true)
                : _line.UntilEndOfWordHasChar(index + 2, '_');

        return (!tagCondition.GetTagOpenStatus(TokenType.Strong) && _line.IsOpenTag(index)) ||
               (tagCondition.GetTagOpenStatus(TokenType.Strong) && _line.IsCloseTag(index));
    }

    private bool IsUnderscore(int index)
    {
        if (TagInMiddleOfNumber(index, index))
            return false;

        if (tagCondition.GetTagOpenStatus(TokenType.Italic))
            return
                (UnderscoreInMiddleOfWorld(index) && _line.UntilEndOfWordHasChar(index - 1, '_', true)) ||
                (_line.IsCloseTag(index) && _line[index-1] != '_' && !(_line.HasElementAt(index+1) && _line[index+1] != ' '));


        return
            (_line.IsOpenTag(index) && (index == 0 || _line[index - 1] == ' ' || _line[index - 1] == '\\')) ||
            (_line.CharInMiddleOfWord(index) && _line.UntilEndOfWordHasChar(index + 1, '_'));
    }

    private bool TagInMiddleOfNumber(int start, int end)
    {
        return _line.CharInMiddleOfWord(start)
               && char.IsDigit(_line[start - 1])
               && char.IsDigit(_line[end + 1]);
    }

    private bool DoubleUnderscoreInMiddleOfWorld(int index)
    {
        if(!_line.CharInMiddleOfWord(index))
            return false;

        return !ServiceSymbols.Contains(_line[index - 1]) && (_line.HasElementAt(index + 2) &&
                                                             !ServiceSymbols.Contains(_line[index + 2]) &&
                                                             _line[index + 2] != ' ');
    }

    private bool UnderscoreInMiddleOfWorld(int index)
    {
        if (!_line.CharInMiddleOfWord(index))
            return false;

        return !ServiceSymbols.Contains(_line[index - 1]) && !ServiceSymbols.Contains(_line[index + 1]);
    }
}