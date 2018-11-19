using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class MdTests
    {
        private Md md = new Md();

        [Test]
        public void Should_MakeEmTag_WhenOneWordSurroundedByUnderscores()
        {
            var text = "_курсив_";
            md.Render(text).Should().Be("<em>курсив</em>");
        }

        [Test]
        public void Should_MakeEmTag_WhenSomeWordsSurroundedByUnderscores()
        {
            var text = "_и здесь курсив_";
            md.Render(text).Should().Be("<em>и здесь курсив</em>");
        }

        [Test]
        public void Should_MakeStrongTag_WhenOneWordSurroundedByDoubleUnderscores()
        {
            var text = "__жирный__";
            md.Render(text).Should().Be("<strong>жирный</strong>");
        }

        [Test]
        public void Should_MakeStrongTag_WhenSomeWordsSurroundedByDoubleUnderscores()
        {
            var text = "__и здесь жирный__";
            md.Render(text).Should().Be("<strong>и здесь жирный</strong>");
        }

        [Test]
        public void Should_MakeEmTagInsideStrongTag_WhenDoubleUnderscoresHaveInnerUnderscores()
        {
            var text = "__просто жирный _жирный курсив___";
            md.Render(text).Should().Be("<strong>просто жирный <em>жирный курсив</em></strong>");
        }

        [Test]
        public void ShouldNot_MakeStrongTagInsideEmTag_WhenUnderscoresHaveInnerDoubleUnderscores()
        {
            var text = "_просто курсив __тоже просто курсив___";
            md.Render(text).Should().Be("<em>просто курсив __тоже просто курсив__</em>");
        }

        [Test]
        public void ShouldNot_MakeTags_WhenNonPairedUnderscores()
        {
            var text = "_непарное подчеркивание __и здесь";
            md.Render(text).Should().Be("_непарное подчеркивание __и здесь");
        }

        [Test]
        public void ShouldNot_MakeTags_WhenSpaceBeforeClosingUnderscore()
        {
            var text = "_перед закрывающим подчеркиванием _пробел";
            md.Render(text).Should().Be("_перед закрывающим подчеркиванием _пробел");
        }

        [Test]
        public void ShouldNot_MakeTags_WhenSpaceBeforeClosingDoubleUnderscore()
        {
            var text = "__перед закрывающим подчеркиванием __пробел";
            md.Render(text).Should().Be("__перед закрывающим подчеркиванием __пробел");
        }

        [Test]
        public void ShouldNot_MakeTags_WhenSpaceAfterOpeningUnderscore()
        {
            var text = "после_ открывающего подчеркивания_ пробел";
            md.Render(text).Should().Be("после_ открывающего подчеркивания_ пробел");
        }

        [Test]
        public void ShouldNot_MakeTags_WhenSpaceAfterOpeningDoubleUnderscore()
        {
            var text = "после__ открывающего подчеркивания__ пробел";
            md.Render(text).Should().Be("после__ открывающего подчеркивания__ пробел");
        }

        [Test]
        public void ShouldNot_MakeTags_WhenEscapedUnderscore()
        {
            var text = @"это \_не курсив_";
            md.Render(text).Should().Be(@"это не курсив_");
        }

        [Test]
        public void ShouldNot_RemoveBackslash_WhenItIsNotEscaping()
        {
            var text = @"это \обратные слэши\";
            md.Render(text).Should().Be(@"это \обратные слэши\");
        }
    }
}
