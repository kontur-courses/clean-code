using MarkupConverter.AST.Blocks;
using MarkupConverter.Parsers.BlockParsers;
using MarkupConverter.Parsers.InlineParsers;

namespace MarkupConverter.Parsers;

public class MarkdownParser : IParser
{
    private readonly List<IBlockParser> blockParsers;
    private readonly IInlineParser inlineParser;

    public MarkdownParser(List<IBlockParser> blockParsers, IInlineParser inlineParser)
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
            ProcessLine(line, parsingContext, doc);
        }

        while (parsingContext.HasOpenBlocks)
        {
            var block = parsingContext.Pop();
            doc.Blocks.Add(block.Close(inlineParser));
        }

        return doc;
    }

    private void ProcessLine(string line, ParsingContext parsingContext, Document doc)
    {
        if (IsBlank(line))
        {
            if (parsingContext.HasOpenBlocks)
            {
                var block = parsingContext.Pop();
                doc.Blocks.Add(block.Close(inlineParser));
            }

            return;
        }

        var selectedParser = blockParsers.FirstOrDefault(blockParser => blockParser.CanParse(line));

        if (selectedParser is null)
            return;

        var newBlock = selectedParser.Parse(line);

        if (!parsingContext.HasOpenBlocks)
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

    private static bool IsBlank(string line)
    {
        return string.IsNullOrWhiteSpace(line);
    }
}