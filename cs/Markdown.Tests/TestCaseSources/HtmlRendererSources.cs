using System.Collections.Generic;
using Markdown.Extensions;
using Markdown.Primitives;
using NUnit.Framework;

namespace Markdown.Tests.TestCaseSources;

public static class HtmlRendererSources
{
    public static IEnumerable<TestCaseData> HtmlRendererSource()
    {
        yield return new TestCaseData(Tags.Text("Text").ToTagNode())
            .SetName("{m}TextTagNode").Returns("Text");
        
        yield return new TestCaseData(new TagNode(Tags.Italic(Tokens.Italic.Value), Tags.Text("Text").ToTagNode()))
            .SetName("{m}ItalicTagNode").Returns("<em>Text</em>");
        
        yield return new TestCaseData(new TagNode(Tags.Bold(Tokens.Bold.Value), Tags.Text("Text").ToTagNode()))
            .SetName("{m}BoldTagNode").Returns("<strong>Text</strong>");
        
        yield return new TestCaseData(new TagNode(Tags.Header1(Tokens.Header1.Value), Tags.Text("Text").ToTagNode()))
            .SetName("{m}HeaderTagNode").Returns("<h1>Text</h1>");
        
        yield return new TestCaseData(new TagNode(Tags.Header1(Tokens.Header1.Value), new[]
            {
                Tags.Text("A").ToTagNode(),
                new TagNode(Tags.Italic(Tokens.Italic.Value), Tags.Text("Italic").ToTagNode()),
                Tags.Text("C").ToTagNode()
            }))
            .SetName("{m}InnerItalicTagNode").Returns("<h1>A<em>Italic</em>C</h1>");
        
        yield return new TestCaseData(new TagNode(Tags.Link("link"), Tags.Text("Text").ToTagNode()))
            .SetName("{m}LinkTagNode").Returns("<a href=\"link\">Text</a>");
    }
}