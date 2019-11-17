using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown.TagsDataBase;

namespace Markdown.Tests.TagsDataBase_Tests.MdAndHtmlTags_Tests
{
    class MdAndHtmlTags_GetHtmlTagByMdTag_Tests
    {
        [TestCase("open_")]
        [TestCase("close_")]
        [TestCase("open__")]
        [TestCase("close__")]
        public void ReturnedTag_ShouldBe_AmongHtmlTags(string mdTagId)
        {
            var mdTag = TagsDB.GetMdTagById(mdTagId);
            var htmlTag = MdAndHtmlTags.GetHtmlTagByMdTag(mdTag);

            Action act = () => TagsDB.GetHtmlTagById(htmlTag.Id);

            act.Should().NotThrow();
        }

        [TestCase("open_", ExpectedResult = "em")]
        [TestCase("close_", ExpectedResult = "/em")]
        [TestCase("open__", ExpectedResult = "strong")]
        [TestCase("close__", ExpectedResult = "/strong")]
        public string ReturnedTagId_ShouldBe_AssociationToMdTagId(string mdTagId)
        {
            var mdTag = TagsDB.GetMdTagById(mdTagId);

            var htmlTag = MdAndHtmlTags.GetHtmlTagByMdTag(mdTag);

            return htmlTag.Id;
        }

        [Test]
        public void IfNoAssociationToMdTag_ShouldThrowArgumentExcepton()
        {
            var mdTag = new Tag { Id = "123", Value = "_" };

            Action act = () => MdAndHtmlTags.GetHtmlTagByMdTag(mdTag);

            act.Should().Throw<ArgumentException>();
        }
    }
}