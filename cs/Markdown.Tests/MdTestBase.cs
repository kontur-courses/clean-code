using Markdown.TagReaders;
using Markdown.Writers;
using Markdown.Writers.TagHtmlInfos;
using NUnit.Framework;

namespace Markdown.Tests;

public abstract class MdTestBase
{
    protected Md md = null!;

    [SetUp]
    public void SetUp() => md = CreateMd();

    protected string Render(string input) => md.Render(input);

    private static Md CreateMd() => new(
        new Tokenizer(
            new TagReadersProvider(
            [
                new LinkTagReader(),
                new UnderscoresTagsReader(),
                new HeaderTagReader()
            ])),
        new HtmlWriter(
        [
            new LinkTagHtmlInfo(),
            new UnderscoreTagHtmlInfo(),
            new HeaderTagHtmlInfo(),
            new DoubleUnderscoreTagHtmlInfo()
        ]));
}
