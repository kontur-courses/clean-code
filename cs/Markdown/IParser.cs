using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    interface IParser
    {
        List<Token> ParseTextToTokens(string text); // Делит текст на список токенов
        string ConvertTokensToHtml(List<Token> tokens); // Собирает текст из списока токенов
    }
}
