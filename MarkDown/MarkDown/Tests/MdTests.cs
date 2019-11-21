using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MarkDown.Tests
{
    [TestFixture]
    public class MdTests
    {
        [TestCase("<em>html</em>", "&lt;em&gt;html&lt;/em&gt;")]
        public void Render_ShouldRemoveHtmlTags(string line, string expectedLine)
        {
            Md.Render(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase(@"\\_a_", @"\<em>a</em>")]
        public void Render_RemovesExtraEscapeChars(string line, string expectedLine)
        {
            Md.Render(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("_a__a__a_", "<em>a__a__a</em>")]
        [TestCase("_Hello__World__A__a__A_", "<em>Hello__World__A__a__A</em>")]
        public void Render_RemovesNestedTags(string line, string expectedLine)
        {
            Md.Render(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("__a_a_a__", "<strong>a<em>a</em>a</strong>")]
        public void Render_NotRemovesAllowedNestedTags(string line, string expectedLine)
        {
            Md.Render(line).Should().BeEquivalentTo(expectedLine);
        }

        [TestCase("_hello__world_a__", "<em>hello__world</em>a__")]
        [TestCase("__hello_world__a_", "<strong>hello<em>world</em></strong><em>a</em>")]
        public void Render_HandlesCrossedTagsCorrectly(string line, string expectedLine)
        {
            Md.Render(line).Should().BeEquivalentTo(expectedLine);
        }
        
        [TestCase(100000, 3)]
        [TestCase(1151, 214)]
        public void Render_Performance_HandlesTagsInLinearTime(int length,  int @const)
        {
            for (var i = 0; i < 5; i++)
            {
                var line = RandomString(length);
                var secondLine = RandomString(length * @const);
                GC.Collect();
                var watch = System.Diagnostics.Stopwatch.StartNew();
                Md.Render(line);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                GC.Collect();
                watch = System.Diagnostics.Stopwatch.StartNew();
                Md.Render(secondLine);
                watch.Stop();
                (watch.ElapsedMilliseconds / elapsedMs).Should().BeLessOrEqualTo((long) (@const * 1.5));
            }
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "a_A__gds";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}