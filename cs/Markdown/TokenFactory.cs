using Markdown.Interfaces;
using Markdown.Model;
using System;

namespace Markdown
{
    public static class TokenFactory
    {
        public static IToken CreateToken(string markDownTag)
        {
            return markDownTag switch
            {
                "__" => new BoldToken(),
                "_" => new ItalicToken(),
                "# " => new HeaderToken(),
                _ => throw new Exception("Токен не найден")
            };
        }
    }
}