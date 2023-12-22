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
            new TestCaseData("* One\n* Two\n* Three\n").Returns("<ul><li>One</li><li>Two</li><li>Three</li></ul>").SetName("ListWithThreeElements"),
            new TestCaseData("* One\n* Two\n").Returns("<ul><li>One</li><li>Two</li></ul>").SetName("ListWithTwoElements"),
            new TestCaseData("* One\n").Returns("<ul><li>One</li></ul>").SetName("ListWithOneElements"),
            new TestCaseData("- One\n- Two\n").Returns("<ul><li>One</li><li>Two</li></ul>").SetName("MinusAsMarkupSign"),
            new TestCaseData("+ One\n").Returns("<ul><li>One</li></ul>").SetName("PlusAsMarkupSign"),
            new TestCaseData("* One *Two\n* Three\n").Returns("<ul><li>One *Two</li><li>Three</li></ul>").SetName("NoNewLineSymbolAndNoSpaceAfterMarkup"),
            new TestCaseData("В тексте \n- One\n- Two\n").Returns("В тексте <ul><li>One</li><li>Two</li></ul>").SetName("BulletedListTestInString"),
            new TestCaseData("В середине \n- One\n- Two\n текста").Returns("В середине <ul><li>One</li><li>Two</li></ul> текста").SetName("BulletedListTestInMiddleOfString"),
            new TestCaseData("* _One_\n* _Two_\n").Returns("<ul><li><em>One</em></li><li><em>Two</em></li></ul>").SetName("ListWithItalicTag"),
            new TestCaseData("* __One__\n* __Two__\n").Returns("<ul><li><strong>One</strong></li><li><strong>Two</strong></li></ul>").SetName("ListWithBoldTag"),
            new TestCaseData("* _One_\n* __Two__\n").Returns("<ul><li><em>One</em></li><li><strong>Two</strong></li></ul>").SetName("ListWithBoldAndItalicTag"),
            new TestCaseData("# После заголовка \n- One\n- Two\n").Returns("<h1> После заголовка</h1> <ul><li>One</li><li>Two</li></ul>").SetName("BulletedListAfterHeading"),
        };

        [TestCaseSource(nameof(bulletedListTestCases))]
        public string WhenBulletedListTagInsideBoldTag_ShouldReturnTwoTags(string input) =>
            sut.Render(input);
    }
}