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
        void AddToken(Token token);

        /// <summary>
        /// Удаляет токен из дерева
        /// </summary>
        void RemoveToken(Token token);

    }
}