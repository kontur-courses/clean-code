using NUnit.Framework;

namespace Markdown
{
    public class Tests
    {
        [TestCase("", ExpectedResult = "")]
        [TestCase(" ", ExpectedResult = " ")]
        [TestCase("a", ExpectedResult = "a")]
        [TestCase("some text", ExpectedResult = "some text")]
        public string Test_TextWithoutTags(string text)
        {
            return Md.Render(text);
        }

        [TestCase("_some text_", ExpectedResult = @"<em>some text<\em>")]
        [TestCase("a _some text_ 89", ExpectedResult = @"a <em>some text<\em> 89")]
        [TestCase("_some text_ a _some text 2_", ExpectedResult = @"<em>some text<\em> a <em>some text 2<\em>")]
        public string Test_TextWithTagEm(string text) 
        {
            return Md.Render(text);
        }

        [TestCase("__some text__", ExpectedResult = @"<strong>some text<\strong>")]
        [TestCase("a __some text__ 89", ExpectedResult = @"a <strong>some text<\strong> 89")]
        [TestCase("__some text__ a __some text 2__", ExpectedResult = @"<strong>some text<\strong> a <strong>some text 2<\strong>")]
        public string Test_TextWithTagStrong(string text)
        {
            return Md.Render(text);
        }

        [TestCase("__some text__ a _some text_", ExpectedResult = @"<strong>some text<\strong> a <em>some text<\em>")]
        public string Test_TextWithSimpleCombineTagsStrongAndEm(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"\_some text\_", ExpectedResult = @"_some text_")]
        [TestCase(@"\__some text_\_", ExpectedResult = @"_<em>some text<\em>_")]
        [TestCase(@"\te\xt", ExpectedResult = @"\te\xt")]
        [TestCase(@"\\_some text_", ExpectedResult = @"\<em>some text<\em>")]
        public string Test_TextWithShield(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"__twise _once_ twise__", ExpectedResult = @"<strong>twise <em>once<\em> twise<\strong>")]
        [TestCase(@"___some text___", ExpectedResult = @"<strong><em>some text<\em><\strong>")]
        public string Test_TextTagEmInStrong(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"_once __twise__ once_", ExpectedResult = @"<em>once <\em><em>twise<\em><em> once<\em>")]
        public string Test_TextTagStrongInEm(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"text_123_45", ExpectedResult = @"text_123_45")]
        public string Test_TextWithDigits(string text)
        {
            return Md.Render(text);
        }
    }
}