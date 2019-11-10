using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public string Render(string rawText)
        {
            /*
             * сначала разбиваем rawText на токены c помощью TokenReader'а,
             * затем каждый токен вставляем в финальную строку, добавив тэги
             * и удалив специальные символы, если это нужно, в зависимости 
             * от вложенности тега
             */
            var tokenDescription = new List<TokenDescription>();
            List<Token> tokens = new TokenReader(rawText, tokenDescription).TokenizeText();

            throw new NotImplementedException();
        }

        public string TokensToRenderedString(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }
    }
}