using NUnit.Framework;

namespace Markdown
{
    public class MdTest
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
        [TestCase("_some text_ a _some text_", ExpectedResult = @"<em>some text<\em> a <em>some text<\em>")]
        public string Test_TextWithTagEm(string text) 
        {
            return Md.Render(text);
        }

        [TestCase("__some text__", ExpectedResult = @"<strong>some text<\strong>")]
        [TestCase("a __some text__ 89", ExpectedResult = @"a <strong>some text<\strong> 89")]
        [TestCase("__some text__ a __some text__", ExpectedResult = @"<strong>some text<\strong> a <strong>some text<\strong>")]
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
        [TestCase(@"\_ _some text_ \_", ExpectedResult = @"_ <em>some text<\em> _")]
        [TestCase(@"\te\xt", ExpectedResult = @"\te\xt")]
        [TestCase(@"\\ _some text_", ExpectedResult = @"\ <em>some text<\em>")]
        public string Test_TextWithShield(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"__twise _once_ twise__", ExpectedResult = @"<strong>twise <em>once<\em> twise<\strong>")]
        public string Test_TextTagEmInStrong(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"_once __twise__ once_", ExpectedResult = @"<em>once __twise__ once<\em>")]
        public string Test_TextTagStrongInEm(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"text_123_45", ExpectedResult = @"text_123_45")]
        [TestCase(@"text__123__45", ExpectedResult = @"text__123__45")]
        public string Test_TextWithDigits(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"_te_xt", ExpectedResult = @"<em>te<\em>xt")]
        [TestCase(@"t_ex_t", ExpectedResult = @"t<em>ex<\em>t")]
        [TestCase(@"te_xt_", ExpectedResult = @"te<em>xt<\em>")]
        public string Test_TagsEmInWord(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"__te__xt", ExpectedResult = @"<strong>te<\strong>xt")]
        [TestCase(@"t__ex__t", ExpectedResult = @"t<strong>ex<\strong>t")]
        [TestCase(@"te__xt__", ExpectedResult = @"te<strong>xt<\strong>")]
        public string Test_TagsStrongInWord(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"_some te_xt", ExpectedResult = @"_some te_xt")]
        [TestCase(@"so_me text_", ExpectedResult = @"so_me text_")]
        [TestCase(@"so_me te_xt", ExpectedResult = @"so_me te_xt")]
        public string Test_TagsEmInSomeWords(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"__some te__xt", ExpectedResult = @"__some te__xt")]
        [TestCase(@"so__me text__", ExpectedResult = @"so__me text__")]
        [TestCase(@"so__me te__xt", ExpectedResult = @"so__me te__xt")]
        public string Test_TagsStrongInSomeWords(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"__not pair _tag", ExpectedResult = @"__not pair _tag")]
        [TestCase(@"not __pair tag", ExpectedResult = @"not __pair tag")]
        [TestCase(@"not pair _tag", ExpectedResult = @"not pair _tag")]
        public string Test_NotPairTags(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"its_ tag_ not tag", ExpectedResult = @"its_ tag_ not tag")]
        [TestCase(@"its__ tag__ not tag", ExpectedResult = @"its__ tag__ not tag")]
        [TestCase(@"its_ tag__ not tag", ExpectedResult = @"its_ tag__ not tag")]
        [TestCase(@"its__ tag_ not tag", ExpectedResult = @"its__ tag_ not tag")]
        public string Test_TagStartAfterWhiteSpace(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"its _tags _not the_ end", ExpectedResult = @"its <em>tags _not the<\em> end")]
        [TestCase(@"its __tags __not the__ end", ExpectedResult = @"its <strong>tags __not the<\strong> end")]
        public string Test_TagFinishBeforeWhiteSpace(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"some text __", ExpectedResult = @"some text __")]
        [TestCase(@"some ____ text", ExpectedResult = @"some ____ text")]
        public string Test_TagEmptyInside(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"intersection __twice _and__ once_ tag", ExpectedResult = @"intersection __twice _and__ once_ tag")]
        [TestCase(@"intersection _once __and_ twice__ tag", ExpectedResult = @"intersection _once __and_ twice__ tag")]
        public string Test_TextWithInterectionTags(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"# Text __with _different_ simbols__", ExpectedResult = @"<h1>Text <strong>with <em>different<\em> simbols<\strong><\h1>")]
        public string Test_TextWithH1(string text)
        {
            return Md.Render(text);
        }

        [TestCase(@"blalba |mark1;mark2;mark3| bla", ExpectedResult = @"blalba <ul><li>mark1<\li><li>mark2<\li><li>mark3<\li><\ul> bla")]
        [TestCase(@"blalba |mark1 _once_;mark2 __twise__| bla", ExpectedResult = @"blalba <ul><li>mark1 <em>once<\em><\li><li>mark2 <strong>twise<\strong><\li><\ul> bla")]
        [TestCase(@"blalba |_once __twise__ once_;__twise _once_ twise__| bla", ExpectedResult = @"blalba <ul><li><em>once __twise__ once<\em><\li><li><strong>twise <em>once<\em> twise<\strong><\li><\ul> bla")]
        [TestCase(@"# Text |mark|", ExpectedResult = @"<h1>Text |mark|<\h1>")]
        [TestCase(@"# Text |_once_;__twise__|", ExpectedResult = @"<h1>Text |<em>once<\em>;<strong>twise<\strong>|<\h1>")]
        [TestCase(@"bla |mark1|mark2|mark3", ExpectedResult = @"bla <ul><li>mark1<\li><\ul>mark2|mark3")]
        [TestCase(@"bla \|mark1;|mark2\;mark3;mark4| bla", ExpectedResult = @"bla |mark1;<ul><li>mark2;mark3<\li><li>mark4<\li><\ul> bla")]
        [TestCase(@"bla |ma_rk;ma_tk;ma__rk;ma__rk|", ExpectedResult = @"bla <ul><li>ma_rk<\li><li>ma_tk<\li><li>ma__rk<\li><li>ma__rk<\li><\ul>")]
        [TestCase(@"_some | text_| ", ExpectedResult = @"_some | text_| ")]
        public string Test_TextWithUl(string text)
        {
            return Md.Render(text);
        }
    }
}