using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Markdown.Tests;

[UseApprovalSubdirectory("Results")]
[TestFixture]
public class MdSpecApprovalTest
{
    [Test]
    [UseReporter(typeof(DiffReporter))]
    public void Render_MdSpec_ReturnCorrectHtml()
    {
        var markdown = File.ReadAllText(@"..\..\..\Tests\MdSpec.txt");

        var renderer = new Md();
        var html = renderer.Render(markdown);

        Approvals.Verify(html);
    }
}