namespace Markdown;

public class MarkdownLexerInput(string input)
{
    public bool NextIsDoubleGround(int position) =>
        position + 2 < input.Length && input[position + 1] == MarkdownSymbols.GroundChar &&
        input[position + 2] == MarkdownSymbols.GroundChar;

    public bool NextIsSpace(int position) =>
        position + 1 < input.Length && input[position + 1] == MarkdownSymbols.SpaceChar;

    public bool NextIsGround(int position) =>
        position + 1 < input.Length && input[position + 1] == MarkdownSymbols.GroundChar;

    public bool CurrentIsDigit(int position) => char.IsDigit(input[position]);

    public bool IsStartOfParagraph(int position) =>
        position == 0 || position > 0 && input[position - 1] == MarkdownSymbols.NewLineChar;
    
    public char this[int index] => input[index];
    public int Length => input.Length;
}