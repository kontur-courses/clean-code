using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal.Filters;

namespace Markdown
{
    class MdTests
    {
        private Md md;

        [SetUp]
        public void CreateMd()
        {
            md = new Md();
        }

        [TestCase("hello _great_ world", "hello <em>great</em> world",
            TestName = "ShouldCreateEmTag_IfTextSurroundedByUnderscores")]

        [TestCase("_hello_ great world", "<em>hello</em> great world",
            TestName = "ShouldCreateEmTag_IfTextSurroundedByUnderscoresInStart")]

        [TestCase("hello great _world_", "hello great <em>world</em>",
            TestName = "ShouldCreateEmTag_IfTextSurroundedByUnderscoresInEnd")]

        [TestCase("hello ____ world", "hello ____ world",
            TestName = "ShouldNotCreateTag_IfEmptyStringBetweenDoubleUnderscores")]

        [TestCase("hello __great__ world", "hello <strong>great</strong> world",
            TestName = "ShouldCreateStrongTag_IfTextSurroundedByDoubleUnderscores")]

        [TestCase("__hello__ great world", "<strong>hello</strong> great world",
            TestName = "ShouldCreateStrongTag_IfTextSurroundedByDoubleUnderscoresInStart")]

        [TestCase("hello great __world__", "hello great <strong>world</strong>",
            TestName = "ShouldCreateStrongTag_IfTextSurroundedByDoubleUnderscoresInEnd")]

        [TestCase("hello __a_great_a__ world", "hello <strong>a<em>great</em>a</strong> world",
            TestName = "ShouldAddEmInsideStrongTag_IfUnderscoresInsideDoubleUnderscores")]

        [TestCase("hello _great_ __beautiful__ __interesting__ _wonderful_ _lovely_ world",
            "hello <em>great</em> <strong>beautiful</strong> <strong>interesting</strong> <em>wonderful</em> <em>lovely</em> world",
            TestName = "Render_ShouldAddCorrectTags_IfSeveralSurroundedParts")]

        [TestCase("hello _ great_ world", "hello _ great_ world",
            TestName = "Render_ShouldNotCreateTag_IfSurroundedTextHaveSpaceInStart")]

        [TestCase("hello _great _ world", "hello _great _ world",
            TestName = "Render_ShouldNotCreateTag_IfSurroundedTextHaveSpaceInEnd")]

        [TestCase(@"hello \_great\_ world", "hello _great_ world",
            TestName = "ShouldNotCreateTag_IfSlashBeforeUnderscores")]

        [TestCase(@"hello _gre\_at_ world", "hello <em>gre_at</em> world",
            TestName = "ShouldNotCloseTag_IfSlashBeforeUnderscores")]

        [TestCase(@"\hell\o great\\ \worl\d", @"hello great\ world",
            TestName = "Render_ShouldRemoveSlashes_IfSlashesBeforeSymbols")]

        public void Render(string mdText, string expectedHtml)
        {
            var actualHtml = md.Render(mdText);

            actualHtml.Should().Be(expectedHtml);
        }
    }
}
