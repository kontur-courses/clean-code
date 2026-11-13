using System.Linq;
using System.Text;
using Markdown.Lexer;
using Markdown.Syntax;

namespace Markdown.Parsing;

/// <summary>
/// Разбирает токены разметки и строит дерево узлов
/// </summary>
public class InlineParser()
{
    private readonly InlineLexer inlineLexer = new();

    /// <summary>
    /// Разбирает inline-разметку и возвращает построенные узлы
    /// </summary>
    public IReadOnlyList<InlineNode> Parse(string text)
    {
        var tokens = inlineLexer.Tokenize(text);

        var pairsByOpeningTokenIndex = FindMarkerPairs(tokens);
        var validPairs = RemoveCrossedPairs(pairsByOpeningTokenIndex);

        return BuildNodes(tokens, 0, tokens.Count, validPairs, insideEmphasis: false);
    }

    /// <summary>
    /// Проверяет, подходит ли маркер на роль открывающего
    /// </summary>
    /// <param name="token">Токен для проверки</param>
    /// <param name="previousToken">Предыдущий токен</param>
    /// <param name="nextToken">Следующий токен</param>
    private static bool CanOpen(InlineToken token, InlineToken? previousToken, InlineToken? nextToken)
    {
        var nextCharacter = GetFirstCharacter(nextToken);
        if (token.Kind == InlineTokenKind.Text || nextCharacter is null || char.IsWhiteSpace(nextCharacter.Value))
        {
            return false;
        }

        return !IsBetweenDigits(previousToken, nextToken);
    }

    /// <summary>
    /// Проверяет, подходит ли маркер на роль закрывающего
    /// </summary>
    /// <param name="token">Токен для проверки</param> 
    /// <param name="previousToken">Предыдущий токен</param>
    /// <param name="nextToken">Следующий токен</param>
    private static bool CanClose(InlineToken token, InlineToken? previousToken, InlineToken? nextToken)
    {
        var previousCharacter = GetLastCharacter(previousToken);
        if (token.Kind == InlineTokenKind.Text || previousCharacter is null || char.IsWhiteSpace(previousCharacter.Value))
        {
            return false;
        }

        return !IsBetweenDigits(previousToken, nextToken);
    }

    /// <summary>
    /// Находит пары открывающих и закрывающих маркеров
    /// Для каждого вида маркеров хранит индексы открывающих маркеров в стеке
    /// Одновременно считает префиксные суммы пробелов и отбрасывает пары, которые не могут охватывать слова
    /// </summary>
    /// <param name="tokens">Список токенов inline-разметки</param>
    /// <returns>Пары, расположенные по индексам открывающих маркеров</returns>
    private static MarkerPair?[] FindMarkerPairs(IReadOnlyList<InlineToken> tokens)
    {
        var openMarkerIndicesByKind = new Dictionary<InlineTokenKind, Stack<int>>();
        var pairsByOpeningTokenIndex = new MarkerPair?[tokens.Count];
        var tokensWithWhitespacePrefix = new int[tokens.Count + 1];

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            var containsWhitespace = token.Kind == InlineTokenKind.Text && token.Value.Any(char.IsWhiteSpace);
            tokensWithWhitespacePrefix[i + 1] = tokensWithWhitespacePrefix[i] + (containsWhitespace ? 1 : 0);

            if (token.Kind == InlineTokenKind.Text)
            {
                continue;
            }

            if (!openMarkerIndicesByKind.TryGetValue(token.Kind, out var openMarkerIndices))
            {
                openMarkerIndices = new Stack<int>();
                openMarkerIndicesByKind.Add(token.Kind, openMarkerIndices);
            }

            var previousToken = i > 0 ? tokens[i - 1] : null;
            var nextToken = i + 1 < tokens.Count ? tokens[i + 1] : null;
            var canOpen = CanOpen(token, previousToken, nextToken);
            var canClose = CanClose(token, previousToken, nextToken);

            if (canClose && openMarkerIndices.Count > 0)
            {
                var pair = new MarkerPair(openMarkerIndices.Pop(), i, token.Kind);
                if (CanSpanWords(tokens, pair, tokensWithWhitespacePrefix))
                {
                    pairsByOpeningTokenIndex[pair.OpenTokenIndex] = pair;
                }
            }
            else if (canOpen && openMarkerIndices.Count == 0)
            {
                openMarkerIndices.Push(i);
            }
        }

