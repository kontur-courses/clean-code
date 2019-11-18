using System.Collections.Generic;
using FluentAssertions;
using Markdown.ConverterTokens;
using Markdown.Tokens;
using Markdown.Transducer.ConverterTokenToHtml;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TransducerTests
    {
        private Transducer.Transducer transducer;
        [OneTimeSetUp]
        public void SetUpFixture()
        {
            transducer = new Transducer.Transducer();
            var singleEmphasis = new ConverterSingleEmphasis();
            var doubleEmphasis = new ConverterDoubleEmphasis();
            singleEmphasis.RegisterNested(new DoubleEmphasisToken("", 0, new IToken[0]), doubleEmphasis);
            doubleEmphasis.RegisterNested(new SingleEmphasisToken("", 0, new IToken[0]), singleEmphasis);
            transducer.Registred(new SingleEmphasisToken("", 0, new IToken[0]), singleEmphasis);
            transducer.Registred(new DoubleEmphasisToken("", 0, new IToken[0]), doubleEmphasis);
        }
        
        [Test, TestCaseSource(nameof(DifferentInputs))]
        public void Tokenize_IsNotEmpty(string str, IToken[] tokens, string outStr)
        {
            transducer.MakeHtmlString(str, tokens).Should().Be(outStr);
        }

        public static IEnumerable<TestCaseData>  DifferentInputs 
        {
            get
            {
                yield return new TestCaseData("Те _вы_ _од_", new IToken[]
                    {
                        new SingleEmphasisToken("вы", 3, new IToken[0]),
                        new SingleEmphasisToken("од", 8, new IToken[0]),
                    }, "Те <em>вы</em> <em>од</em>")
                    .SetName("Текст окруженный с двух сторон  одинарными символами подчерка");
                yield return new TestCaseData("Вн __дв _вы_ _од_ ра__.", new IToken[]
                    {
                        new DoubleEmphasisToken("дв _вы_ _од_ ра", 3, new IToken[]
                        {
                            new SingleEmphasisToken("вы", 3, new IToken[0]),
                            new SingleEmphasisToken("од", 8, new IToken[0]),
                        } ),
                    }, "Вн <strong>дв <em>вы</em> <em>од</em> ра</strong>.")
                    .SetName("Внутри двойного выделения одинарное распознает.");
                yield return new TestCaseData("Вн _дв __вы__ __од__ ра_.", new IToken[]
                    {
                        new SingleEmphasisToken("дв __вы__ __од__ ра", 3, new IToken[0]), 
                    }, "Вн <em>дв __вы__ __од__ ра</em>.")
                    .SetName("Внутри одинарного выделения двойное не распознает.");
            }
        }
    }
}