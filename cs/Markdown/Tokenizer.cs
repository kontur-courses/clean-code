using Markdown.TagReaders;
using Markdown.Tokens;

namespace Markdown;

public class Tokenizer(TagReadersProvider tagReadersProvider)
{
    public List<TextToken> Tokenize(string text)
    {
        var tokens = new List<TextToken>();
        var position = 0;
        while (position < text.Length)
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

            var reader = tagReadersProvider.GetReader(text[position]);
            
            if (reader is ITagReaderWithNestedTokens readerWithNestedTokens)
                position = ReadNested(readerWithNestedTokens, text, position, tokens);

            else
                position = reader.Read(text, position, tokens);
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
        var nestedTokensToAdd = new List<TextToken>();
        position = readerWithNestedTokens.Read(text, position, tokens, out var nestedTextToRead, nestedTokensToAdd);

        foreach (var nestedToken in Tokenize(nestedTextToRead))
        {
            nestedTokensToAdd.Add(nestedToken);
        }

        return position;
    }
}
