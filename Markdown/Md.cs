using System.Text;

namespace Markdown;

public class Md
{
    public string Render(string text)
    {
        var sb = new StringBuilder();

		var mdSymbols = SplitIntoSubstrings(text.AsMemory(), t => t == '\n')
			.Select(t => SplitIntoSubstrings(t, char.IsWhiteSpace))
            .SelectMany(MdKeySymbol.GetMdKeySymbolsInLine)
            .ToArray();

        MdKeySymbol.Validate(mdSymbols);

        var onlyValidatedMdSymbols = mdSymbols
            .Where(t => t.IsValid)
            .ToArray();

        return BuildConvertedString(text, onlyValidatedMdSymbols);
    }

    private IEnumerable<ReadOnlyMemory<char>> SplitIntoSubstrings(ReadOnlyMemory<char> text, Func<char, bool> predicate)
    {
        var lastIndex = 0;

        for (var i = 0; i < text.Length; i++)
        {
            if (!predicate(text.Span[i]))
                continue;

            if (lastIndex != i)
				yield return text[lastIndex..i];
            yield return text[i..(i + 1)];
			lastIndex = i + 1;
        }

        if (lastIndex != text.Length)
            yield return text[lastIndex..^0];
    }

    private string BuildConvertedString(string text, MdKeySymbol[] mdSymbols)
    {
        if (mdSymbols.Length == 0)
            return text;

        var sb = new StringBuilder();
        var stringIndex = 0;
        var mdSymbolIndex = 0;

        while (stringIndex < text.Length || mdSymbolIndex < mdSymbols.Length)
        {
            if (mdSymbolIndex == mdSymbols.Length
                || stringIndex < mdSymbols[mdSymbolIndex].Position)
            {
				sb.Append(text[stringIndex++]);
                continue;
			}

            var mdSymbol = mdSymbols[mdSymbolIndex++];
            sb.Append(mdSymbol.ToHtmlTag());

            if (stringIndex < text.Length)
                stringIndex += mdSymbol.CalculateSymbolLength();
        }

        return sb.ToString();
    }
}
