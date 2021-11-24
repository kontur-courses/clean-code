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

        [Test]
        public void GetString_ItalicInsideBold_ShouldWorkCorrect()
        {
            "__ну _и_ что__"
                .AfterProcessingShouldBe("<strong>ну <em>и</em> что</strong>");
        }

        [Test]
        public void GetString_ItalicsInsideBoldInsideHeader_ShouldWorkCorrect()
        {
            "# t __b _i_ _i_ b__"
                .AfterProcessingShouldBe("<h1>t <strong>b <em>i</em> <em>i</em> b</strong></h1>");
        }

        [Test]
        public void GetString_BoldInsideItalics_ShouldNotTag()
        {
            "Но не наоборот — внутри _одинарного __двойное__ не_ работает."
                .AfterProcessingShouldBe("Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.");
        }
        
        [Test]
        public void GetString_IncorrectSituation_ShouldNotTag()
        {
            "__Непарные_ символы в рамках одного абзаца не считаются выделением."
                .AfterProcessingShouldBe("__Непарные_ символы в рамках одного абзаца не считаются выделением.");
        }

        [Test]
        public void GetString_WithDigits_ShouldNotTag()
        {
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка."
                .AfterProcessingShouldBe("Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.");
        }

        [Test]
        public void GetString_ItalicWithPartOfWord_ShouldTag()
        {
            "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._"
                .AfterProcessingShouldBe("Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>");
        }
        
        [Test]
        public void GetString_ItalicSplittingWords_ShouldNotTag()
        {
            "В то же время выделение в ра_зных сл_овах не работает."
                .AfterProcessingShouldBe("В то же время выделение в ра_зных сл_овах не работает.");
        }

        [Test]
        public void GetString_IncorrectItalicStart_ShouldNotTag()
        {
            "эти_ подчерки_ не считаются выделением и остаются просто символами подчерка."
                .AfterProcessingShouldBe("эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.");
        }

        [Test]
        public void GetString_IncorrectItalicEnd_ShouldNotTag()
        {
            "эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка."
                .AfterProcessingShouldBe("эти <em>подчерки _не считаются</em> окончанием выделения и остаются просто символами подчерка.");
        }

        [Test]
        public void GetString_Intersection_ShouldNotTag()
        {
            "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением."
                .AfterProcessingShouldBe("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.");
        }

        [Test]
        public void GetString_OnEscapeGround_ShouldWork()
        {
            "\\_Вот это\\_, не должно выделиться тегом"
                .AfterProcessingShouldBe("_Вот это_, не должно выделиться тегом");
        }

        [Test]
        public void GetString_OnEscapeEscape_ShouldWork()
        {
            "\\\\Вот это\\\\, не должно выделиться тегом"
                .AfterProcessingShouldBe("\\Вот это\\, не должно выделиться тегом");
        }
        [Test]
        public void GetString_OnEscapeNothing_ShouldWork()
        {
            "\\Вот это\\, не должно выделиться тегом"
                .AfterProcessingShouldBe("\\Вот это\\, не должно выделиться тегом");
        }
    }
}
