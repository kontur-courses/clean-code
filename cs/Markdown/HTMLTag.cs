namespace Markdown
{
    public class HTMLTag
    {
        public string OpenTag { get; private set; }
        public string CloseTag { get; private set; }

        /// <summary>
        /// Создает новый объект HTMLTag по его строковому представлению в md формате
        /// </summary>
        public HTMLTag CreateFormMd(string mdTag, IMdSpecification specification)
        {
            var converter = new TagConverter(specification);
            return new HTMLTag(converter.ToHTML(mdTag));
        }

        /// <summary>
        /// Создает новый объект HTMLTag по названию тэга
        /// </summary>
        public HTMLTag(string htmlTag)
        {
            OpenTag = $"<{htmlTag}>";
            CloseTag = $"</{htmlTag}>";
        }
    }
}
