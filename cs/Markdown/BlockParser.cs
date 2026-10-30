using static Markdown.Blocks;

namespace Markdown
{
    internal class BlockParser
    {
        // Разбивает входной текст на блоки
        public List<IBlock> ParseToBlocks(string markdown)
        {
            // Прочитать первую строку. Если начинается с "# ", 
            // создать HeadingBlock, иначе ParagraphBlock
            throw new NotImplementedException();
        }
    }
}
