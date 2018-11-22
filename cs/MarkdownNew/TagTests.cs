using NUnit.Framework;


namespace MarkdownNew
{
    [TestFixture]
    class TagTests
    {
        private Tag tag;

        [SetUp]
        public void SetUp()
        {
            tag = new Tag("_", "<>");
        }

        [TestCase("_www<>", 0, ExpectedResult = true, TestName = "Open Tag Validator Should Be Valid In Start Of Line")]
        [TestCase(" _www<>", 1, ExpectedResult = true, TestName = "Open Tag Validator Should Be Valid")]
        [TestCase("1_1www<>", 1, ExpectedResult = false, TestName = "Open Tag Validator Should Not Be Between Digits")]
        [TestCase("w_1www<>", 1, ExpectedResult = true, TestName = "Open Tag Validator Should Be Valid Before Digit")]
        [TestCase(" _ www<>", 1, ExpectedResult = false, TestName = "Open Tag Validator Should Not Be Valid Before Space")]
        public bool testOpenTags(string someString, int position)
        {
            return tag.IsValidOpenTagFromPosition(someString, position);
        }

        [TestCase("_www<>", 4, ExpectedResult = true, TestName = "Close Tag Validator Should Be Valid In Start Of Line")]
        [TestCase(" _www<> ", 5, ExpectedResult = true, TestName = "Close Tag Validator Should Be Valid")]
        [TestCase("1_www1<>1", 6, ExpectedResult = false, TestName = "Close Tag Validator Should Not Be Between Digits")]
        [TestCase("w_1www<>1", 6, ExpectedResult = true, TestName = "Close Tag Validator Should Be Valid Before Digit")]
        [TestCase(" _ www <> ", 7, ExpectedResult = false, TestName = "Close Tag Validator Should Not Be Valid Before Space")]
        public bool testCloseTags(string someString, int position)
        {
            return tag.IsValidCloseTagFromPosition(someString, position);
        }
    }
}
