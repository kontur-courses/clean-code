using System.IO;
using Markdown;
using Markdown.Renderers;
using NUnit.Framework;

namespace MarkdownTests
{
    class MdTests
    {
        [TestCase("", ExpectedResult = "", TestName = "Empty text")]
        [TestCase(null, ExpectedResult = null, TestName = "Null text")]
        public string MdRender_ConvertText_When(string markdownText) =>
            MdRender(markdownText);
        private string MdRender(string markdownText)
        {
            var md = new Md(new HtmlRenderer());
            return md.Render(markdownText);
        }

        [TestCase("t, _t t t_ t", ExpectedResult = "t, <em>t t t</em> t", TestName = "some text between tags")]
        [TestCase("в _нач_але", ExpectedResult = "в <em>нач</em>але", TestName = "one tag in word start and other in mid")]
        [TestCase("в сер_еди_не", ExpectedResult = "в сер<em>еди</em>не", TestName = "two tag into word")]
        [TestCase("в кон_це._", ExpectedResult = "в кон<em>це.</em>", TestName = "one tag into word and other in word end")]
        public string MdRender_ConvertItalicText_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase("witn numbers_12_3 text", ExpectedResult = "witn numbers_12_3 text", TestName = "inside text with numbers")]
        [TestCase("_text___", ExpectedResult = "_text___", TestName = "blink string")]
        [TestCase("text_ text_", ExpectedResult = "text_ text_", TestName = "after each tag space")]
        [TestCase("_text text _text", ExpectedResult = "_text text _text", TestName = "end tag before symbol")]
        [TestCase("text _text text", ExpectedResult = "text _text text", TestName = "exist only start tag")]
        [TestCase("text t_ext te_xt", ExpectedResult = "text t_ext te_xt", TestName = "tags into different words")]
        [TestCase("text t_ext ___te_xt", ExpectedResult = "text t_ext ___te_xt", TestName = "tags into different words with ___")]
        public string MdRender_NotConvertItalicText_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase("__text__", ExpectedResult = "<strong>text</strong>", TestName = "text between tags")]
        public string MdRender_ConvertBoldText_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase(@"\_text\_", ExpectedResult = @"_text_", TestName = "single comment")]
        [TestCase(@"\\\_text\\\_", ExpectedResult = @"\_text\_", TestName = "triple comment")]
        public string MdRender_ConvertCommentedTags_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase(@"\\_text\\_", ExpectedResult = @"\<em>text\</em>", TestName = "double comment")]
        [TestCase(@"\\\\_text\\\\_", ExpectedResult = @"\\<em>text\\</em>", TestName = "quad comment")]
        public string MdRender_NotConvertCommentedTags_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase("__t_t_t_t__", ExpectedResult = "<strong>t<em>t</em>t_t</strong>", TestName = "Italic inside bold into word")]
        [TestCase("__t_t___t__", ExpectedResult = "<strong>t_t___t</strong>", TestName = "Italic with blink end inside bold into word")]
        [TestCase(@"\__t_t___t__", ExpectedResult = @"__t_t___t__", TestName = "commented start tag")]
        [TestCase("t _t __t__ t_ t", ExpectedResult = @"t <em>t __t__ t</em> t", TestName = "bold inside italic")]
        [TestCase("t __t _t_ t__ t", ExpectedResult = @"t <strong>t <em>t</em> t</strong> t", TestName = "Italic inside bold")]
        [TestCase("t _t t __t t_ t__ t", ExpectedResult = "t _t t __t t_ t__ t", TestName = "one tag start before end other and end after")]
        [TestCase("__t _t t__ t t_ _t t__ t t_ t__ t", ExpectedResult = "__t _t t__ t t_ <em>t t__ t t</em> t__ t", TestName = "intersections that start with bold")]
        [TestCase("_t __t t_ t t__ _t __t t_ t t__ t", ExpectedResult = "_t __t t_ t t__ _t __t t_ t t__ t", TestName = "intersections that start with italic")]
        [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", ExpectedResult = @"Здесь сим\волы экранирования\ \должны остаться.\", TestName = "comment without tag")]
        public string MdRender_ConvertTextWithDifferentTags_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase("         # text", ExpectedResult = "         <h1>text</h1>", TestName = "header tag is not at the beginning of the text")]
        [TestCase("# text", ExpectedResult = "<h1>text</h1>", TestName = "header tag is at the beginning of the text")]
        [TestCase("# _text_", ExpectedResult = "<h1><em>text</em></h1>", TestName = "header tag before tag")]
        [TestCase("# t __t _t_ t__", ExpectedResult = "<h1>t <strong>t <em>t</em> t</strong></h1>", TestName = "header tag with other tags")]
        public string MdRender_ConvertHeaderText_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase("#_text_", ExpectedResult = "#<em>text</em>", TestName = "header tag without space before tag")]
        public string MdRender_NotConvertHeaderText_When(string markdownText) =>
            MdRender(markdownText);

        [TestCase("Markdown.md", TestName = "Markdown")]
        [TestCase("MarkdownSpec.md", TestName = "MarkdownSpec")]
        public void MdRender_MdFileToHtml_CreatesFor(string path)
        {
            var mdSpec = File.ReadAllText(path);
            var htmlFile = Path.ChangeExtension(path, ".html");
            File.Delete(htmlFile);
            using (var htmlMdSpec = File.CreateText(htmlFile))
            {
                htmlMdSpec.WriteLine(MdRender(mdSpec));
            }
            Assert.IsTrue(File.Exists(htmlFile));
        }
    }
}
