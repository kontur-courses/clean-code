using System.Text;
using Markdown.Tokens;

namespace Markdown;

public class MarkdownLexer : ILexer
{
    private int position;
    private readonly List<Token> tokens = [];

    private readonly char[] escapedChars =
    [
        MarkdownSymbols.SharpChar, MarkdownSymbols.GroundChar, MarkdownSymbols.EscapeChar, MarkdownSymbols.NewLineChar
    ];

    public List<Token> Tokenize(string input) => Tokenize(new MarkdownLexerInput(input));

    private List<Token> Tokenize(MarkdownLexerInput input)
    {
        position = 0;
        var nestingStack = new Stack<string>();

        while (position < input.Length)
        {
            switch (input[position])
            {
                case MarkdownSymbols.SpaceChar:
                    ParseSpaceAndAdvance();
                    break;
                case MarkdownSymbols.NewLineChar:
                    ParseNewLineAndAdvance(nestingStack);
                    break;
                case MarkdownSymbols.EscapeChar:
                    ParseEscapeAndAdvance(input);
                    break;
                case MarkdownSymbols.GroundChar:
                    ParseItalicOrBoldAndAdvance(input, nestingStack);
                    break;
                case MarkdownSymbols.SharpChar:
                    ParseHeadingAndAdvance(input);
                    break;
                default:
                    ParseTextAndAdvance(input);
                    break;
            }
        }

        return tokens;
    }

    private void ParseSpaceAndAdvance() => tokens.Add(new SpaceToken(position++));

    private void ParseHeadingAndAdvance(MarkdownLexerInput input)
    {
        if (input.NextIsSpace(position) && input.IsStartOfParagraph(position)) tokens.Add(new HeadingToken(position++));
        else tokens.Add(new TextToken(position, MarkdownSymbols.Sharp));
        position++;
    }

    private void ParseTextAndAdvance(MarkdownLexerInput input)
    {
        var value = new StringBuilder();
        var start = position;
        var endChars = new[]
        {
            MarkdownSymbols.SharpChar, MarkdownSymbols.GroundChar, MarkdownSymbols.NewLineChar,
            MarkdownSymbols.EscapeChar, MarkdownSymbols.SpaceChar
        };
        while (position < input.Length && !endChars.Contains(input[position]) && !input.CurrentIsDigit(position))
            value.Append(input[position++]);

        if (value.Length > 0) tokens.Add(new TextToken(start, value.ToString()));
        if (position < input.Length && input.CurrentIsDigit(position)) ParseNumberAndAdvance(input);
    }


    private void ParseNumberAndAdvance(MarkdownLexerInput input)
    {
        var sb = new StringBuilder();
        var start = position;
        while (position < input.Length && (input.CurrentIsDigit(position) || input[position] == MarkdownSymbols.GroundChar))
            sb.Append(input[position++]);
        tokens.Add(new NumberToken(start, sb.ToString()));
    }

    private void ParseItalicOrBoldAndAdvance(MarkdownLexerInput input, Stack<string> stack)
    {
        var isDoubleGround = input.NextIsGround(position);
        var isTripleGround = input.NextIsDoubleGround(position);
        var isSingleGround = !isTripleGround && !isDoubleGround;
        if (stack.Count == 0) ParseItalicOrBoldAndAdvanceWhenStackEmpty(isSingleGround, isTripleGround, stack);
        else if (stack.Count == 1)
            ParseItalicOrBoldAndAdvanceWhenStackHasOne(isSingleGround, isDoubleGround, isTripleGround, stack);
        else if (stack.Count == 2) ParseItalicOrBoldAndAdvanceWhenStackHasTwo(isSingleGround, isTripleGround, stack);
    }

    private void ParseItalicOrBoldAndAdvanceWhenStackEmpty(bool isSingleGround, bool isTripleGround,
        Stack<string> stack)
    {
        if (isSingleGround)
        {
            ParseItalicAndAdvance();
            stack.Push(MarkdownSymbols.Ground);
            return;
        }

        ParseBoldAndAdvance();
        stack.Push(MarkdownSymbols.DoubleGround);
        if (!isTripleGround) return;
        ParseItalicAndAdvance();
        stack.Push(MarkdownSymbols.Ground);
    }

    private void ParseItalicOrBoldAndAdvanceWhenStackHasOne(bool isSingleGround, bool isDoubleGround,
        bool isTripleGround,
        Stack<string> stack)
    {
        switch (stack.Peek())
        {
            case MarkdownSymbols.DoubleGround when isSingleGround:
                ParseItalicAndAdvance();
                stack.Push(MarkdownSymbols.Ground);
                break;
            case MarkdownSymbols.DoubleGround:
            {
                if (isTripleGround) ParseItalicAndAdvance();
                ParseBoldAndAdvance();
                stack.Pop();
                break;
            }
            case MarkdownSymbols.Ground:
            {
                if (isTripleGround)
                {
                    ParseBoldAndAdvance();
                    ParseItalicAndAdvance();
                }
                else if (isDoubleGround)
                {
                    tokens.Add(new TextToken(position, MarkdownSymbols.DoubleGround));
                    position += 2;
                }
                else ParseItalicAndAdvance();

                stack.Pop();
                break;
            }
        }
    }

    private void ParseItalicOrBoldAndAdvanceWhenStackHasTwo(bool isSingleGround, bool isTripleGround,
        Stack<string> stack)
    {
        if (isSingleGround)
        {
            ParseItalicAndAdvance();
            stack.Pop();
            return;
        }

        if (isTripleGround) ParseItalicAndAdvance();
        ParseBoldAndAdvance();

        stack.Pop();
        stack.Pop();
    }

    private void ParseBoldAndAdvance()
    {
        tokens.Add(new BoldToken(position));
        position += 2;
    }

    private void ParseItalicAndAdvance()
    {
        tokens.Add(new ItalicToken(position));
        position++;
    }

    private void ParseNewLineAndAdvance(Stack<string> stack)
    {
        tokens.Add(new NewLineToken(position));
        stack.Clear();
        position++;
    }

    private void ParseEscapeAndAdvance(MarkdownLexerInput input)
    {
        if (position + 1 >= input.Length)
        {
            tokens.Add(new TextToken(position++, MarkdownSymbols.Escape));
            return;
        }

        if (input.NextIsDoubleGround(position))
        {
            tokens.Add(new TextToken(position, MarkdownSymbols.DoubleGround));
            position += 3;
            return;
        }

        var next = input[position + 1];
        tokens.Add(escapedChars.Contains(next)
            ? new TextToken(position, next.ToString())
            : new TextToken(position, MarkdownSymbols.Escape + next));
        position += 2;
    }
}