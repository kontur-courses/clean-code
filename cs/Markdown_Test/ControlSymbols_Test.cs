using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Test
{
    [TestFixture]
    public class ControlSymbols_Test
    {
        [Test]
        public void MakeDecisionForTwoUnderscores_ShouldStop_IfTakesTwoUnderscoresWithWhiteSpace()
        {
            var input = "asd__ ";
            
            var actual = ControlSymbols.DecisionByControlSymbol["__"](input, 3);

            actual.Should().Be(StopSymbolDecision.Stop);
        }
        
        [Test]
        public void MakeDecisionForTwoUnderscores_ShouldStop_IfTakesTwoUnderscoresAndEnd()
        {
            var input = "asd__";
            
            var actual = ControlSymbols.DecisionByControlSymbol["__"](input, 3);

            actual.Should().Be(StopSymbolDecision.Stop);
        }
        
        [Test]
        public void MakeDecisionForTwoUnderscores_ShouldContinue_IfTakesWhiteSpaceTwoUnderscoresAndEnd()
        {
            var input = "asd __";
            
            var actual = ControlSymbols.DecisionByControlSymbol["__"](input, 4);

            actual.Should().Be(StopSymbolDecision.AddChar);
        }
        
        [Test]
        public void MakeDecisionForTwoUnderscores_ShouldContinue_IfTakesWhiteSpaceTwoUnderscoresAndWhiteSpace()
        {
            var input = "asd __ ";
            
            var actual = ControlSymbols.DecisionByControlSymbol["__"](input, 4);

            actual.Should().Be(StopSymbolDecision.AddChar);
        }
        
        [Test]
        public void MakeDecisionForTwoUnderscores_ShouldNestedToken_IfTakesWhiteSpaceOneUnderscoresAndASymbol()
        {
            var input = "asd _asd";
            
            var actual = ControlSymbols.DecisionByControlSymbol["__"](input, 4);

            actual.Should().Be(StopSymbolDecision.NestedToken);
        }
        
        [Test]
        public void MakeDecisionForTwoUnderscores_ShouldNesredToken_IfTakesWhiteSpaceTwoUnderscoresAndASymbol()
        {
            var input = "asd __asd";
            
            var actual = ControlSymbols.DecisionByControlSymbol["__"](input, 4);

            actual.Should().Be(StopSymbolDecision.NestedToken);
        }

        [Test]
        public void MakeDecisionForOneUnderscores_ShouldStop_IfTakesCharUnderscoreSpace()
        {
            var input = "ab_ ";

            var actual = ControlSymbols.DecisionByControlSymbol["_"](input, 2);

            actual.Should().Be(StopSymbolDecision.Stop);
        }
        
        [Test]
        public void MakeDecisionForOneUnderscores_ShouldStop_IfTakesCharUnderscoreEnd()
        {
            var input = "ab_";

            var actual = ControlSymbols.DecisionByControlSymbol["_"](input, 2);

            actual.Should().Be(StopSymbolDecision.Stop);
        }
        
        [Test]
        public void MakeDecisionForOneUnderscores_ShouldContinue_IfTakesCharUnderscoreChar()
        {
            var input = "ab_a";

            var actual = ControlSymbols.DecisionByControlSymbol["_"](input, 2);

            actual.Should().Be(StopSymbolDecision.AddChar);
        }
        
        [Test]
        public void MakeDecisionForOneUnderscores_ShouldContinue_IfTakesSpaceUnderscoreChar()
        {
            var input = "a _a";

            var actual = ControlSymbols.DecisionByControlSymbol["_"](input, 2);

            actual.Should().Be(StopSymbolDecision.NestedToken);
        }
        
        [Test]
        public void MakeDecisionForOneUnderscores_ShouldContinue_IfTakesCharTwoUnderscoreSpace()
        {
            var input = "ab__ ";

            var actual = ControlSymbols.DecisionByControlSymbol["_"](input, 3);

            actual.Should().Be(StopSymbolDecision.AddChar);
        }
        
        [Test]
        public void MakeDecisionForOneUnderscores_ShouldNestedToken_IfTakesSpaceTwoUnderscoreChar()
        {
            var input = "ab __a";

            var actual = ControlSymbols.DecisionByControlSymbol["_"](input, 3);

            actual.Should().Be(StopSymbolDecision.NestedToken);
        }
    }
}