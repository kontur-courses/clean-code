
namespace Markdown.Handlers
{
    public class ItalicTagMdParser : IHandler
    {
        public static readonly string Tag = "_";
        
        /// <summary>
        /// Метод, который проверяет наличие курсивного текста. В случае успеха возвращает токен, иначе null
        /// </summary>
        /// <param name="position">Позиция начала предполагаемого тега</param>
        /// <param name="text">Текст, в котором проверяется наличие курсива</param>
        /// <returns>Token, если в тексте курсив; null - если другой тег</returns>
        public Token TryHandleTag(int position, string text)
        {
            throw new System.NotImplementedException();
        }
    }
}