using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    public class BulletedListHandlerTest
    {
        private Md? sut;

        [SetUp]
        public void Init()
        {
            sut = new Md();
        }

        private static TestCaseData[] bulletedListTestCases =
        {
            new TestCaseData("* One\n* Two\n* Three").Returns("<ul>\n<li>One</li>\n<li>Two</li>\n<li>Three</li>\n</ul>").SetName("ListWithThreeElements"),
            new TestCaseData("* One\n* Two").Returns("<ul>\n<li>One</li>\n<li>Two</li>\n</ul>").SetName("ListWithTwoElements"),
            new TestCaseData("* One").Returns("<ul>\n<li>One</li>\n</ul>").SetName("ListWithOneElements"),
            new TestCaseData("- One").Returns("<ul>\n<li>One</li>\n</ul>").SetName("MinusAsMarkupSign"),
            new TestCaseData("+ One").Returns("<ul>\n<li>One</li>\n</ul>").SetName("PlusAsMarkupSign"),
            new TestCaseData("В тексте \n- One\n- Two").Returns("В тексте \n<ul>\n<li>One</li>\n<li>Two</li>\n</ul>").SetName("bulletedListTestInString"),
        };

        [TestCaseSource(nameof(bulletedListTestCases))]
        public string WhenBulletedListTagInsideBoldTag_ShouldReturnTwoTags(string input) =>
            sut.Render(input);
    }
}