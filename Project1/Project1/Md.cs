using System;
using System.Text;

namespace Programm
{
    public class Md
    {
        public static string Render(string markDownParagraph)
        {
            var paragraphInTokens = GetTokens(markDownParagraph);
            var UnderScoresHandledTokens = UnderScoresHandler.HandleUnderScores(paragraphInTokens);
            var htmlParagraph = GetHtmlTextFromTokens(UnderScoresHandledTokens);
            return htmlParagraph;
        }

        // Разбивка строки на токены: одинарные символы подчерка, двойные символы подчерка,
        // остальные символы(буквы, цифры, экранированные символы подчерка)
        private static Token[] GetTokens(string markDownParagraph)
        {
            return new Token[]{};
        }

        // По токенам преобразует в html строку
        private static string GetHtmlTextFromTokens(Token[] tokens)
        {
            var s = new StringBuilder();
            foreach (var token in tokens)
                s.Append(token.RealValue);
            return s.ToString();
        }
    }

    // Все, что делается с "землей" в этом классе
    public class UnderScoresHandler
    {
        public static Token[] HandleUnderScores(Token[] tokens)
        {
            var unaryUnderScoresHandledTokens = HandleUnaryUnderScores(tokens);
            var binaryUnderScoresHandledTokens = HandleBinaryUnderScores(unaryUnderScoresHandledTokens);
            return binaryUnderScoresHandledTokens;
        }

        //Обрабатывает одинарные нижнии подчеркивания.
        //Если внутри выделения были двойные подчеркивания, то они становятся просто подчеркиваниями
        private static Token[] HandleUnaryUnderScores(Token[] tokens)
        {
            return new Token[] { };
        }

        //Обрабатывает двойные нижнии подчеркивания.
        private static Token[] HandleBinaryUnderScores(Token[] tokens)
        {
            return new Token[] { };
        }
    }

    public class Token
    {
        public string RealValue;
        public string ValueInString;
    }
}