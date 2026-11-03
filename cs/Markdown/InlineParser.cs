namespace Markdown
{
    /// <summary>
    /// Класс проходит посимвольно по строке и определяет текст или особые конструкции (курсив, выделение)
    /// </summary>
    internal class InlineParser : IInlineParser
    {
        private readonly Dictionary<string, TagInfo> tags;

        public InlineParser()
        {
            tags = new Dictionary<string, TagInfo>() // появится новый тег - можно написать его сюда
            {
                ["__"] = new TagInfo("__", "strong"),
                ["_"] = new TagInfo("_", "em")
            };
        }

        //внутри будет цикл, в котором будут вызываться вспомогательные методы
        public string Parse(string markdownText)
        {
            throw new NotImplementedException();
            //var result = new string builder
            // foreach ...
            //для вложенных конструкций рекурсивно вызывать Parse(SubString)
        }


        //Вспомогательные методы
        private bool IsEscaping(string line, int index)
        {
            throw new NotImplementedException();
        }

        private bool IsValidStartingTag(string line, int index, TagInfo tag)
        {
            throw new NotImplementedException();
        }

        private int FindEndingTag(string subline, int index, TagInfo tag)
        {
            throw new NotImplementedException();
        }

        private bool IsValidEndingTag(string line, int index, TagInfo tag)
        {
            throw new NotImplementedException(); 
        }
        
    }
}
