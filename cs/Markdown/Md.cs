using Markdown.HtmlTools;
using Markdown.MarkdownParsers;
using Markdown.TagInfo;
using Markdown.TokenInfo;

namespace Markdown;

public class Md(IEnumerable<IMarkdownParser> parsers)
{
    public Md() : this([
        new SingleTagParser("# ", Tag.FirstLevelHeader), 
        new SingleTagParser("## ", Tag.SecondLevelHeader),
        new SingleTagParser("### ", Tag.ThirdLevelHeader),
        new SingleTagParser("#### ", Tag.FourthLevelHeader),
        new SingleTagParser("##### ", Tag.FifthLevelHeader), 
        new SingleTagParser("###### ", Tag.SixthLevelHeader),
        new PairedTagsParser("_", Tag.Italic), 
        new PairedTagsParser("__", Tag.Strong),
        new EscapedCharactersParser(),
        new LinkTagParser()
    ]) { }

    public string Render(string markdownText)
    {
        var tokens = new List<Token>();

        foreach (var paragraph in markdownText.Split('\n'))
        {
            foreach (var parser in parsers)
            {
                tokens.AddRange(parser.Parse(ReplaceSpecialSymbols(paragraph)));
            }
        }

        return HtmlTokenHandler.Handle(markdownText, tokens);
    }

    private static string ReplaceSpecialSymbols(string text)
    {
        return text.Replace("\t", " ");
    }
}