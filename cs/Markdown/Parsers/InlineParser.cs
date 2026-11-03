namespace Markdown
{
    /// <summary>
    /// Класс проходит посимвольно по строке и определяет текст или особые конструкции (курсив, выделение)
    /// </summary>
    internal class InlineParser : IInlineParser
    {
        private readonly TagRegistry registry;

        public InlineParser(TagRegistry registry)
        {
            this.registry = registry;
        }

        public List<Token> ParseToTokens(string line)
        {
            throw new NotImplementedException();
        }


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
