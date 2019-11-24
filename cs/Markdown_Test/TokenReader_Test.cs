//using FluentAssertions;
//using Markdown;
//using NUnit.Framework;
//
//namespace Markdown_Test
//{
//    [TestFixture]
//    public class TokenReader_Test
//    {
//        private readonly TokenReader tokenReader = new TokenReader();
//        [Test]
//        public void ReadUntil_ShouldReadUntilStopChar()
//        {
//            var input = "_adad_ asd";
//            
//            var actual = tokenReader.ReadUntil(input, 0);
//            
//            actual.ConvertToHTMLTag().Should().Be("<em>adad</em>");
//        }
//        
//        [Test]
//        public void ReadUntil_ShouldReadUntilAnyStopChar()
//        {
//            var input = "__abc _defi_ jkl__";
//            var excepted = "<strong>abc <em>defi</em> jkl</strong>";
//            
//            var actual = TokenReader.ReadUntil(input, 0);
//                
//            actual.ConvertToHTMLTag().Should().Be(excepted);
//        }
//        
//        [Test]
//        public void ReadUntil_ShouldReturnString_IfTokenNotClosed()
//        {
//            var input = "__asd adad_ asS";
//            var excepted = "__asd adad_ asS";
//            
//            var actual = TokenReader.ReadUntil(input, 0);
//            
//            actual.ConvertToHTMLTag().Should().Be(excepted);
//        }
//    }
//}