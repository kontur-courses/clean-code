using MarkupConverter.AST.Blocks;
using MarkupConverter.Parsers.BlockParsers;
using MarkupConverter.Parsers.InlineParsers;

namespace MarkupConverter.Parsers;

public class Parser : IParser
{
    private readonly List<IBlockParser> blockParsers;
    private readonly IInlineParser inlineParser;

    public Parser(List<IBlockParser> blockParsers, IInlineParser inlineParser)
    {
        this.blockParsers = blockParsers;
        this.inlineParser = inlineParser;
    }

    public Document Parse(string text)
    {
        var doc = new Document([]);
        var parsingContext = new ParsingContext();

        var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        
        foreach (var line in lines)
        {
            if (IsBlank(line))
            {
                if (parsingContext.HasOpenBlocks)
                {
                    var block = parsingContext.Pop();
                    doc.Blocks.Add(block.Close(inlineParser));
                }
                continue;
            }

            var selectedParser = blockParsers.FirstOrDefault(blockParser => blockParser.CanParse(line));
            
            if(selectedParser is null)
                continue;
            
            var newBlock = selectedParser.Parse(line);
            
            if (parsingContext.CurrentBlock is null)
            {
                parsingContext.Push(newBlock);
            }
            else if (parsingContext.CurrentBlock.CanAccept(newBlock))
            {
                parsingContext.CurrentBlock.Accept(newBlock);
            }
            else
            {
                var oldBlock = parsingContext.Pop();
                doc.Blocks.Add(oldBlock.Close(inlineParser));
                parsingContext.Push(newBlock);
            }
        }
        
        if (parsingContext.CurrentBlock != null)
        {
            var block = parsingContext.Pop();
            doc.Blocks.Add(block.Close(inlineParser));
        }
            
        return doc;
    }
    
    private static bool IsBlank(string line) => string.IsNullOrWhiteSpace(line);
}