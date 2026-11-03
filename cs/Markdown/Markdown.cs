using System.Globalization;

namespace Markdown
{
    /// <summary>
    /// Публичный класс, который будет работать с другими приватными классами, скрывая реализацию и предоставляя интерфейс
    /// </summary>
    public static class Md
    {
        public static string FromMarkdownToHtml(string  markdownText)
        {
             throw new NotImplementedException();
        }
    }

    internal class MarkdownParser
    {
        private readonly InlineParser inlineParser;

        public string Parse(string markdownText)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Класс проходит посимвольно по строке и определяет текст или особые конструкции (курсив, выделение)
    /// </summary>
    internal class InlineParser
    {
        //внутри будет цикл, в котором будут вызываться вспомогательные методы
        public string Parse(string markdownText)
        {
            throw new NotImplementedException();
        }


        //Вспомогательные методы

        /// <summary>
        /// Определяет, является ли символ экранированным
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool IsEscapeSequence(string text, int currentIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Определяет, можно ли считать символ "_" началом курсива (em)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool IsValidEmphasisStart(string text, int currentIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Определяет, можно ли считать символ "_" концом курсива (em)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool IsValidEmphasisEnd(string text, int currentIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Определяет, можно ли считать комбинацию символов "__" началом выделения (strong)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool IsValidStrongStart(string text, int currentIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Определяет, можно ли считать комбинацию символов "__" концом выделения (strong)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool IsValidStrongEnd(string text, int currentIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}
