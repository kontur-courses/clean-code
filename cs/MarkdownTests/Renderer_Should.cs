using System;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class Renderer_Should
    {
        [TestCase("", TestName = "When string is empty")]
        [TestCase(null, TestName = "When string is null")]
        public void Render_ThrowException(string originalText)
        {
            var act = new Action(() => Renderer.Render(originalText));
            
            act.Should().Throw<ArgumentException>();
        }
        
        [TestCase("_aabb_", "<em>aabb</em>", TestName = "When simple italic tag")]
        [TestCase("a_b_", "a<em>b</em>", TestName = "When italic tag in word")]
        [TestCase("_d__a__f_", "<em>d__a__f</em>", TestName = "When bold inside italic selection")]
        public void Render_Italic(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);
            
            act.Should().Be(expectedText);
        }
        
        [TestCase("__aabb__", "<strong>aabb</strong>", TestName = "When simple bold")]
        [TestCase("a__b__", "a<strong>b</strong>", TestName = "When bold in word")]
        public void Render_Bold(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);
            
            act.Should().Be(expectedText);
        }
        
        [TestCase("____", TestName = "When empty inside bold")]
        [TestCase("__", TestName = "When empty inside italic")]
        [TestCase("__ aabb__", TestName = "When white space at the beginning of bold")]
        [TestCase("a__ b__", TestName = "When white space at the beginning of bold in word")]
        [TestCase("_ aabb_", TestName = "When white space at the beginning of italic")]
        [TestCase("a_ b_", TestName = "When white space at the beginning of italic in word")]
        [TestCase("__aabb __", TestName = "When white space at the end of bold")]
        [TestCase("a__b __", TestName = "When white space at the end of bold in word")]
        [TestCase("_aabb _", TestName = "When white space at the end of italic")]
        [TestCase("a_b _", TestName = "When white space at the end of italic in word")]
        [TestCase("__aabb", TestName = "When not bold end")]
        [TestCase("a__b", TestName = "When bold in word and not bold end")]
        [TestCase("_aabb", TestName = "When not italic end")]
        [TestCase("a_b", TestName = "When italic in word and not italic end")]
        [TestCase("__ab_bb__c_", TestName = "When italic and bold intersection")]
        [TestCase("aa_aaa c_c", TestName = "When italic in different words")]
        [TestCase("aa__aaa c__c", TestName = "When bold in different words")]
        [TestCase("_1_", TestName = "When only digits inside italic")]
        [TestCase("__12345__", TestName = "When only digits inside bold")]
        public void Render_SimpleText(string originalText)
        {
            var act = Renderer.Render(originalText);
            
            act.Should().Be(originalText);
        }
        
        [TestCase("__ab__ _c_", "<strong>ab</strong> <em>c</em>", TestName = "When not intersection")]
        [TestCase("__d_a_f__", "<strong>d<em>a</em>f</strong>", TestName = "When italic inside bold selection")]
        public void Render_ItalicAndBold(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);
            
            act.Should().Be(expectedText);
        }
    }
}