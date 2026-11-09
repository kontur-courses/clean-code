using Markdown.TagReaders;
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
            ])));
}
