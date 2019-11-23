using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Test
{
    public class TokenToHtmlConverter_Test
    {
        private readonly TokenToHtmlConverter htmlConverter = new TokenToHtmlConverter();
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnSimpleString_OnSimplestInput()
        {
            var input = "asda";
            var token = new Token("");
            foreach (var cha in input)
            {
                token.AddChar(cha);
            }
            token.CloseToken();
            var actual = htmlConverter.ConvertToHtml(token);

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
            token.CloseToken();
            var actual = htmlConverter.ConvertToHtml(token);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInMiddleOfToken()
        {
            var input = "asda";
            var excepted = "<em>a<strong>sd</strong>a</em>";
            var token = new Token("_");
            
            token.CreateInnerToken("");
            token.AddChar('a');
            var innerToken = token.CreateInnerToken("__");
            token.AddChar('s');
            token.AddChar('d');
            innerToken.CloseToken();
            token.AddChar('a');
            token.CloseToken();
            
            var actual = htmlConverter.ConvertToHtml(token);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInRightCornerOfToken()
        {
            var input = "asda";
            var excepted = "<em>as<strong>da</strong></em>";
            var token = new Token("_");
            
            token.CreateInnerToken("");
            token.AddChar('a');
            token.AddChar('s');
            var innerToken = token.CreateInnerToken("__");
            token.AddChar('d');
            token.AddChar('a');
            innerToken.CloseToken();
            token.CloseToken();

            var actual = htmlConverter.ConvertToHtml(token);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInLeftCornerOfToken()
        {
            var input = "asda";
            var excepted = "<em><strong>as</strong>da</em>";
            var token = new Token("_");
            var innerToken = token.CreateInnerToken("__");
            token.AddChar('a');
            token.AddChar('s');
            innerToken.CloseToken();
            token.AddChar('d');
            token.AddChar('a');
            token.CloseToken();
            
            var actual = htmlConverter.ConvertToHtml(token);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenIsWholeToken()
        {
            var input = "asda";
            var excepted = "<em><strong>asda</strong></em>";
            var token = new Token("_");
            var innerToken = token.CreateInnerToken("__");
            token.AddChar('a');
            token.AddChar('s');
            token.AddChar('d');
            token.AddChar('a');
            innerToken.CloseToken();
            token.CloseToken();
            
            var actual = htmlConverter.ConvertToHtml(token);

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

            var actual = htmlConverter.ConvertToHtml(token);

            actual.Should().Be(excepted);
        }
    }
}