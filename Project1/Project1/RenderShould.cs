using NUnit.Framework;
using FluentAssertions;
using System;
using Programm;


namespace Tests
{
    [TestFixture]
    public class RenderShould
    {
        [Test]
        [TestCase("abc eqwe", ExpectedResult = "abc eqwe")]
        [TestCase("123", ExpectedResult = "123")]
        [TestCase("Hi, User! How are you?", ExpectedResult = "Hi, User! How are you?")]
        public string GiveTheSameString_IfParagraphWithoutUnderscores(string paragraph)
        {
            return Md.Render(paragraph);
        }



    }
}