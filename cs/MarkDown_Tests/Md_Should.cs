using System.Collections.Generic;
using MarkDown;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    public class Md_Should
    {
        private Md markDown;
        [SetUp]
        public void SetUp()
        {
            markDown = new Md(new List<TagType>(){ new StrongTag(), new EmTag()});
        }

        [TestCase("a", ExpectedResult = "<p>a</p>", TestName = "when text have no tags")]
        [TestCase("_ a", ExpectedResult = "<p>_ a</p>", TestName = "when opening unpaired single underscore with space after")]
        [TestCase(" _a", ExpectedResult = "<p> _a</p>", TestName = "when opening unpaired single underscore with space before")]
        [TestCase("\\_a", ExpectedResult = "<p>_a</p>", TestName = "when opening screened unpaired single underscore")]
        [TestCase("a _", ExpectedResult = "<p>a _</p>", TestName = "when closing unpaired single underscore with space before")]
        [TestCase("a_ ", ExpectedResult = "<p>a_ </p>", TestName = "when closing unpaired single underscore with space after")]
        [TestCase("a\\_", ExpectedResult = "<p>a_</p>", TestName = "when closing screened unpaired single underscore")]
        [TestCase("_a_", ExpectedResult = "<p><em>a</em></p>", TestName = "when correctly paired single underscore")]
        [TestCase("\\_a\\_", ExpectedResult = "<p>_a_</p>", TestName = "when correctly paired screened single underscore")]
        [TestCase("__", ExpectedResult = "<p>__</p>", TestName = "when correctly paired without text")]
        [TestCase("_underscores _", ExpectedResult = "<p>_underscores _</p>", TestName = "when paired single underscore with whitespace before closing")]
        [TestCase("_ underscores_", ExpectedResult = "<p>_ underscores_</p>", TestName = "when paired single underscore with whitespace after opening")]
        [TestCase("_ _", ExpectedResult = "<p>_ _</p>", TestName = "when correctly paired with whitespace")]
        [TestCase("__a _a_ a__", ExpectedResult = "<p><strong>a <em>a</em> a</strong></p>", TestName = "when correctly paired double underscore with inner single underscore")]
        [TestCase("_a __aa__ a_", ExpectedResult = "<p><em>a __aa__ a</em></p>", TestName = "when correctly paired single underscore with inner double underscore")]
        [TestCase("aa __aaaa__ aa", ExpectedResult = "<p>aa <strong>aaaa</strong> aa</p>", TestName = "when correctly paired double underscore inside text")]
        [TestCase("aa __aaaa__ _aa_", ExpectedResult = "<p>aa <strong>aaaa</strong> <em>aa</em></p>", TestName = "when all types of tokens")]
        [TestCase("__unpaired _symbols", ExpectedResult = "<p>__unpaired _symbols</p>", TestName = "when unpaired symbols")]
        [TestCase("aa_aa_aa__a__", ExpectedResult = "<p>aa<em>aa</em>aa<strong>a</strong></p>", TestName = "when no whitespace before and after")]
        [TestCase("___a___", ExpectedResult = "<p>_<strong>a</strong>_</p>", TestName = "when three correctly paired underscore")]
        [TestCase("____a____", ExpectedResult = "<p>__<strong>a</strong>__</p>", TestName = "when four correctly paired underscore")]
        [TestCase(" __a\\\\a__ ", ExpectedResult = "<p> <strong>a\\a</strong> </p>", TestName = "when screening screen character")]
        [TestCase("__a\\a__", ExpectedResult = "<p><strong>aa</strong></p>", TestName = "when screen character")]
        [TestCase("number_12_3", ExpectedResult = "<p>number_12_3</p>", TestName = "when inside text with numbers only numbers in it")]
        [TestCase("te1xt_with_nu1mbers", ExpectedResult = "<p>te1xt_with_nu1mbers</p>", TestName = "when inside text with numbers and letters in it")]
        [TestCase("te1xt _w1th_ nu1mbers", ExpectedResult = "<p>te1xt <em>w1th</em> nu1mbers</p>", TestName = "when text with numbers and letters in it inside and out with whitespaces")]
        [TestCase("_te1xt with nu1mbers_", ExpectedResult = "<p><em>te1xt with nu1mbers</em></p>", TestName = "when outside text with numbers and letters in it")]
        [TestCase("_12_ 3", ExpectedResult = "<p><em>12</em> 3</p>", TestName = "when outside text with numbers and letters in it")]
        public string Render_ReturnCorrectlyParsedTag(string text) => markDown.Render(text);
        
    }
}
