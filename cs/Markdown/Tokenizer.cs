using Markdown.Abstractions;
using Markdown.Primitives;

namespace Markdown;

public class Tokenizer : ITokenizer
{
    private static HashSet<char> _specialSymbols = new HashSet<char>
    {
        Characters.Underline,
        Characters.NewLine,
        Characters.Escape
    };
    
    private int currentIndex;
    private bool isNewLine = true;

    private string text;

    public IEnumerable<Token> Tokenize(string text)
    {
        this.text = text;
        if (string.IsNullOrEmpty(this.text))
            throw new ArgumentException("Text should not be null or empty");

        return Tokenize();
    }

    private IEnumerable<Token> Tokenize()
    {
        while (currentIndex < text.Length)
        {
            var ch = text[currentIndex];
            yield return ch switch
            {
                Characters.Sharp => TokenizeHeader1(),
                Characters.NewLine => TokenizeNewLine(),
                Characters.Escape => TokenizeEscape(),
                Characters.Underline => TokenizeUnderline(),
                _ => TokenizeText()
            };
            currentIndex++;
        }
    }

    private Token TokenizeHeader1()
    {
        if (!isNewLine || !IsNext(Characters.WhiteSpace))
            return TokenizeText();
        currentIndex++;
        return Tokens.Header1;
    }

    private Token TokenizeNewLine()
    {
        isNewLine = true;
        return Tokens.NewLine;
    }

    private Token TokenizeEscape()
    {
        isNewLine = false;
        return Tokens.Escape;
    }

    private Token TokenizeUnderline()
    {
        if (!IsNext(Characters.Underline))
            return Tokens.Italic;

        currentIndex++;
        return Tokens.Bold;
    }

    private Token TokenizeText()
    {
        var start = currentIndex;
        var end = currentIndex + 1;
        while (end < text.Length && !_specialSymbols.Contains(text[end]))
        {
            end++;
        }

        currentIndex = end - 1;
        return Tokens.Text(text.Substring(start, end - start));
    }

    private bool IsNext(char ch)
    {
        return currentIndex + 1 < text.Length && text[currentIndex + 1] == ch;
    }
}