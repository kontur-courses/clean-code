//using System;
//using System.Collections.Generic;
//using FluentAssertions;
//using MarkDown.TagParsers;
//using NUnit.Framework;

//namespace MarkDown.Tests
//{
//    [TestFixture]
//    public class TagParsersTests
//    {
//        private readonly List<Tag> parsers = new List<Tag>()
//        {
//            new EmTag(),
//            new StrongTag()
//        };

//        [Test]
//        public void GetParsedLine_ThrowsArgumentException_OnNull()
//        {
//            foreach (var parser in parsers)
//            {
//                Action action = () => parser.GetParsedLineFrom(null);
//                action.Should().Throw<ArgumentException>();
//            }
//        }


//    }
//}