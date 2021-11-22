using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class RenderTests
    {
        [TestCase("abc", "abc")]
        [TestCase("ab c", "ab c")]
        [TestCase("a b\nc", "a b<br>c")]
        public void Should_ParseSimpleString(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("_abc_", "<em>abc</em>")]
        [TestCase("__abc__", "<strong>abc</strong>")]
        [TestCase("_abc def_", "<em>abc def</em>")]
        [TestCase("__abc def__", "<strong>abc def</strong>")]
        public void Should_ParseSimplePairedTags(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("\\_abc\\_", "_abc_")]
        [TestCase("\\__abc__", "__abc__")]
        [TestCase("\\\\_abc_", "\\<em>abc</em>")]
        [TestCase("__abc\\def__", "<strong>abc\\def</strong>")]
        public void Should_ParseStringWithEscape(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("# abc", "<h1>abc</h1>")]
        [TestCase("# # abc", "<h1># abc</h1>")]
        [TestCase("abc# ", "abc# ")]
        [TestCase("abc# d#f", "abc# d#f")]
        [TestCase("# abc\n# def", "<h1>abc</h1><br><h1>def</h1>")]
        [TestCase("# ab __c _de_ fg__", "<h1>ab <strong>c <em>de</em> fg</strong></h1>")]
        public void Should_ParseStringWithHeader(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>")]
        [TestCase("_a__b__c_", "<em>a__b__c</em>")]
        [TestCase("__a_b__c_", "__a_b__c_")]
        public void Should_ParseStringWithNestedTags(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("as_ads_", "as<em>ads</em>")]
        [TestCase("as_ad_s", "as<em>ad</em>s")]
        [TestCase("_as_ads", "<em>as</em>ads")]
        [TestCase("1as_ads_", "1as_ads_")]
        [TestCase("as_ads ab_cd", "as_ads ab_cd")]
        public void Should_ParseStringWithEmbeddedPairedTags(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("_a _b _c", "_a _b _c")]
        [TestCase("a_ b_ c_", "a_ b_ c_")]
        [TestCase("__abc_", "__abc_")]
        [TestCase("_a\nbc_", "_a<br>bc_")]
        [TestCase("____", "____")]
        public void Should_ParseStringWithUnfinishedTags(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("- First\n- Second", "<ul><li>First</li><li>Second</li></ul>")]
        [TestCase("- - First\n- Second- ", "<ul><li>- First</li><li>Second- </li></ul>")]
        [TestCase("- _First_\n- # Second", "<ul><li><em>First</em></li><li># Second</li></ul>")]
        [TestCase("- _First\n- Second_", "<ul><li>_First</li><li>Second_</li></ul>")]
        [TestCase("-First\n- Second", "-First<br><ul><li>Second</li></ul>")]
        [TestCase("- First - Second", "<ul><li>First - Second</li></ul>")]
        public void Should_ParseStringWithUnorderedList(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
