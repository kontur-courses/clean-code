using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkDownTests
    {
        [Test]
        public void Render_SimpleString_ThisString()
        {
            Md.Render("abc").Should().Be("abc");
        }

        [Test]
        public void Render_SimpleItalic_ChangedOnHtml()
        {
            Md.Render("_abc_").Should().Be("<em>abc</em>");
        }

        [Test]
        public void Render_SimpleStrong_ChangedOnHtml()
        {
            Md.Render("__abc__").Should().Be("<strong>abc</strong>");
        }

        [Test]
        public void Render_StrongAndItalic_ChangedOnHtml()
        {
            Md.Render("_abc_ __cde__").Should().Be("<em>abc</em> <strong>cde</strong>");
        }

        [Test]
        public void Render_ItalicInStrong_ChangedOnHtml()
        {
            Md.Render("__a _abc_ e__").Should().Be("<strong>a <em>abc</em> e</strong>");
        }

        [Test]
        public void Render_StrongInItalic_OnlyItalicChangedOnHtml()
        {
            Md.Render("_a __abc__ e_").Should().Be("<em>a __abc__ e</em>");
        }

        [Test]
        public void Render_CrossOfTags_SameString()
        {
            Md.Render("__a _b c__ d_").Should().Be("__a _b c__ d_");
        }

        [Test]
        public void Render_ItalicInDifferentPartsOfWord_ChangedOnHtml()
        {
            Assert.Multiple(() =>
                {
                    Md.Render("_a_bc").Should().Be("<em>a</em>bc");
                    Md.Render("a_b_c").Should().Be("a<em>b</em>c");
                    Md.Render("ab_c_").Should().Be("ab<em>c</em>");
                }
            );
        }

        [Test]
        public void Render_EmptyBetweenTags_SameString()
        {
            Md.Render("____").Should().Be("____");
        }

        [Test]
        public void Render_InItalicPartsOfDifferentWords_SameString()
        {
            Md.Render("a_bc de_f").Should().Be("a_bc de_f");
        }

        [Test]
        public void Render_ItalicInWordWithDigits_SameString()
        {
            Md.Render("a_bc_d12").Should().Be("a_bc_d12");
        }

        [Test]
        public void Render_TagsInHeader_SameString()
        {
            Md.Render("# __a _abc_ e__").Should().Be("<h1><strong>a <em>abc</em> e</strong></h1>");
        }

        [Test]
        public void Render_ShieldedTag_SameString()
        {
            Md.Render("\\_abc_").Should().Be("_abc_");
        }

        [Test]
        public void Render_ShieldedShieldBeforeTag_ChangedOnHtml()
        {
            Md.Render("\\\\_abc_").Should().Be("\\<em>abc</em>");
        }

        [Test]
        [Description("Performance test")]
        public void Render()
        {
            var singleString = "__something _word_ more words__ _wordWithNumb_er312 __s__ ____ \\__\\_d_";
            var timer = new Stopwatch();
            timer.Start();
            Md.Render(singleString);
            timer.Stop();
            var singleTime = timer.ElapsedMilliseconds;
            var thousandString = string.Join(" ", Enumerable.Range(0, 2000).Select(x => singleString));
            timer.Reset();
            timer.Start();
            Md.Render(thousandString);
            timer.Stop();
            var thousandTime = timer.ElapsedMilliseconds;
            (thousandTime - singleTime * 2000).Should().BeLessThan(1000);
        }
    }
}