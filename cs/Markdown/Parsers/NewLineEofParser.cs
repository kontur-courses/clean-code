using Markdown.Parsers.Interfaces;

namespace Markdown.Parsers;

public class NewLineEofParser : ITokenParser
{
    private readonly List<ICompletableParse> completableParsers;

    public NewLineEofParser(List<ICompletableParse> completableParsers)
    {
        this.completableParsers = completableParsers;
    }

    public void Parse()
    {   
        foreach (var parser in completableParsers)
        {
            parser.Finish();
        }
    }
}