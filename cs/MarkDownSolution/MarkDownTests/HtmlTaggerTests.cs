using FluentAssertions;
using MarkDown;
using NUnit.Framework;

namespace MarkDownTests
{
    public class HtmlTaggerTests
    {
        [Test]
        public void GetString_WithSimpleString_ShouldReturnTheString()
        {
            "Простой текст, ничего лишнего"
                .AfterProcessingShouldBe("Простой текст, ничего лишнего");
        }

        [Test]
        public void GetString_WithOneGround_ShouldReturnTheString()
        {
            "_земля в иллюминаторе"
                .AfterProcessingShouldBe("_земля в иллюминаторе");
        }

        [Test]
        public void GetString_WithItalic_ShouldReturnItalicTagged()
        {
            "_абоба_"
                .AfterProcessingShouldBe("<em>абоба</em>");
        }

        [Test]
        public void GetString_WithTwoItalics_ShouldReturnItalicTagged()
        {
            "_абоба_ _вторая абоба_"
                .AfterProcessingShouldBe("<em>абоба</em> <em>вторая абоба</em>");
        }

        [Test]
        public void GetString_WithBold_ShouldReturnBoldTagged()
        {
            "__жирный__"
                .AfterProcessingShouldBe("<strong>жирный</strong>");
        }

        [Test]
        public void GetString_WithTwoBolds_ShouldReturnBoldTagged()
        {
            "__жирный__ и __ещё жирнее__"
                .AfterProcessingShouldBe("<strong>жирный</strong> и <strong>ещё жирнее</strong>");
        }

        [Test]
        public void GetString_WithHeader_ShouldReturnHeaderTagged()
        {
            "# где"
                .AfterProcessingShouldBe("<h1>где</h1>");
        }

        [Test]
        public void GetString_ItalicAndBold_ShouldWorkCorrect()
        {
            "_i_ __b__ _i_ _i_ __b__"
                .AfterProcessingShouldBe("<em>i</em> <strong>b</strong> <em>i</em> <em>i</em> <strong>b</strong>");
        }
    }
}
