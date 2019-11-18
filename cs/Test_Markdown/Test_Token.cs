using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Test_Markdown
{
    [TestFixture]
    public class Test_Token
    {
        [Test]
        public void ConvertTokenToHTML_ShouldReturnSimpleString_OnSimplestInput()
        {
            var input = "asda";
            var token = new Token("");
            foreach (var cha in input)
            {
                token.AddChar(cha);
            }
            token.CloseToken(4, "");
            var actual = token.ConvertToHTMLTag();

            actual.Should().Be(input);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringInTag_OnInputWithTag()
        {
            var input = "asda";
            var excepted = "<em>asda</em>";
            var token = new Token("_");

            foreach (var cha in input)
            {
                token.AddChar(cha);
            }
            token.CloseToken(4, "em");
            var actual = token.ConvertToHTMLTag();

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInMiddleOfToken()
        {
            var input = "asda";
            var excepted = "<all>a<inside>sd</inside>a</all>";
            var token = new Token("");
            
            token.CreateInnerToken("");
            token.AddChar('a');
            var innerToken = token.CreateInnerToken("");
            token.AddChar('s');
            token.AddChar('d');
            innerToken.CloseToken(2, "inside");
            token.AddChar('a');
            token.CloseToken(3, "all");
            
            var actual = token.ConvertToHTMLTag();

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInRightCornerOfToken()
        {
            var input = "asda";
            var excepted = "<all>as<inside>da</inside></all>";
            var token = new Token("");
            
            token.CreateInnerToken("");
            token.AddChar('a');
            token.AddChar('s');
            var innerToken = token.CreateInnerToken("");
            token.AddChar('d');
            token.AddChar('a');
            innerToken.CloseToken(3, "inside");
            token.CloseToken(3, "all");

            var actual = token.ConvertToHTMLTag();

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInLeftCornerOfToken()
        {
            var input = "asda";
            var excepted = "<all><inside>as</inside>da</all>";
            var token = new Token("");
            var innerToken = token.CreateInnerToken("");
            token.AddChar('a');
            token.AddChar('s');
            innerToken.CloseToken(1, "inside");
            token.AddChar('d');
            token.AddChar('a');
            token.CloseToken(3, "all");
            
            var actual = token.ConvertToHTMLTag();

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenIsWholeToken()
        {
            var input = "asda";
            var excepted = "<all><inside>asda</inside></all>";
            var token = new Token("");
            var innerToken = token.CreateInnerToken("");
            token.AddChar('a');
            token.AddChar('s');
            token.AddChar('d');
            token.AddChar('a');
            innerToken.CloseToken(3, "inside");
            token.CloseToken(3, "all");
            
            var actual = token.ConvertToHTMLTag();

            actual.Should().Be(excepted);
        }

        [Test]
        public void ConvertToHTML_ShouldReturnStringWithPrefix_IfTokenNotClosed()
        {
            var input = "asda";
            var excepted = "__asda";
            var token = new Token("__");
            
            token.CreateInnerToken("");
            foreach (var cha in input)
            {
                token.AddChar(cha);
            }

            var actual = token.ConvertToHTMLTag();

            actual.Should().Be(excepted);
        }
    }
}