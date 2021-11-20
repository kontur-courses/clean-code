using System.Collections.Generic;
using System.Linq;
using AhoCorasick;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    public class TokenParser : ITokenParser
    {
        // private List<Token> tokens = new();
        private readonly Trie<Token> trie = new();

        internal TokenParser()
        {
        }

        public void SetTokens(List<Token> tokensToSearch)
        {
            // tokens = tokensToSearch;
            
            foreach (var token in tokensToSearch)
                trie.Add(token.ToString(), token);
            trie.Build();
        }

        public (IEnumerable<ITokenSegment>, IEnumerable<ITokenSegment>) ValidatePairSets((IEnumerable<ITokenSegment>, IEnumerable<ITokenSegment>) pair)
        {
            throw new System.NotImplementedException();
        }

        public string ReplaceTokens(IEnumerable<ITokenSegment> tokenSegments, ITokenTranslator translator)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ITokenSegment> GetTokensSegments(Dictionary<int, TokenInfo> tokensByLocation)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<int, TokenInfo> FindAllTokens(string paragraph)
        {
            var result = new Dictionary<int, TokenInfo>();

            foreach (var (token, index) in trie.Find(paragraph))
            {
                if (token is null) continue;
                
                var closeValid = index > 0 && !char.IsWhiteSpace(paragraph[index - 1]);
                var openValid = index < paragraph.Length - token.Length 
                                && !char.IsWhiteSpace(paragraph[index + token.Length]);
                
                result[index] = new TokenInfo(
                    token, closeValid, openValid,
                    closeValid && openValid,
                    closeValid || openValid
                );
            }
            
            return result;
        }
    }

    [TestFixture]
    public class ParserShould
    {
        private ITokenParser parser;
        
        [SetUp]
        public void SetUp()
        {
            parser = TokenParserConfigurator.CreateTokenParser()
                .AddToken(new Token("-"))
                .AddToken(new Token("--"))
                .AddToken(new Token("__"))
                .AddToken(new Token("_<"))
                .Configure();
        }

        [TestCase("-a", true, false)]
        [TestCase("a-", false, true)]
        [TestCase("-", false, false)]
        [TestCase("a-a", true, true)]
        [TestCase("a -a", true, false)]
        [TestCase("a- a", false, true)]
        [TestCase("a-- a", false, true)]
        [TestCase("a --a", true, false)]
        public void FindAllTokens_Validation_Test(string text, bool expectedValidToStart, bool expectedValidToClose)
        {
            var actualToken = parser
                .FindAllTokens(text)
                .First()
                .Value;

            var actualValidToStart = actualToken.OpenValid;
            var actualValidToClose = actualToken.CloseValid;

            actualValidToStart.Should().Be(expectedValidToStart);
            actualValidToClose.Should().Be(expectedValidToClose);
        }
        
        [TestCase("a-a", 1)]
        [TestCase("a-a-a", 1, 3)]
        [TestCase("-a", 0)]
        [TestCase("a-", 1)]
        [TestCase("a--a", 1)]
        [TestCase("a---a", 1, 3)]
        [TestCase("a----a", 1, 3)]
        [TestCase("a--a--a", 1, 4)]
        [TestCase("a__a-_<a", 1, 4, 5)]
        [TestCase("a__", 1)]
        [TestCase("__a", 0)]
        [TestCase("a_<", 1)]
        [TestCase("_<a", 0)]
        public void FindAllTokens_Location_Test(string text, params int[] expectedIndexes)
        {
            var actualIndexes = parser
                .FindAllTokens(text)
                .Select(x => x.Key)
                .OrderBy(x => x)
                .ToArray();

            actualIndexes
                .Should()
                .Equal(expectedIndexes
                    .OrderBy(x => x)
                    .ToArray()
                );
        }
    }
}