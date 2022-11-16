namespace Markdown.Handlers
{
    public class BoldTagMdParser : IHandler
    {
        public static readonly string Tag = "__";
        
        /// <summary>
        /// Метод, который проверяет наличие жирного текста. В случае успеха возвращает токен, иначе null
        /// </summary>
        /// <param name="position">Позиция начала предполагаемого тега</param>
        /// <param name="text">Текст, в котором проверяется наличие жирности</param>
        /// <returns>Token, если текст жирный; null - если другой тег</returns>
        public Token TryHandleTag(int position, string text)
        {
            throw new System.NotImplementedException();
        }
    }
}