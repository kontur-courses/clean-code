using System.Collections.Generic;
using FluentAssertions;
using Markdown.CoreParser;
using Markdown.CoreParser.ConverterInTokens;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private Parser parser;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            parser = new Parser();
            var singleEmphasis = new SingleEmphasis();
            var doubleEmphasis = new DoubleEmphasis();
            doubleEmphasis.RegisterNested(singleEmphasis);
            parser.Register(singleEmphasis);
            parser.Register(doubleEmphasis);
        }
        
        [Test, TestCaseSource(nameof(DifferentInputs))]
        public void Tokenize_IsNotEmpty(string str, IToken[] tokens)
        {
            parser.Tokenize(str).Should().Equal(tokens);
        }

        public static IEnumerable<TestCaseData>  DifferentInputs 
        {
            get
            {
                yield return new TestCaseData("Те _вы_ _од_", new IToken[]
                    {
                        new SingleEmphasisToken("вы", 3, new IToken[0]),
                        new SingleEmphasisToken("од", 8, new IToken[0]),
                    })
                    .SetName("Текст окруженный с двух сторон  одинарными символами подчерка");
                yield return new TestCaseData("Вн __дв _вы_ _од_ ра__.", new IToken[]
                    {
                        new DoubleEmphasisToken("дв _вы_ _од_ ра", 3, new IToken[]
                        {
                            new SingleEmphasisToken("вы", 3, new IToken[0]),
                            new SingleEmphasisToken("од", 8, new IToken[0]),
                        }), 
                    })
                    .SetName("Внутри двойного выделения одинарное распознает.");
                yield return new TestCaseData("Вн _дв __вы__ __од__ ра_.", new IToken[]
                    {
                        new SingleEmphasisToken("дв __вы__ __од__ ра", 3, new IToken[0]), 
                    })
                    .SetName("Внутри одинарного выделения двойное не распознает.");
            }
        }
    }
}