namespace Markdown.Lexer;

public static class EscapeSymbolProcessor
{
    private static bool IsEscapeSymbolEscaped(int prevPos, int currPos, string line, char escapeSymbol)
        => prevPos != -1 && line[currPos] == escapeSymbol && prevPos == currPos - 1;

    public static IReadOnlyDictionary<int, bool> GetEscapedEscapeSymbolsPositions(string line, char escapeSymbol)
    {
        var lastEscapeIndex = -1;
        var positionToEscapeMarker = new Dictionary<int, bool>();

        for (var i = 0; i < line.Length; i++)
        {
            if (IsEscapeSymbolEscaped(lastEscapeIndex, i, line, escapeSymbol))
            {
                positionToEscapeMarker.Add(lastEscapeIndex, true);
                positionToEscapeMarker.Add(i, false);
                lastEscapeIndex = -1;
                continue;
            }

            if (line[i] == escapeSymbol)
                lastEscapeIndex = i;
            else lastEscapeIndex = -1;
        }

        return positionToEscapeMarker;
    }
}