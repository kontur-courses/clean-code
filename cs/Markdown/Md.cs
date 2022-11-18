using Markdown.Handlers;
using Markdown.Parsers;

namespace Markdown
{
    public class Md
    {
        /// <summary>
        /// Метод, который преобразует текст из markdown в html формат
        /// </summary>
        /// <param name="mdText">Текст в markdown формате</param>
        /// <returns>Текст в html формате</returns>
        public static string Render(string mdText)
        {
            var mdStringParser = new TokenTreeCreator(
                new BoldTagMdParser(),
                new ItalicTagMdParser(),
                new HeadingMdParser());
            var rootToken = mdStringParser.GetRootToken(mdText);
            return HtmlWriter.CreateHtmlFromTokens(rootToken, mdText);
        }
    }
}