        return pairsByOpeningTokenIndex;
    }

    /// <summary>
    /// Отбрасывает пары, которые пересекаются с другими парами
    /// </summary>
    /// <param name="pairsByOpeningTokenIndex">Пары маркеров, сгруппированные по индексу открывающего маркера</param>
    private static MarkerPair?[] RemoveCrossedPairs(MarkerPair?[] pairsByOpeningTokenIndex)
    {
        var activePairs = new Stack<MarkerPair>();

        for (var i = 0; i < pairsByOpeningTokenIndex.Length; i++)
        {
            if (pairsByOpeningTokenIndex[i] is not { } pair)
            {
                continue;
            }

            while (activePairs.Count > 0 && activePairs.Peek().CloseTokenIndex < pair.OpenTokenIndex)
            {
                activePairs.Pop();
            }

            var crossesAnOpenPair = false;
            while (activePairs.Count > 0 && activePairs.Peek().CloseTokenIndex < pair.CloseTokenIndex)
            {
                var crossedPair = activePairs.Pop();
                pairsByOpeningTokenIndex[crossedPair.OpenTokenIndex] = null;
                crossesAnOpenPair = true;
            }

            if (crossesAnOpenPair)
            {
                pairsByOpeningTokenIndex[i] = null;
            }
            else
            {
                activePairs.Push(pair);
            }
        }

        return pairsByOpeningTokenIndex;
    }

    /// <summary>
    /// Определяет, может ли пара маркеров охватывать текст с пробелами
    /// Если внутри пары есть пробелы, проверяет, что перед открывающим и после закрывающего маркера нет букв или цифр
    /// </summary>
    /// <param name="tokens">Список токенов inline-разметки</param>
    /// <param name="pair">Проверяемая пара маркеров</param>
    /// <param name="tokensWithWhitespacePrefix">Префиксные суммы количества токенов, содержащих пробелы</param>
    private static bool CanSpanWords(
        IReadOnlyList<InlineToken> tokens,
        MarkerPair pair,
        int[] tokensWithWhitespacePrefix)
    {
        var containsWhitespace = tokensWithWhitespacePrefix[pair.CloseTokenIndex] - tokensWithWhitespacePrefix[pair.OpenTokenIndex + 1] > 0;
        if (!containsWhitespace)
        {
            return true;
        }

        var previousToken = pair.OpenTokenIndex > 0 ? tokens[pair.OpenTokenIndex - 1] : null;
        var nextToken = pair.CloseTokenIndex + 1 < tokens.Count ? tokens[pair.CloseTokenIndex + 1] : null;
        return !IsWordCharacter(GetLastCharacter(previousToken)) && !IsWordCharacter(GetFirstCharacter(nextToken));
    }

    /// <summary>
    /// Рекурсивно строит узлы inline-разметки из диапазона токенов
    /// </summary>
    /// <param name="tokens">Токены inline-разметки</param>
    /// <param name="startTokenIndex">Начальный индекс диапазона включительно</param>
    /// <param name="endTokenIndexExclusive">Конечный индекс диапазона не включительно</param>
    /// <param name="pairsByOpeningTokenIndex">Пары маркеров, сгруппированные по индексу открывающего маркера</param>
    /// <param name="insideEmphasis">Показывает, находится ли диапазон внутри Emphasis, чтобы исключить обработку Strong внутри Emphasis</param>
    private static IReadOnlyList<InlineNode> BuildNodes(
        IReadOnlyList<InlineToken> tokens,
        int startTokenIndex,
        int endTokenIndexExclusive,
        IReadOnlyList<MarkerPair?> pairsByOpeningTokenIndex,
        bool insideEmphasis)
    {
        var nodes = new List<InlineNode>();
        var textBuffer = new StringBuilder();

        for (var i = startTokenIndex; i < endTokenIndexExclusive; i++)
        {
            var pair = pairsByOpeningTokenIndex[i];

            if (pair != null && pair.CloseTokenIndex < endTokenIndexExclusive)
            {
                if (insideEmphasis && pair.Kind is InlineTokenKind.StrongMarker or InlineTokenKind.StrongEmphasisMarker)
                {
                    AppendLiteralTokens(tokens, i, pair.CloseTokenIndex + 1, textBuffer);
                    i = pair.CloseTokenIndex;
                    continue;
                }

                var childrenAreInsideEmphasis = insideEmphasis || pair.Kind is InlineTokenKind.EmphasisMarker or InlineTokenKind.StrongEmphasisMarker;
                var children = BuildNodes(
                    tokens,
                    i + 1,
                    pair.CloseTokenIndex,
                    pairsByOpeningTokenIndex,
                    childrenAreInsideEmphasis);

                FlushText(nodes, textBuffer);
                switch (pair.Kind)
                {
                    case InlineTokenKind.EmphasisMarker:
                        nodes.Add(new EmphasisNode(children));
                        break;
                    case InlineTokenKind.StrongMarker:
                        nodes.Add(new StrongNode(children));
                        break;
                    case InlineTokenKind.StrongEmphasisMarker:
                        nodes.Add(new StrongNode([new EmphasisNode(children)]));
                        break;
                }

                i = pair.CloseTokenIndex;
                continue;
            }

            textBuffer.Append(tokens[i].Value);
        }

        FlushText(nodes, textBuffer);
        return nodes;
    }

    /// <summary>
    /// Добавляет значения токенов указанного диапазона в буфер как обычный текст
    /// </summary>
    /// <param name="tokens">Список токенов inline-разметки</param>
    /// <param name="startTokenIndex">Начальный индекс диапазона включительно</param>
    /// <param name="endTokenIndexExclusive">Конечный индекс диапазона не включительно</param>
    /// <param name="textBuffer">Буфер, в который добавляется текст</param>
    private static void AppendLiteralTokens(
        IReadOnlyList<InlineToken> tokens,
        int startTokenIndex,
        int endTokenIndexExclusive,
        StringBuilder textBuffer)
    {
        for (var i = startTokenIndex; i < endTokenIndexExclusive; i++)
        {
            textBuffer.Append(tokens[i].Value);
        }
    }

    /// <summary>
    /// Создаёт текстовый узел из буфера и очищает буфер
    /// </summary>
    /// <param name="nodes">Список узлов, куда добавляется текстовый узел</param>
    /// <param name="textBuffer">Буфер с накопленным текстом</param>
    private static void FlushText(List<InlineNode> nodes, StringBuilder textBuffer)
    {
        if (textBuffer.Length == 0)
        {
            return;
        }

        nodes.Add(new TextNode(textBuffer.ToString()));
        textBuffer.Clear();
    }

    private static char? GetFirstCharacter(InlineToken? token) =>
        token is { Kind: InlineTokenKind.Text, Value.Length: > 0 } ? token.Value[0] : null;

    private static char? GetLastCharacter(InlineToken? token) =>
        token is { Kind: InlineTokenKind.Text, Value.Length: > 0 } ? token.Value[^1] : null;

    private static bool IsBetweenDigits(InlineToken? previousToken, InlineToken? nextToken)
    {
        var previousCharacter = GetLastCharacter(previousToken);
        var nextCharacter = GetFirstCharacter(nextToken);

        return previousCharacter is not null && nextCharacter is not null && char.IsDigit(previousCharacter.Value) && char.IsDigit(nextCharacter.Value);
    }

    private static bool IsWordCharacter(char? character) => 
        character is not null && char.IsLetterOrDigit(character.Value);

    private sealed record MarkerPair(int OpenTokenIndex, int CloseTokenIndex, InlineTokenKind Kind);
}
