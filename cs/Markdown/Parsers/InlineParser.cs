using System.Text;
using Markdown.Entities;
using Markdown.Interfaces;

namespace Markdown.Parsers;

public sealed class InlineParser
{
    private readonly IInlineTag[] _tags;

    public InlineParser(IReadOnlyList<IInlineTag> tags)
    {
        _tags = tags
            .OrderByDescending(tag => tag.Marker.Length)
            .ToArray();
    }

    public string Parse(string markdownText)
    {
        if (string.IsNullOrEmpty(markdownText))
        {
            return string.Empty;
        }

        var markers = CollectMarkers(markdownText);
        PairMarkers(markdownText, markers);
        return Render(markdownText, markers);
    }

    private List<InlineMarker> CollectMarkers(string text)
    {
        var markers = new List<InlineMarker>();
        var index = 0;
        while (index < text.Length)
        {
            if (text[index] == '\\')
            {
                index += index + 1 < text.Length ? 2 : 1;
                continue;
            }

            var tag = MatchTag(text, index);
            if (tag is null)
            {
                index++;
                continue;
            }

            var length = tag.Marker.Length;
            if (!IsBetweenDigits(text, index, length))
            {
                var canOpen = CanOpen(text, index, length);
                var canClose = CanClose(text, index);
                if (canOpen || canClose)
                {
                    markers.Add(new InlineMarker(tag, index, canOpen, canClose));
                }
            }

            index += length;
        }

        return markers;
    }

    private IInlineTag? MatchTag(string text, int index)
    {
        foreach (var tag in _tags)
        {
            var marker = tag.Marker;
            if (index + marker.Length > text.Length)
            {
                continue;
            }

            if (text.AsSpan(index, marker.Length).SequenceEqual(marker))
            {
                return tag;
            }
        }

        return null;
    }

    private static bool IsBetweenDigits(string text, int start, int length)
    {
        var before = start - 1;
        var after = start + length;

        return before >= 0
            && after < text.Length
            && char.IsDigit(text[before])
            && char.IsDigit(text[after]);
    }

    private static bool CanOpen(string text, int start, int length)
    {
        var next = start + length;
        return next < text.Length && !char.IsWhiteSpace(text[next]);
    }

    private static bool CanClose(string text, int start)
    {
        return start > 0 && !char.IsWhiteSpace(text[start - 1]);
    }

    private static void PairMarkers(string text, List<InlineMarker> markers)
    {
        var stack = new Stack<InlineMarker>();
        var openCount = new Dictionary<IInlineTag, int>();

        foreach (var marker in markers)
        {
            if (marker.CanClose && TryClose(text, stack, openCount, marker))
            {
                continue;
            }

            if (!marker.CanOpen)
            {
                continue;
            }

            if (stack.Count > 0 && !stack.Peek().Tag.CanContain(marker.Tag))
            {
                continue;
            }

            stack.Push(marker);
            openCount[marker.Tag] = openCount.GetValueOrDefault(marker.Tag) + 1;
        }
    }

    private static bool TryClose(
        string text,
        Stack<InlineMarker> stack,
        Dictionary<IInlineTag, int> openCount,
        InlineMarker closer)
    {
        if (!openCount.TryGetValue(closer.Tag, out var count) || count == 0)
        {
            return false;
        }

        var top = stack.Peek();
        if (top.Tag == closer.Tag)
        {
            if (!IsValidPair(text, top, closer))
            {
                return false;
            }

            stack.Pop();
            openCount[closer.Tag] = count - 1;
            top.Pair = closer;
            closer.Pair = top;

            return true;
        }

        while (stack.Count > 0)
        {
            var opener = stack.Pop();
            openCount[opener.Tag]--;
            if (opener.Tag == closer.Tag)
            {
                break;
            }
        }

        return true;
    }

    private static bool IsValidPair(string text, InlineMarker open, InlineMarker closer)
    {
        if (closer.Start <= open.Start + open.Length)
        {
            return false;
        }

        var openInsideWord = open.Start > 0 && char.IsLetterOrDigit(text[open.Start - 1]);
        var closeEnd = closer.Start + closer.Length;
        var closeInsideWord = closeEnd < text.Length && char.IsLetterOrDigit(text[closeEnd]);

        if (!openInsideWord && !closeInsideWord)
        {
            return true;
        }

        for (var i = open.Start + open.Length; i < closer.Start; i++)
        {
            if (char.IsWhiteSpace(text[i]))
            {
                return false;
            }
        }

        return true;
    }

    private static string Render(string text, List<InlineMarker> markers)
    {
        var result = new StringBuilder(text.Length);
        var markerIndex = 0;
        var index = 0;
        while (index < text.Length)
        {
            if (text[index] == '\\')
            {
                if (index + 1 < text.Length && IsMarkupCharacter(text[index + 1]))
                {
                    result.Append(text[index + 1]);
                    index += 2;
                }
                else
                {
                    result.Append('\\');
                    index++;
                }

                continue;
            }

            if (markerIndex < markers.Count && markers[markerIndex].Start == index)
            {
                var marker = markers[markerIndex];
                markerIndex++;
                if (marker.Pair is not null)
                {
                    var isOpening = marker.Start < marker.Pair.Start;
                    result.Append(isOpening ? marker.Tag.RenderOpen() : marker.Tag.RenderClose());
                    index += marker.Length;
                    continue;
                }
            }

            result.Append(text[index]);
            index++;
        }

        return result.ToString();
    }

    private static bool IsMarkupCharacter(char character)
    {
        return character is '\\' or '_' or '#' or '-';
    }
}