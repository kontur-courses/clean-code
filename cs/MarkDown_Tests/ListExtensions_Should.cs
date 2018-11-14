using System.Collections.Generic;
using FluentAssertions;
using MarkDown;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    public class ListExtensions_Should
    {
        public void ConditionalAdd_AddsItems_WhenConditionTrue()
        {
            var list = new List<int>();
            list.ConditionalAdd(true, 1);
            list.Should().Contain(1);
        }

        public void ConditionalAdd_NotAddsItems_WhenConditionFalse()
        {
            var list = new List<int>();
            list.ConditionalAdd(false, 1);
            list.Should().BeEmpty();
        }

        public void ConditionalAdd_AddsMultipleItems()
        {
            var list = new List<int>();
            list.ConditionalAdd(true, 1, 2, 3);
            list.Should().Contain(new []{1, 2, 3});
        }
    }
}
