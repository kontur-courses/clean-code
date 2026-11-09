using System.Net.Mime;
using Markdown.TagReaders;
using Markdown.Tokens;

namespace Markdown;

public class Tokenizer(TagReadersProvider tagReadersProvider)
{
    public List<TextToken> Tokenize(string text)
        => Tokenize(text, 0, text.Length);
    
    public List<TextToken> Tokenize(string text, int start, int end)
    {
        var tokens = new List<TextToken>();
        var position = start;
        while (position < end)
        {
            if (text[position] == '\\')
            {
                position = ReadBackslash(text, position, tokens);
                continue;
            }

            if (text[position] == '\n')
            {
                tokens.Add(new LineFeedToken("\n", position));
                position++;
                continue;
            }

            if (tagReadersProvider.TryGetReader(text[position], out var reader))
            {
                if (reader is ITagReaderWithNestedTokens readerWithNestedTokens)
                    position = ReadNested(readerWithNestedTokens, text, position, tokens);

                else
                    position = reader.Read(text, position, tokens);
            }
            else 
                position = ReadText(text, position, end, tokens);
            
        }
        return tokens;
    }

    private int ReadBackslash(string text, int start, List<TextToken> tokens)
    {
        if (start + 1 < text.Length && (tagReadersProvider.IsTagSymbol(text[start + 1]) || text[start + 1] == '\\'))
        {
            tokens.Add(new TextToken(text[start + 1].ToString(), start));
            return start + 2;
        }

        tokens.Add(new TextToken("\\", start));
        return start + 1;
    }

    private int ReadNested(ITagReaderWithNestedTokens readerWithNestedTokens, string text, int position, List<TextToken> tokens)
    {
        var newPosition = readerWithNestedTokens.Read(text, position, tokens, out var nestedStart, out var endToken);

        if (endToken != null)
        {
            foreach (var nestedToken in Tokenize(text, nestedStart, endToken.Start))
            {
                tokens.Add(nestedToken);
            }
            tokens.Add(endToken);
        }
        
        return newPosition;
    }
    
    private int ReadText(string text, int start, int stopAt,  List<TextToken> tokens)
    {
        var end = start;
        while (stopAt > end && text[end] != '\\' && text[end] != '\n' && !tagReadersProvider.IsTagOpener(text[end]))
            end++;

        tokens.Add(new TextToken(text.Substring(start, end - start), start));
        return end;
    }
}
