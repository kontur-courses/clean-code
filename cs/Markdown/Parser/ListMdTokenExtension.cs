using Markdown.Domains;
using Markdown.Domains.Nodes;

namespace Markdown.Parser;

public static class ListMdTokenExtension
{
    /// <summary>
    ///     Проверка на то, что подчёркивания находятся в разных словах.
    /// </summary>
    /// <param name="tokens">Список токенов для анализа.</param>
    /// <param name="startIndex">Индекс первого токена в цепочке подчёркиваний.</param>
    /// <param name="closeIndex">Индекс последнего токена в цепочке подчёркиваний.</param>
    /// <param name="underscoreCount">Количество подчёркиваний в цепочке.</param>
    /// <returns>
    ///     Возвращает true, если подчёркивания находятся в разных словах, иначе false.
    ///     Метод может выбросить исключение, если входные данные некорректны:
    ///     - <see cref="ArgumentOutOfRangeException"/>: если closeIndex меньше startIndex.
    /// </returns>
    public static bool IsUnderscoreInDifferentWord(this List<MdToken> tokens, int startIndex, int closeIndex,
        int underscoreCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(closeIndex, startIndex);

        var nextTokenInd = closeIndex + underscoreCount;
        if (nextTokenInd > tokens.Count)
            return false;

        var range = tokens.GetRange(startIndex, closeIndex - startIndex);
        var hasSeparator = range.ContainsTokenType(TokenType.Space) || range.ContainsTokenType(TokenType.Escape);

        var prevIsWord = startIndex > 0 && tokens[startIndex - 1].Type == TokenType.Word;
        var nextIsWord = nextTokenInd < tokens.Count && tokens[nextTokenInd].Type == TokenType.Word;

        return hasSeparator && (prevIsWord || nextIsWord);
    }

    /// <summary>
    ///     Проверка на то, что подчёркивания находятся в разных словах.
    /// </summary>
    /// <param name="tokens">Список токенов для анализа.</param>
    /// <param name="startIndex">Индекс первого токена в цепочке подчёркиваний.</param>
    /// <param name="closeIndex">Индекс последнего токена в цепочке подчёркиваний.</param>
    /// <param name="underscoreCount">Количество подчёркиваний в цепочке.</param>
    /// <returns>
    ///     Возвращает true, если подчёркивания находятся в разных словах, иначе false.
    ///     Метод может выбросить исключение, если входные данные некорректны:
    ///     - <see cref="ArgumentOutOfRangeException"/>: если closeIndex меньше startIndex.
    /// </returns>
    public static bool IsUnderscoreInWordWithNumbers(this List<MdToken> tokens, int startIndex, int closeIndex,
        int underscoreCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(closeIndex, startIndex);

        var hasNumber = tokens.GetRange(startIndex + 1, closeIndex - startIndex).ContainsTokenType(TokenType.Number);
        var prevIsWord = startIndex > 0 && startIndex - underscoreCount >= 0
                                        && tokens[startIndex - underscoreCount].Type == TokenType.Word;

        return hasNumber && prevIsWord;
    }

    public static void AddSymbol(this List<Node> root, string symbol, int count)
    {
        for (var i = 0; i < count; i++)
            root.Add(new TextNode(symbol));
    }

    public static int GetTokensCountAfter(this List<MdToken> tokens, int startIndex, TokenType tokenType)
    {
        var tokenChainLength = 0;
        while (startIndex < tokens.Count && tokens[startIndex].Type == tokenType)
        {
            tokenChainLength++;
            startIndex++;
        }

        return tokenChainLength;
    }

    public static bool HaveNotPairedUnderscore(this List<MdToken> tokens)
    {
        return tokens.Count(token => token.Type == TokenType.Underscore) % 2 == 1;
    }

    private static bool ContainsTokenType(this List<MdToken> tokens, TokenType tokenType)
    {
        return tokens.Any(token => token.Type == tokenType);
    }
}