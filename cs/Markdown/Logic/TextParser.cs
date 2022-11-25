using System.Collections.Generic;
using System.Linq;
using Markdown.DataStructures;

namespace Markdown.Logic
{
    public class TextParser : ITextParser
    {
        /// <summary>
        /// количество EmTag которые нужно пропустить. Используются при обнаружении пересечения тегов
        /// </summary>
        private int emToSkip;

        /// <summary>
        /// количество StrongTag которые нужно пропустить. Используются при обнаружении пересечения тегов
        /// </summary>
        private int strongToSkip;

        private Stack<Token> openedTokens;
        private TokensTree tokensTree;
        private int symbolsToSkip;
        public List<int> IndexesEscapeCharacters { get; }

        public TextParser()
        {
            openedTokens = new Stack<Token>();
            IndexesEscapeCharacters = new List<int>();
            tokensTree = new TokensTree();
        }

        /// <summary>
        /// Последовательно обрабатывает все символы входной строки.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Корневой токен дерева</returns>
        public Token Parse(string text)
        {
            var currentIndex = 0;
            tokensTree.RootToken.EndIndex = text.Length - 1;
            while (currentIndex < text.Length)
            {
                if (symbolsToSkip > 0)
                    symbolsToSkip--;
                else
                    HandleSymbol(text, currentIndex);
                currentIndex++;
            }

            HandleParagraphSymbolOrEndText(currentIndex);
            return tokensTree.RootToken;
        }

        private void HandleSymbol(string text, int index)
        {
            switch (text[index])
            {
                case '#':
                    HandleHashSymbol(index);
                    break;
                case '/':
                    HandleEscapeSymbol(text, index);
                    break;
                case '\n':
                    HandleParagraphSymbolOrEndText(index);
                    break;
                case '_':
                    var startsInsideWord = index > 0 && IsLetter(text[index - 1]);
                    HandleUnderscore(text, index, startsInsideWord);
                    break;
                case ' ':
                    if (openedTokens.Count != 0)
                        openedTokens.Peek().ContainsSpaces = true;
                    break;
            }
        }

        /// <summary>
        /// Создает HeaderTag и кладет в стек и добавляет в дерево
        /// </summary>
        private void HandleHashSymbol(int index)
        {
            if (openedTokens.Count != 0)
                return;
            var token = new Token(new HeaderTag(), tokensTree.RootToken, index + 1);
            openedTokens.Push(token);
            tokensTree.AddToken(token);
        }

        /// <summary>
        /// Проверяет стоит ли после слэша спецсимвол, если да, то заносит индекс слэша в indexesEscapeCharacters
        /// </summary>
        private void HandleEscapeSymbol(string text, int index)
        {
            var indexToCheck = index + 1;
            var specialSymbols = new[] {'_', '/', '#'};
            if (indexToCheck >= text.Length)
                return;
            if (!specialSymbols.Contains(text[indexToCheck])) return;
            IndexesEscapeCharacters.Add(index);
            symbolsToSkip++;
        }

        /// <summary>
        /// Проверяет стек на пересечение тегов, выясняет тип тега (strong или em),
        /// создает тег, кладет в стек и добавляет в дерево
        /// </summary>
        private void HandleUnderscore(string text, int index, bool startsInsideWord)
        {
            var tagType = index + 1 < text.Length && text[index + 1] == '_' ? (ITag) new StrongTag() : new EmTag();
            if (tagType.MarkdownName == "__") symbolsToSkip++;

            if (SkipCurrentTag(tagType)) return;
            Token token;
            if (openedTokens.Count == 0)
                token = new Token(tagType, tokensTree.RootToken, index + tagType.MarkdownName.Length);
            else
            {
                if (openedTokens.Peek().Tag.MarkdownName == tagType.MarkdownName)
                {
                    CloseTag(openedTokens.Peek(), index, text);
                    return;
                }

                token = new Token(tagType, openedTokens.Peek(), index + tagType.MarkdownName.Length, startsInsideWord);
                if (TokenIntersectsTokens(token))
                {
                    HandleTokensIntersect(token);
                    return;
                }

                if (token.Tag.MarkdownName == "__" && token.Parent.Tag.MarkdownName == "_")
                    return;
            }

            openedTokens.Push(token);
            tokensTree.AddToken(token);
        }

        /// <summary>
        /// Принимает токен на котором высянилось пересечение тегов (1-й закрывающий),
        /// удаляет токены из стека и дерева
        /// </summary>
        private void HandleTokensIntersect(IToken token)
        {
            if (token.Tag.OpeningTag == "<em>")
                strongToSkip++;
            else
                emToSkip++;
            tokensTree.RemoveToken(openedTokens.Pop());
            tokensTree.RemoveToken(openedTokens.Pop());
        }

        /// <summary>
        /// Добавляет конечный индекс токену, снимает со стека,
        /// если есть ошибки, то удаляет из дерева
        /// </summary>
        private void CloseTag(Token currentToken, int index, string text)
        {
            currentToken.EndIndex = index - 1;
            var endTagIndex = index + currentToken.Tag.MarkdownName.Length;
            currentToken.EndsInsideWord = endTagIndex < text.Length && IsLetter(text[endTagIndex]);
            openedTokens.Pop();
            if (UnderscoreTokenHasMistakes(currentToken, text))
                tokensTree.RemoveToken(currentToken);
        }

        /// <summary>
        /// проверяет на пустую строку внутри, недопустимые симваолы до и после тегов
        /// </summary>
        private bool UnderscoreTokenHasMistakes(IToken token, string text)
        {
            var indexToCheckOpeningTag = token.StartIndex;
            var indexToCheckClosingTag = token.EndIndex;

            var unresolvedSymbols = new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' '};
            if (unresolvedSymbols.Contains(text[indexToCheckOpeningTag]) ||
                unresolvedSymbols.Contains(text[indexToCheckClosingTag]))
                return true;
            return token.StartIndex > token.EndIndex ||
                   (token.StartsInsideWord || token.EndsInsideWord) && token.ContainsSpaces;
        }

        /// <summary>
        /// Проверяет пустой ли стек, если нет, удаляет из дерева токены оставшиеся в стеке.
        /// Если в стеке есть тег h1, то добавляет в HeaderTag индекс окончания тега.
        /// Очищает стек.
        /// </summary>
        private void HandleParagraphSymbolOrEndText(int index)
        {
            while (openedTokens.Count > 0)
            {
                var token = openedTokens.Pop();
                if (token.Tag.MarkdownName == "#")
                    token.EndIndex = index - 1;
                else
                    tokensTree.RemoveToken(token);
            }
        }

        /// <summary>
        /// Проверяет нужно ли пропускать теги
        /// </summary>
        private bool SkipCurrentTag(ITag tag)
        {
            if (tag.OpeningTag == "<em>" && emToSkip > 0)
            {
                emToSkip--;
                return true;
            }

            if (strongToSkip == 0) return false;
            strongToSkip--;
            return true;
        }

        private bool IsLetter(char symbol)
        {
            var specSymbols = new[] {' ', '_', '#'};
            return !specSymbols.Contains(symbol);
        }

        private bool TokenIntersectsTokens(IToken token)
        {
            return openedTokens.Count >= 2 &&
                   openedTokens.Peek().Tag.OpeningTag != token.Tag.OpeningTag &&
                   openedTokens.Peek().Parent.Tag.OpeningTag == token.Tag.OpeningTag;
        }
    }
}