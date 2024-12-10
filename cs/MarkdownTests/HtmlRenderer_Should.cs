using FluentAssertions;
using Markdown.Renderers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class HtmlRenderer_Should
{
    private readonly HtmlRenderer renderer = new ();

    [Test]
    public void Render_ShouldHandleTextWithoutTags()
    {
        var tokens = new List<BaseToken>
        {
            new (TokenType.Text, "Текст без тегов.")
        };

        var result = renderer.Render(tokens);

        result.Should().Be("Текст без тегов.");
    }

    [Test]
    public void Render_ShouldHandleEmphasisTags()
    {
        var tokens = new List<BaseToken>
        {
            new (TokenType.Text, "Это "),
            new (TokenType.Emphasis, "курсив") 
                { Children = new List<BaseToken>
                    { new (TokenType.Text, "курсив") } 
                },
            new (TokenType.Text, " текст")
        };

        var result = renderer.Render(tokens);

        result.Should().Be("Это <em>курсив</em> текст");
    }

    [Test]
    public void Render_ShoulHandleStrongTags()
    {
        var strongToken = new BaseToken(TokenType.Strong, string.Empty);
        strongToken.Children.Add(new BaseToken(TokenType.Text, "полужирный"));

        var tokens = new List<BaseToken>
        {
            new (TokenType.Text, "Это "),
            strongToken,
            new (TokenType.Text, " текст")
        };

        var result = renderer.Render(tokens);

        result.Should().Be("Это <strong>полужирный</strong> текст");
    }

    [Test]
    public void Render_ShouldHandleHeaderTags()
    {
        var headerToken = new BaseToken(TokenType.Header, string.Empty);
        headerToken.Children.Add(new BaseToken(TokenType.Text, "Заголовок"));
        var tokens = new List<BaseToken> { headerToken };

        var result = renderer.Render(tokens);

        result.Should().Be("<h1>Заголовок</h1>");
    }

    [Test]
    public void Render_ShouldHandleNestedTags()
    {
        var headToken = new BaseToken(TokenType.Header, string.Empty);
        var strongToken = new BaseToken(TokenType.Strong, string.Empty);
        var emphasisToken = new BaseToken(TokenType.Emphasis, string.Empty);

        emphasisToken.Children.Add(new BaseToken(TokenType.Text, "курсивом"));
        strongToken.Children.Add(new BaseToken(TokenType.Text, "полужирным текстом с "));
        strongToken.Children.Add(emphasisToken);
        headToken.Children.Add(new BaseToken(TokenType.Text, "заголовок с "));
        headToken.Children.Add(strongToken);
        var tokens = new List<BaseToken>
        {
            new BaseToken(TokenType.Text, "Это "),
            headToken
        };

        var result = renderer.Render(tokens);

        result.Should().Be("Это <h1>заголовок с <strong>полужирным " +
                           "текстом с <em>курсивом</em></strong></h1>");
    }

    [Test]
    public void Render_ShouldHandleEmptyTags()
    {
        var tokens = new List<BaseToken>
        {
            new (TokenType.Text, "Это "),
            new (TokenType.Emphasis, string.Empty),
            new (TokenType.Text, " текст")
        };

        var result = renderer.Render(tokens);

        result.Should().Be("Это <em></em> текст");
    }

    [Test]
    public void Render_ShouldHandleMultipleTags()
    {
        var tokens = new List<BaseToken>
        {
            new (TokenType.Text, "Это "),
            new (TokenType.Strong, "полужирный") { Children = { new BaseToken(TokenType.Text, "полужирный") } },
            new (TokenType.Text, " и "),
            new (TokenType.Emphasis, string.Empty) { Children = { new BaseToken(TokenType.Text, "курсив") } },
            new (TokenType.Text, " текст.")
        };

        var result = renderer.Render(tokens);

        result.Should().Be("Это <strong>полужирный</strong> и <em>курсив</em> текст.");
    }

    [Test]
    public void Render_ShouldHandleNestedTagsWithMultipleLevels()
    {
        var innerStrongToken = new BaseToken(TokenType.Strong, string.Empty);
        innerStrongToken.Children.Add(new BaseToken(TokenType.Text, "полужирный заголовок"));

        var innerEmphasisToken = new BaseToken(TokenType.Emphasis, string.Empty);
        innerEmphasisToken.Children.Add(new BaseToken(TokenType.Text, "полужирный курсив"));

        var outerHeaderToken = new BaseToken(TokenType.Header, string.Empty);
        outerHeaderToken.Children.Add(innerStrongToken);

        var outerStrongToken = new BaseToken(TokenType.Strong, string.Empty);
        outerStrongToken.Children.Add(new BaseToken(TokenType.Text, "и "));
        outerStrongToken.Children.Add(innerEmphasisToken);

        var tokens = new List<BaseToken>
        {
            outerHeaderToken,
            outerStrongToken,
        };

        var result = renderer.Render(tokens);

        result.Should().Be("<h1><strong>полужирный заголовок</strong></h1>" +
                           "<strong>и <em>полужирный курсив</em></strong>");
    }

    [Test]
    public void Render_ShouldHandleLinkTags()
    {
        var tokens = new List<BaseToken>
        {
            new LinkToken(new List<BaseToken>
            {
                new (TokenType.Text, "Ссылка на Google")
            }, "http://google.com")
        };

        var result = renderer.Render(tokens);

        result.Should().Be("<a href=\"http://google.com\">Ссылка на Google</a>");
    }
}