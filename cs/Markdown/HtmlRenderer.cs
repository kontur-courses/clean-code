using System.Text;
using Markdown.Blocks;
using Markdown.Inline;

namespace Markdown;

public class HtmlRenderer
{
    
    public string RenderDocument(IEnumerable<Block> blocks)
    {
        // Рендер всего документа
    }

    
    private string RenderBlock(Block block)
    {
        // Рендер одного блока
    }

    
    public string RenderInlineTags(IEnumerable<InlineTag> tags)
    {
        // Рендер инлайн-тегов
    }
}