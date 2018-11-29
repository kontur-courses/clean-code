using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class Md_Should
    {
        private Md parser;

        private string[] textLines = new[]
        {
            "*foo**bar**baz*\n\n", 
            "_some __bike_ is__\n\n",
            "*************",
            "  #    some ##\nwhat i want\n\n",
            "**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\r\n*Asclepias physocarpa*)**\n\n",
            "### head hello\n"
        };

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase(1000, 5000)]        
        [TestCase(3000, 15000)]
        [TestCase(5000, 25000)] 
        [TestCase(10000, 50000)] 
        [TestCase(20000, 100000)]  
        public void Render_InLinearTime(int first, int second)
        {
            var firstText = GetText(first, 0);
            var secondText = GetText(second, 0);

            double firstTime = GetRenderTime(firstText);
            double secondTime = GetRenderTime(secondText);

            (secondTime / firstTime).Should().BeLessOrEqualTo(6);
        }

        [TestCase(100, 500)]        
        [TestCase(300, 1500)]
        [TestCase(500, 2500)] 
        [TestCase(1000, 5000)] 
        public void Render_InLinearTime_WithNestedTags(int first, int second)
        {
            var firstText = GetText(first, 40);
            var secondText = GetText(second, 40);

            double firstTime = GetRenderTime(firstText);
            double secondTime = GetRenderTime(secondText);

            (secondTime / firstTime).Should().BeLessOrEqualTo(6);
        }
        
        

        [TestCase("  #    some ##\nwhat i want", ExpectedResult = "<h1>some</h1><p>what i want</p>")]
        [TestCase("_some __bike_ is__", ExpectedResult = "<p><em>some <em><em>bike</em> is</em></em></p>")]
        [TestCase("** some \r\n\r\n some**", ExpectedResult = "<p>** some \r\n\r\nsome**</p>")]
        [TestCase("_(__foo__)_", ExpectedResult = "<p><em>(<strong>foo</strong>)</em></p>")]
        [TestCase("*foo**bar**baz*", ExpectedResult = "<p><em>foo<strong>bar</strong>baz</em></p>")]
        [TestCase("**Gomphocarpus (*Gomphocarpus physocarpus*, syn.\r\n*Asclepias physocarpa*)**", 
            ExpectedResult = "<p><strong>Gomphocarpus (<em>Gomphocarpus physocarpus</em>, syn.\r\n<em>Asclepias physocarpa</em>)</strong></p>")]
        public string ComplexTest(string input)
        {
            return parser.Render(input);
        }

        private long GetRenderTime(string input)
        {
            var watch = new Stopwatch();
            watch.Start();
            parser.Render(input);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private string GetText(int numOfLines, int nestingLevel)
        {
            var result = new StringBuilder();

            for (int i = 0; i < numOfLines; i++)
            {
                result.Append(GetNestingTag(nestingLevel));
            }

            return result.ToString();
        }
        
        
        
        private string GetNestingTag(int maxLevel)
        {
            if (maxLevel == 0)
                return "";
            return $"_body {maxLevel} m  " + textLines[maxLevel%textLines.Length] + GetNestingTag(maxLevel - 1) + "boi_";
        }
    }
}

