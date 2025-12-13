namespace Markdown.Tokenizer;

public class MarkdownTokenizer
{
    private readonly string markdownText;
    private readonly HashSet<char> specialsSymbols = ['#', '_', '!', '[', ']', '(', ')', ' ', '\n', '\\', '\r'];
    private readonly HashSet<char> escapableSymbols = ['#', '_', '!', '[', ']', '(', ')', '\\'];
    private char? PrevSymbol => Position - 1 < 0 ? null : markdownText[Position - 1];
    private char? NextSymbol => Position + 1 == markdownText.Length ? null : markdownText[Position + 1];
    private int Position { get; set; }

    public MarkdownTokenizer(string markdownText)
    {
        this.markdownText = markdownText;
    }
    
    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();
        while (Position < markdownText.Length)
        {
            var symbol = markdownText[Position];
            var token = symbol switch
            {
                '#' => TokenizeHeader(),
                '_' => TokenizeUnderscore(),
                '!' => TokenizeExclamation(),
                '(' => TokenizeLParenthesis(),
                ')' => TokenizeRParenthesis(),
                '[' => TokenizeLBracket(),
                ']' => TokenizeRBracket(),
                '\\' => TokenizeEscape(),
                ' ' => TokenizeSpace(),
                '\n' => TokenizeNewLine(),
                '\r' => TokenizeCarriage(),
                _ => TokenizeText()
            };
            tokens.Add(token);
            Position++;
        }

        var eof = TokenizeEndOfFile();
        tokens.Add(eof);
        return tokens;
    }

    private Token TokenizeCarriage()
    {
        return new Token("\r", TokenType.Carriage);
    }

    private Token TokenizeHeader()
    {
        if (PrevSymbol is null or '\n' && NextSymbol is ' ') return new Token("#", TokenType.Hash);

        return new Token("#", TokenType.Text);
    }

    private Token TokenizeEndOfFile()
    {
        return new Token("", TokenType.Eof);
    }

    private Token TokenizeNewLine()
    {
        return new Token("\n", TokenType.NewLine);
    }

    private Token TokenizeSpace()
    {
        return new Token(" ", TokenType.Space);
    }

    private Token TokenizeEscape()
    {
        return NextSymbol != null && IsEscapableSymbol(NextSymbol.Value)
            ? new Token(@"\", TokenType.Escape)
            : new Token(@"\", TokenType.Text);
    }

    private Token TokenizeUnderscore()
    {
        var startPos = Position;

        if (NextSymbol != '_')
        {
            if (!IsValidUnderscores(PrevSymbol, NextSymbol))
                return new Token("_", TokenType.Text);

            if (IsInsideWord(PrevSymbol, NextSymbol))
                return IsValidWordUnderscores(PrevSymbol!.Value, NextSymbol!.Value)
                    ? new Token("_", TokenType.WordUnderscore)
                    : new Token("_", TokenType.Text);

            return new Token("_", TokenType.Underscore);
        }

        Position++;
        char? leftSymbol = startPos == 0 ? null : markdownText[startPos - 1];
        if (!IsValidUnderscores(leftSymbol, NextSymbol))
            return new Token("__", TokenType.Text);

        if (IsInsideWord(leftSymbol, NextSymbol))
            return IsValidWordUnderscores(markdownText[startPos - 1], NextSymbol.Value)
                ? new Token("__", TokenType.WordDoubleUnderscore)
                : new Token("__", TokenType.Text);

        return new Token("__", TokenType.DoubleUnderscore);
    }

    private static bool IsValidWordUnderscores(char left, char right)
    {
        if (char.IsDigit(right) && char.IsDigit(left))
            return false;

        if (char.IsDigit(right) && char.IsLetter(left))
            return false;

        if (char.IsDigit(left) && char.IsLetter(right))
            return false;

        return true;
    }

    private static bool IsInsideWord(char? left, char? right)
    {
        return right != null && left != null
                             && char.IsLetterOrDigit(left.Value)
                             && char.IsLetterOrDigit(right.Value);
    }

    private bool IsValidUnderscores(char? left, char? right)
    {
        var leftValid = left.HasValue && left != ' ';
        var rightValid = right.HasValue && right != ' ';

        return leftValid || rightValid;
    }

    private Token TokenizeLParenthesis()
    {
        return new Token("(", TokenType.LParenthesis);
    }

    private Token TokenizeRParenthesis()
    {
        return new Token(")", TokenType.RParenthesis);
    }

    private Token TokenizeLBracket()
    {
        return new Token("[", TokenType.LBracket);
    }

    private Token TokenizeRBracket()
    {
        return new Token("]", TokenType.RBracket);
    }

    private Token TokenizeExclamation()
    {
        return NextSymbol == '[' ? new Token("!", TokenType.Exclamation) : new Token("!", TokenType.Text);
    }

    private Token TokenizeText()
    {
        var start = Position;
        while (Position < markdownText.Length && !IsSpecialSymbol(markdownText[Position]))
            Position++;

        var text = markdownText[start..Position];
        Position--;

        return new Token(text, TokenType.Text);
    }

    private bool IsSpecialSymbol(char c)
    {
        return specialsSymbols.Contains(c);
    }

    private bool IsEscapableSymbol(char c)
    {
        return escapableSymbols.Contains(c);
    }
}