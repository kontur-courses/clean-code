using System.Net.Mime;
using Markdown.TagReaders;
using Markdown.Tokens;

namespace Markdown;

public class Tokenizer(TagReadersProvider tagReadersProvider)
{
    public List<Token> Tokenize(string text)
        => Tokenize(text, 0, text.Length);

    public List<Token> Tokenize(string text, int start, int end)
    {
        var tokens = new List<Token>();
        var position = start;
        while (position < end)
        {
            if (text[position] == '\\')
            {
                position = ReadBackslash(text, position, tokens);
            }

            else if (text[position] == '\n')
            {
                tokens.Add(new LineFeedToken("\n", position));
                position++;
            }

            else if (tagReadersProvider.TryGetReader(text[position], out var reader))
            {
                position = ReadTag(reader, text, position, tokens);
            }
            else
                position = ReadText(text, position, end, tokens);
        }

        return tokens;
    }

    private int ReadBackslash(string text, int start, List<Token> tokens)
    {
        if (start + 1 < text.Length && (tagReadersProvider.IsTagSymbol(text[start + 1]) || text[start + 1] == '\\'))
        {
            tokens.Add(new Token(text[start + 1].ToString(), start));
            return start + 2;
        }

        tokens.Add(new Token("\\", start));
        return start + 1;
    }

    private int ReadTag(ITagReader reader, string text, int position,
        List<Token> tokens)
    {
        var startToken = reader.Read(text, position, out var endToken);
        tokens.Add(startToken);

        if (endToken == null)
            return startToken.End;

        foreach (var nestedToken in Tokenize(text, startToken.End, endToken.Start))
            tokens.Add(nestedToken);

        tokens.Add(endToken);

        return endToken.End;
    }

    private int ReadText(string text, int start, int stopAt, List<Token> tokens)
    {
        var end = start;
        while (stopAt > end && text[end] != '\\' && text[end] != '\n' && !tagReadersProvider.IsTagOpener(text[end]))
            end++;

        tokens.Add(new Token(text.Substring(start, end - start), start));
        return end;
    }
}