using System.Collections;
using System.Text;

namespace Markdown;

public static class Md
{
    public static readonly List<string> Tags = ["__", "_", "#"]; // sorted desc by length

    private static bool TryGetMarkdownTag(string markdown, int startIndex, out string tag)
    {
        // проверка является ли последовательность с startIndex мд тегом
        foreach (var mdTag in Tags)
            if (markdown.AsSpan(startIndex).StartsWith(mdTag))
            {
                tag = markdown.Substring(startIndex, mdTag.Length);
                return true;
            }

        tag = null;
        return false;
    }

    private static bool IsEscapeStarted(string markdown, int index)
    {
        return index + 1 < markdown.Length
               && markdown[index] == '\\'
               && markdown[index + 1] is '_' or '#' or '\\';
    }

    private static bool CanMarkerBeOpen(string markdown, Marker marker)
    {
        return (marker.Value == "_" || marker.Value == "__")
               && marker.EndIndex < markdown.Length
               && !char.IsWhiteSpace(markdown[marker.EndIndex]);
    }

    private static bool CanMarkerBeClose(string markdown, Marker marker)
    {
        return (marker.Value == "_" || marker.Value == "__")
               && marker.StartIndex > 0
               && !char.IsWhiteSpace(markdown[marker.StartIndex - 1]);
    }

    private static List<WordInfo> FindWords(string markdown)
    {
        var words = new List<WordInfo>();
        var i = 0;

        while (i < markdown.Length)
        {
            if (char.IsWhiteSpace(markdown[i]))
            {
                i++;
                continue;
            }

            var startIndex = i;
            var containsDigits = false;
            while (i < markdown.Length && !char.IsWhiteSpace(markdown[i]))
            {
                if (char.IsDigit(markdown[i]))
                    containsDigits = true;
                i++;
            }

            words.Add(new WordInfo(startIndex, i, containsDigits));
        }

        return words;
    }

    private static int[] FindWordIndexesForMarkers(
        List<WordInfo> words,
        List<Marker> markers)
    {
        var wordIndexes = new int[markers.Count];
        var wordIndex = 0;

        for (var markerIndex = 0; markerIndex < markers.Count; markerIndex++)
        {
            var marker = markers[markerIndex];

            while (wordIndex < words.Count && marker.StartIndex >= words[wordIndex].EndIndex)
                wordIndex++;

            if (wordIndex >= words.Count
                || marker.StartIndex < words[wordIndex].StartIndex
                || marker.EndIndex > words[wordIndex].EndIndex)
            {
                throw new ArgumentException("Маркер не принадлежит ни одному слову");
            }

            wordIndexes[markerIndex] = wordIndex;
        }

        return wordIndexes;
    }

    private static List<Marker> FindAllMarkers(string markdown)
    {
        var result = new List<Marker>();

        for (var i = 0; i < markdown.Length; i++)
        {
            if (IsEscapeStarted(markdown, i))
            {
                i++;
                continue;
            }

            if (TryGetMarkdownTag(markdown, i, out var currentTag))
            {
                result.Add(new Marker(currentTag, i, i + currentTag.Length));
                i += currentTag.Length - 1;
            }
        }

        return result;
    }

    private static bool AreMarkersInsideDifferentWords(
        Marker opening,
        WordInfo openingWord,
        Marker closing,
        WordInfo closingWord)
    {
        return openingWord.StartIndex != closingWord.StartIndex
               && opening.StartIndex > openingWord.StartIndex
               && opening.EndIndex < openingWord.EndIndex
               && closing.StartIndex > closingWord.StartIndex
               && closing.EndIndex < closingWord.EndIndex;
    }

    private static bool CanFormPair(Marker opening, Marker closing, WordInfo openingWord, WordInfo closingWord)
    {
        return opening.Value == closing.Value
               && closing.StartIndex > opening.EndIndex
               && !AreMarkersInsideDifferentWords(opening, openingWord, closing, closingWord);
    }
    
    private static List<(Marker Opening, Marker Closing)> FindMarkerPairs(
        string markdown,
        List<Marker> markers,
        List<WordInfo> words,
        int[] wordIndexes)
    {
        var markerIndexStack = new Stack<int>();
        var result = new List<(Marker Opening, Marker Closing)>();

        for (var i = 0; i < markers.Count; i++)
        {
            var marker = markers[i];
            var word = words[wordIndexes[i]];

            if (marker.Value == "_" && word.ContainsDigits)
                continue;

            if (markerIndexStack.Count == 0)
            {
                if (CanMarkerBeOpen(markdown, marker))
                    markerIndexStack.Push(i);
                continue;
            }

            var openingIndex = markerIndexStack.Peek();
            var opening = markers[openingIndex];
            var openingWord = words[wordIndexes[openingIndex]];

            if (CanMarkerBeClose(markdown, marker)
                && CanFormPair(opening, marker, openingWord, word))
            {
                markerIndexStack.Pop();
                result.Add((opening, marker));
                continue;
            }

            if (markerIndexStack.Count == 2)
            {
                var outerIndex = markerIndexStack.ElementAt(1);
                var outer = markers[outerIndex];
                var outerWord = words[wordIndexes[outerIndex]];

                var closesOuter =
                    marker.Value == outer.Value
                    && CanMarkerBeClose(markdown, marker)
                    && marker.StartIndex > outer.EndIndex
                    && !AreMarkersInsideDifferentWords(
                        outer, outerWord, marker, word);

                if (closesOuter)
                {
                    markerIndexStack.Clear();
                    continue;
                }
            }

            if (opening.Value == "__"
                && marker.Value == "_"
                && CanMarkerBeOpen(markdown, marker))
            {
                markerIndexStack.Push(i);
            }
        }

        return result;
    }

    public static string Render(string markdown)
    {
        var words = FindWords(markdown);
        var markers = FindAllMarkers(markdown);
        var wordIndexes = FindWordIndexesForMarkers(words, markers);
        var pairs = FindMarkerPairs(markdown, markers, words, wordIndexes);

        var replacements = new Dictionary<int, (Marker Marker, bool IsOpening)>();

        foreach (var pair in pairs)
        {
            replacements.Add(pair.Opening.StartIndex, (pair.Opening, true));
            replacements.Add(pair.Closing.StartIndex, (pair.Closing, false));
        }

        var result = new StringBuilder();
        var isHeading = markdown.StartsWith("# ", StringComparison.Ordinal);
        var i = isHeading ? 2 : 0;

        if (isHeading)
            result.Append("<h1>");

        while (i < markdown.Length)
        {
            if (IsEscapeStarted(markdown, i))
            {
                result.Append(markdown[i + 1]);
                i += 2;
                continue;
            }

            if (replacements.TryGetValue(i, out var replacement))
            {
                result.Append(HtmlUtils.ConvertMdTagToHtml(
                    replacement.Marker.Value,
                    replacement.IsOpening));

                i += replacement.Marker.EndIndex - replacement.Marker.StartIndex;
            }
            else
            {
                result.Append(markdown[i]);
                i++;
            }
        }

        if (isHeading)
            result.Append("</h1>");

        return result.ToString();
    }
}