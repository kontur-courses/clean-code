namespace Markdown;

public class MdOld
{
    public static readonly List<string> Tags = [ "__", "_", "#"]; // sorted desc by length
    
    private static int FindNextCharIndex(string markdown, char symbol, int startIndex)
    {
        // получение ближайшего справа индекса символа symbol
        for (int i = startIndex; i < markdown.Length; i++)
            if (markdown[i] == symbol)
                return i;
        return -1;
    }

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
        return index + 1 < markdown.Length && markdown[index] == '\\';
    }

    private static bool IsInsideWordWithDigits(string markdown, int startIndex)
    {
        // var wordLeftBound = 0;
        // var wordRightBound = markdown.Length - 1;
        var l = startIndex;
        var r = startIndex;
        while (l >= 0 && !char.IsWhiteSpace(markdown[l]))
            --l;

        while (r < markdown.Length && !char.IsWhiteSpace(markdown[r]))
            ++r;
        
        return Enumerable.Range(l + 1, r - l - 1)
            .Any(i => char.IsDigit(markdown[i]));
    }

    private static string ReplaceMarkerOnTag(string markdown, Marker mdMarker)
    {
        return "";
    }

    private static bool CanMarkerBeClosing(
        string markdown,
        Marker opening,
        Marker potentialClosing)
    {
        if (opening.Value != "_" && opening.Value != "__")
            return false;
        if (opening.Value != potentialClosing.Value)
            return false;
        if (potentialClosing.StartIndex <= opening.EndIndex)
            return false;

        var firstContentChar = markdown[opening.EndIndex];
        var lastContentChar = markdown[potentialClosing.StartIndex - 1];

        return !char.IsWhiteSpace(firstContentChar)
               && !char.IsWhiteSpace(lastContentChar);
    }
    
    // private static List<(Marker Opening, Marker Closing)> GetMarkerPairs(
    //     string markdown,
    //     List<Marker> markers)
    // {
    //     var result = new List<(Marker Opening, Marker Closing)>();
    //     var stack = new Stack<Marker>();
    //
    //     foreach (var marker in markers)
    //     {
    //         // Заголовки обрабатываются отдельно
    //         if (marker.Value != "_" && marker.Value != "__")
    //             continue;
    //
    //         if (stack.Count > 0)
    //         {
    //             var opening = stack.Peek();
    //
    //             if (CanMarkerBeClosing(markdown, opening, marker))
    //             {
    //                 stack.Pop();
    //                 result.Add((opening, marker));
    //                 continue;
    //             }
    //
    //             // Двойное выделение внутри одинарного не открываем
    //             if (opening.Value == "_" && marker.Value == "__")
    //                 continue;
    //
    //             // Вложенность одинаковых маркеров здесь не поддерживаем
    //             if (opening.Value == marker.Value)
    //                 continue;
    //         }
    //
    //         // Открывающий маркер должен иметь непробельный символ справа
    //         if (marker.EndIndex >= markdown.Length
    //             || char.IsWhiteSpace(markdown[marker.EndIndex]))
    //         {
    //             continue;
    //         }
    //
    //         stack.Push(marker);
    //     }
    //
    //     return result;
    // }

    private static (Marker marker, int index) FindMarkersPair(string markdown, List<Marker> markers, int markerIndex)
    {
        var marker = markers[markerIndex];
        for (var i = markerIndex + 1; i < markers.Count; i++)
        {
            var curMarker = markers[i];
            if (marker.Value != curMarker.Value) continue;
            if (marker.EndIndex >= curMarker.StartIndex) continue;
            return (curMarker, i);
        }

        return (null, -1);
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

    private static bool AreBothAdjacentCharactersMatch(
        string markdown,
        Marker marker,
        Predicate<char> predicate)
    {
        var leftMatch = marker.StartIndex > 0 && predicate(markdown[marker.StartIndex - 1]);
        var rightMatch = marker.EndIndex < markdown.Length && predicate(markdown[marker.EndIndex]);
        return leftMatch && rightMatch;
    }
    
    private static bool AreMarkersInsideDifferentWords(
        string markdown, Marker opening, Marker closing)
    {
        // если нет пробелов между тегами, значит в одном слове
        if (!markdown.Substring(
                opening.EndIndex,
                closing.StartIndex - opening.EndIndex).Any(char.IsWhiteSpace))
            return false;
        
        var isFirstInsideWord = AreBothAdjacentCharactersMatch(markdown, opening, char.IsLetter);
        var isSecondInsideWord = AreBothAdjacentCharactersMatch(markdown, closing, char.IsLetter);
        return isFirstInsideWord && isSecondInsideWord;
    }

    private static List<(Marker first, Marker second)> FindMarkerPairsShouldBeRendered(string markdown,
        List<Marker> markers)
    {
        var result = new List<(Marker first, Marker second)>();
        var ignoringIndexes = new HashSet<int>();
        for (int markerIndex = 0; markerIndex < markers.Count; markerIndex++)
        {
            if (ignoringIndexes.Contains(markerIndex)) continue;
            var ignoringPair = false;
            var curMarker = markers[markerIndex];
            (var pairMarker, var pairMarkerIndex) = FindMarkersPair(markdown, markers, markerIndex);
            if (pairMarker is null)
            {
                continue;
            }

            if (curMarker.Value == "_")
            {
                if (IsInsideWordWithDigits(markdown, curMarker.StartIndex)) 
                    ignoringPair = true;
                if (AreMarkersInsideDifferentWords(markdown, curMarker, pairMarker))
                    ignoringPair = true;
            }

            if (!ignoringPair) {
                result.Add((curMarker, pairMarker));
                ignoringIndexes.Add(pairMarkerIndex);
            }
        }

        return result;
    }
    
    public static string Render(string markdown)
    {
        var markers = FindAllMarkers(markdown);
        var result = "";
        var markerIndex = 0;
        var nearestMarkerTextIndex = markers[markerIndex].StartIndex;
        var needToBeRendered = Enumerable.Range(0, markers.Count).Select(x => false);
        for (var i = 0; i < markdown.Length; i++)
        {
            if (i < nearestMarkerTextIndex) result += markdown[i];
            else
            {
                // встали на индексе, где начинается маркер
                (var pairMarker, var pairMarkerIndex) = FindMarkersPair(markdown, markers, markerIndex);
                if (pairMarker is null)
                {
                }
                else
                {
                    
                }
                markerIndex++;
                nearestMarkerTextIndex = markers[markerIndex].StartIndex;
            }
        }
        for (var i = 0; i < markers.Count; i++)
        {
            (var pairMarker, var index) = FindMarkersPair(markdown, markers, i);
            
        } 
        return "";
    }
}