using System;
using System.Collections.Generic;

namespace Markdown.DataStructures
{
    public interface ITokensTree
    {
        Token RootToken { get; }

        /// <summary>
        /// Добавляет токен в дерево
        /// </summary>
        void AddToken(Token parent);

        /// <summary>
        /// Удаляет токен из дерева
        /// </summary>
        void RemoveToken(Token token);

        /// <summary>
        /// Возвращает последовательность всех токенов в правильном порядке
        /// </summary>
        IEnumerable<Token> GetAllTokens(Token token);
    }
}