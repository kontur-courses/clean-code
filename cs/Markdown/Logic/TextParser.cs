using System;
using System.Collections;
using System.Collections.Generic;
using Markdown.DataStructures;

namespace Markdown.Logic
{
    public class TextParser : ITextParser
    {
        /// <summary>
        /// количество '_' которые нужно пропустить. Например, в случае пересечения тегов нужно будет пропустить 2
        /// </summary>
        private int countUndescoreToSkip;
        private Stack<Token> openedTokens;
        private TokensTree tokensTree;
        public List<int> IndexesEscapeCharacters { get; }

        public TextParser()
        {
            openedTokens = new Stack<Token>();
            IndexesEscapeCharacters = new List<int>();
            tokensTree = new TokensTree();
        }

        public IEnumerable<Token> Parse(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создает HeaderTag и кладет в стек и добавляет в дерево
        /// </summary>
        private Token HandleHashSymbol(int index)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Проверяет стоит ли после слэша спецсимвол, если да, то заносит индекс слэша в indexesEscapeCharacters
        /// </summary>
        private void HandleEscapeSymbol(int index)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Проверяет стек на пересечение тегов, выясняет тип тега (strong или em),
        /// создает тег, кладет в стек и добавляет в дерево
        /// </summary>
        private Token HandleUnderscore(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверяет пустой ли стек, если нет, удаляет из дерева токены оставшиеся в стеке.
        /// Если в стеке есть тег h1, то добавляет в HeaderTag индекс окончания тега.
        /// Очищает стек.
        /// </summary>
        private Token HandleParagraphSymbol(int index)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Проверяет ситуации когда цифры/пробелы после открывающего и перед закрывающим тегом
        /// </summary>
        private bool IsValidSymbolBeforeAndAfterTag(int index, ITag tag)
        {
            throw new NotImplementedException();
        }
    }
}