using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Handlers;

namespace Markdown
{
    public class TokenTreeCreator
    {
        private readonly List<IHandler> handlers;

        public TokenTreeCreator(params IHandler[] handlers)
        {
            this.handlers = handlers.ToList();
        }

        /// <summary>
        /// Метод, который парсит текст и создаёт дерево токенов оформления
        /// </summary>
        /// <param name="text">Текст, который необходимо распарсить</param>
        /// <returns>Корневой токен полученного дерева</returns>
        public Token GetRootToken(string text)
        {
            var tagsStack = new Stack<string>();
            throw new NotImplementedException();
        }
    }
}