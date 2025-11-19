using System.Text;

namespace Markdown;

public class MarkdownTokenizer
{
    private int position;
    private readonly string markdownText;
    private char Prev => position - 1 < 0 ? ' ' : markdownText[position - 1];
    private char Next => position + 1 == markdownText.Length ? ' ' : markdownText[position + 1];
    public MarkdownTokenizer(string markdownText)
    {
        this.markdownText = markdownText;
    }
    public List<Token> Tokenize()
    {   
        var tokens = new List<Token>();
        while (position < markdownText.Length)
        {
            var symbol = markdownText[position];
            var token = symbol switch
            {
                '#' => TokenizeHeader(),
                '_' => TokenizeUnderscore(),
                '!' => TokenizeImage(), 
                '(' => TokenizeImage(),
                ')' => TokenizeImage(),
                '[' => TokenizeImage(),
                ']' => TokenizeImage(),
                '\\' => TokenizeEscape(),
                ' ' => TokenizeSpace(),
                '\n' => TokenizeNewLine(),
                _ => TokenizeText()
            };

            tokens.Add(token);
            position++;
        }

        var eof = TokenizeEndOfFile();
        tokens.Add(eof);
        return tokens;
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
        return new Token(@"\", TokenType.Escape);
    }

    private Token TokenizeHeader()
    {   
        return new Token("#", TokenType.Hash);
    }

    private Token TokenizeUnderscore()
    {
        var prevSymbol = Prev;
        var nextSymbol = Next;
        if (nextSymbol != '_')
        {
            if (prevSymbol != ' ' && prevSymbol != '_' && nextSymbol != ' ')
            {   
                return new Token("_", TokenType.WordUnderscore);
            }
            return new Token("_", TokenType.Underscore);

        }

        position++;
        nextSymbol = Next;
        if (prevSymbol != ' ' && nextSymbol != ' ' && nextSymbol != '_')
        {   
            return new Token("__", TokenType.WordDoubleUnderscore);
        }
        return new Token("__", TokenType.DoubleUnderscore);

    }

    private Token TokenizeImage()
    { 
        var symbol = markdownText[position];
        return symbol switch
        {
            '!' => new Token("!", TokenType.Exclamation),
            '(' => new Token("(", TokenType.LParenthesis),
            ')' => new Token(")", TokenType.RParenthesis),
            '[' => new Token("[", TokenType.LBracket),
            ']' => new Token("]", TokenType.RBracket),
            _ => throw new ArgumentOutOfRangeException(nameof(symbol))
        };
    }

    private Token TokenizeText()
    {
        var start = position;
        while (position < markdownText.Length && !IsSpecialSymbol(markdownText[position]))
            position++;

        var text = markdownText[start..position];
        position--;
        
        return new Token(text, TokenType.Text);
    }

    private bool IsSpecialSymbol(char symbol)
        => specialsSymbols.Contains(symbol);

    private readonly HashSet<char> specialsSymbols = ['#', '_', '!', '[', ']', '(', ')', ' ', '\n', '\\'];
}