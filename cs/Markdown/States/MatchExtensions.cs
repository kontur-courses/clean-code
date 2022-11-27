using System.Text;

namespace Markdown.States;

public static class MatchExtensions
{
    public static bool IsStateForPlaceTextualToken(this ProcessState state)
    {
        return state is ProcessState.ReadParagraph or
            ProcessState.ReadHeader or
            ProcessState.ReadBoldText or
            ProcessState.ReadUnorderedListItem or
            ProcessState.EndReadBoldText or
            ProcessState.EndReadItalicText or
            ProcessState.EndReadPlainText;
    }

    public static bool IsOneOf<T>(this T element, params T[] items) where T : notnull
    {
        return items.Any(item => item.Equals(element));
    }

    public static bool IsStateForPlaceContainerToken(this ProcessState processState)
    {
        return processState is ProcessState.ReadDocument or ProcessState.EndReadHeader or ProcessState.EndReadParagraph
            or ProcessState.EndReadUnorderedList;
    }

    public static bool IsNotOneOf<T>(this T element, params T[] items) where T : notnull
    {
        return items.All(item => !item.Equals(element));
    }

    public static bool IsEndOfLine(this State state)
    {
        return state.EndOfFile || state.Input == "\n";
    }

    public static IEnumerable<char> AsEnumerable(this StringBuilder stringBuilder)
    {
        foreach (var chunk in stringBuilder.GetChunks())
            for (var index = 0; index < chunk.Span.Length; index++)
                yield return chunk.Span[index];
    }


    public static bool IsHighlightingInSeparateWords(this State state, int specialSequenceLength, int valueLength)
    {
        var start = state.Index - valueLength;
        var prevStart = start - specialSequenceLength - 1;
        var end = state.Index;
        var nextEnd = end + specialSequenceLength - 1;
        if (prevStart < 0 || nextEnd > state.Markdown.Length)
            return false;
        if (!char.IsLetterOrDigit(state.Markdown[prevStart]) && !char.IsPunctuation(state.Markdown[prevStart]))
            return false;
        if (!char.IsLetterOrDigit(state.Markdown[nextEnd]) && !char.IsPunctuation(state.Markdown[nextEnd]))
            return false;

        var s = state.Markdown[start..end];
        var firstSymbolIndex = s.IndexOfAnyExcept(' ');
        var lastSymbolIndex = s.LastIndexOfAnyExcept(' ');
        var firstSpaceIndex = s.IndexOf(' ');
        var lastSpaceIndex = s.LastIndexOf(' ');
        return firstSymbolIndex < firstSpaceIndex && lastSpaceIndex < lastSymbolIndex;
    }
}