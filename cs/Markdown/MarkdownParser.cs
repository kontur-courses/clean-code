using Markdown.Abstractions;
using Markdown.MDElements;
using Markdown.MDNodes;

namespace Markdown.Parsing
{
    public class MarkdownParser
    {
        /// <summary>
        /// Главная точка входа. Возвращает плоский список узлов верхнего уровня.
        /// </summary>
        public List<MarkdownNode> Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new List<MarkdownNode>();

            var result = new List<MarkdownNode>();
            var lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine)) continue;

                if (trimmedLine.StartsWith("# "))
                {
                    // Путь А: Это блок (Заголовок)
                    var header = new HeaderNode
                    {
                        RawContent = trimmedLine.Substring(2)
                    };

                    // Парсим инлайны внутри заголовка и кладем в его Children
                    ParseInlineContent(header);
                    result.Add(header);
                }
                else
                {
                    // Путь Б: Обычный текст. 
                    // Поскольку ParagraphNode нет, сразу разбиваем строку на инлайн-узлы 
                    // и добавляем их в общий список.
                    var inlineNodes = ParseInlineElements(trimmedLine);
                    result.AddRange(inlineNodes);
                }
            }

            return result;
        }


        private void ParseInlineContent(BlockNode block)
        {
            block.Children = ParseInlineElements(block.RawContent);
        }


        private List<InlineNode> ParseInlineElements(string text)
        {
            // Пока заглушка: возвращаем весь текст как один TextNode
            return new List<InlineNode> { new TextNode(text) };
        }
    }
}