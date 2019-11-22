using System.Collections.Generic;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        private Md processor;
        
        [SetUp]
        public void SetUp()
        {
            processor = new Md();
        }
        
        [TestCase("_a_", ExpectedResult = "<em>a</em>", TestName = "OneUnderlineToEm")]
        [TestCase("__word__", ExpectedResult = "<strong>word</strong>", TestName = "TwoUnderlinesToStrong")]
        [TestCase("a _a_ a __a__", ExpectedResult = "a <em>a</em> a <strong>a</strong>", TestName = "Mixed")]
        [TestCase("_a__a__a_", ExpectedResult = "<em>__a__</em>", TestName = "TwoUnderlinesInsideOneDontRender")]
        public string Processor_ChangesMdElementsToHtml(string mdString)
        {
            var a = processor.Render(mdString);
            return a;
        }
        
        [TestCase]
        public void Processor_FindsElements()
        {
            var a =  processor.FindElements("_a_");
            var expected = new List<ElementData>
            {
                new ElementData(ElementType.Em, 0, 1),
                new ElementData(ElementType.Em, 2, 3)
            };
            Assert.AreEqual(expected,a);
        }
    }
}