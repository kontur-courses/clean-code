namespace Markdown.Interfaces;

public class MdTokenTag
{
    private static readonly List<Func<char, bool>> DetectType = [char.IsDigit, char.IsWhiteSpace];

    private static readonly Dictionary<Func<char, bool>, TokenType> TokenTypes =
        new() // experiments (вывести в другой класс) 
        {
            { char.IsDigit, TokenType.Digit },
            { char.IsWhiteSpace, TokenType.WhiteSpace },
        };
}