using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    public class Tokenizer_should
    {        
        private Tokenizer tokenizer;

        [SetUp]
        public void CreatePossibleTextTypeLists()
        {            
            tokenizer = new Tokenizer();
        }

        [Test]
        public void TokenizerShouldFindItalicToken()
        {                                               
            var tokens = tokenizer.GetTokens("_hello_n");            
            tokens[0].innerText.Should().BeEquivalentTo("_hello_");
            tokens = tokenizer.GetTokens("_hello_");
            tokens[0].innerText.Should().BeEquivalentTo("_hello_");
        }
        
        [Test]
        public void TokenizerShouldFindStrongToken()
        {
            var tokens = tokenizer.GetTokens("__hello__n");
            tokens[0].innerText.Should().BeEquivalentTo("__hello__");
            tokens = tokenizer.GetTokens("__hello__");
            tokens[0].innerText.Should().BeEquivalentTo("__hello__");

        }
        
        [Test]
        public void TokenizerShouldFoundNoTokenIfNotPair()
        {            
            var tokens = tokenizer.GetTokens("__should not _work");
            tokens.Should().BeEmpty();
        }
        
        [Test]
        public void TokenizerShouldFoundNoTokenIfWhiteSpaceBefore()
        {           
            var tokens = tokenizer.GetTokens("_should not _work");
            tokens.Should().BeEmpty();
            tokens = tokenizer.GetTokens("__should not __work");
            tokens.Should().BeEmpty();
        }
        
        [Test]
        public void TokenizerShouldFoundNoTokenIfWhiteSpaceAfter()
        {            
            var tokens = tokenizer.GetTokens("__ should not__work");
            tokens.Should().BeEmpty();
            tokens = tokenizer.GetTokens("_ should not_work");
            tokens.Should().BeEmpty();
        }
        
        [Test]
        public void TokenizerShouldFoundItalicToken_InsideBoldToken()
        {                        
            var tokens = tokenizer.GetTokens("__should _work_ well__ work");            
            tokens[0].innerText.Should().BeEquivalentTo("_work_");
            tokens[1].innerText.Should().BeEquivalentTo("__should _work_ well__");
        }
        
        
        [Test]
        public void TokenizerShouldFoundBoldToken_InsideItalicToken()
        {           
            var tokens = tokenizer.GetTokens("_should __work__ well_ work");            
            tokens[0].innerText.Should().BeEquivalentTo("__work__");
            tokens[1].innerText.Should().BeEquivalentTo("_should __work__ well_");
        }
        
        [Test]
        public void TokenizerShouldFoundTwoItalicToken_InsideStrongToken()
        {            
            var tokens = tokenizer.GetTokens("__should _work_ _well_ fine__ work");          
            tokens[0].innerText.Should().BeEquivalentTo("_work_");
            tokens[1].innerText.Should().BeEquivalentTo("_well_");
            tokens[2].innerText.Should().BeEquivalentTo("__should _work_ _well_ fine__");
        }
        
        [Test]
        public void TokenizerShouldFoundNothingIfBlocked()
        {
            var tokens = tokenizer.GetTokens(@"should \_not_ work");
            tokens.Should().BeEmpty();            
        }        

        [Test]
        public void TokenizerShouldFoundTwoStrongToken_InsideItalicToken()
        {
            var tokens = tokenizer.GetTokens("_a __b__ __c__ d_");           
            tokens[0].innerText.Should().BeEquivalentTo("__b__");
            tokens[1].innerText.Should().BeEquivalentTo("__c__");
            tokens[2].innerText.Should().BeEquivalentTo("_a __b__ __c__ d_");
            tokens[1].outerToken.Item2.Should().Be(0);
        }
    }
}
