using System.Text;
using FluentAssertions;
using System.Diagnostics;

namespace Markdown.Core.Tests
{
    public class MdTests
    {
        private readonly Md _markdown = new Md();
        private readonly string[] _testText = 
        {
            "*qwe**asd**rty*\n",
            "_qqqq __zxc_ qq__\n\n",
            "*************",
            "  #    wasdwasd ##\nzxcbnm\n\n",
            "**parparpar (*qwertyqwerty*, ss.\r\n*bbb**\n\n",
            "### hello world\n"
        };

        [TestCase(100, 1000)]
        [TestCase(1000, 4000)]
        [TestCase(3000, 9000)]
        [TestCase(5000, 15000)]
        [TestCase(15000, 35000)]
        [TestCase(30000, 100000)]
        public void Render_IsLinearExecTime_ShouldBeTrue(int linesCountCase1, int linesCountCase2)
        {
            var firstText = GetText(linesCountCase1, 1);
            var secondText = GetText(linesCountCase2, 1);

            double case1Time = GetRenderTime(firstText);
            double case2Time = GetRenderTime(secondText);
            (case1Time / case2Time).Should().BeLessOrEqualTo(6);
        }

        [TestCase(100, 500)]
        [TestCase(300, 1500)]
        [TestCase(500, 2500)]
        [TestCase(1000, 5000)]
        public void Render_IsLinearExecTimeWithNested_ShouldBeTrue(int first, int second)
        {
            var firstText = GetText(first, 40);
            var secondText = GetText(second, 40);

            double firstTime = GetRenderTime(firstText);
            double secondTime = GetRenderTime(secondText);

            (secondTime / firstTime).Should().BeLessOrEqualTo(6);
        }

        private string GetText(int numOfLines, int nestingLevel)
        {
            var res = new StringBuilder();

            for (int i = 0; i < numOfLines; i++)
                res.Append(GetNestedTag(nestingLevel));
            
            return res.ToString();
        }

        private string GetNestedTag(int maxLevel)
        {
            return maxLevel == 0 ? 
                "" : 
                $"_qwe {maxLevel} qwe {_testText[maxLevel % _testText.Length] + GetNestedTag(maxLevel - 1)} qwe_";
        }

        private long GetRenderTime(string input)
        {
            var sw = new Stopwatch();

            sw.Start();
            _markdown.Render(input);
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }
    }
}