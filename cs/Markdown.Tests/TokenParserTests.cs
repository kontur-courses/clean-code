using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenParserTests
    {
        private TokenParser.TokenParser sut;

        [SetUp]
        public void SetUp()
        {
            sut = new TokenParser.TokenParser();
        }

        [Test(Description = "null")]
        public void Parse_ShouldThrowException_WhenArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Parse(null));
        }

        [Test(Description = "empty")]
        public void Parse_ShouldNotApplyParsing_WhenNoToken()
        {
            var tokens = Array.Empty<Token>();
            var expected = Array.Empty<TagNode>();
            AssertParse(tokens, expected);
        }


        [Test(Description = "A")]
        public void Parse_ShouldNotApply_WhenOnlyTextToken()
        {
            var tokens = new[]
            {
                Token.Text("A")
            };
            var expected = new[]
            {
                Token.Text("A").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"A\n")]
        public void Parse_ShouldConcatenateTextAndNewLine_WhenNewLineAfter()
        {
            var tokens = new[]
            {
                Token.Text("A"),
                Token.NewLine
            };
            var expected = new[]
            {
                CreateTextTokenFrom(Token.Text("A"), Token.NewLine).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\nA")]
        public void Parse_ShouldConcatenateTextAndNewLine_WhenNewLineBefore()
        {
            var tokens = new[]
            {
                Token.NewLine,
                Token.Text("A")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(Token.NewLine, Token.Text("A")).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "AB separated")]
        public void Parse_ShouldConcatenateTextTokens()
        {
            var tokens = new[]
            {
                Token.Text("A"),
                Token.Text("B")
            };
            var expected = new[]
            {
                Token.Text("AB").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "A B")]
        public void Parse_ShouldParseTextWithSeveralWords()
        {
            var tokens = new[]
            {
                Token.Text("A B")
            };
            var expected = new[]
            {
                Token.Text("A B").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\")]
        public void Parse_ShouldTransformEscapeToText()
        {
            var tokens = new[]
            {
                Token.Escape
            };
            var expected = new[]
            {
                Token.Escape.ToText().ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\A")]
        public void Parse_ShouldConcatenateEscapeAndText()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Text("A")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(Token.Escape, Token.Text("A")).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\# A")]
        public void Parse_ShouldEscapeHeader()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Header1,
                Token.Text("A")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\\n")]
        public void Parse_ShouldEscapeNotEscapeNewLine()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.NewLine
            };
            var expected = new[]
            {
                CreateTextTokenFrom(Token.Escape, Token.NewLine).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\_A_")]
        public void Parse_ShouldEscapeFormattingCharacter_WhenSingleCharacter()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Cursive,
                Token.Text("A"),
                Token.Cursive
            };
            var expected = new[]
            {
                CreateTextTokenFrom(Token.Cursive, Token.Text("A"), Token.Cursive).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = @"\\_A_")]
        public void Parse_ShouldEscapeFormattingCharacter_WhenDoubleCharacter()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Escape,
                Token.Cursive,
                Token.Text("A"),
                Token.Cursive
            };
            var expected = new[]
            {
                Token.Escape.ToText().ToNode(),
                new TagNode(Tag.Cursive(Token.Cursive.Value), Tag.Text("A").ToNode())
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = @"\\_A B_")]
        public void Parse_ShouldEscapeFormattingCharacter_WhenDoubleCharacter_AndSeveralWords()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Escape,
                Token.Cursive,
                Token.Text("A B"),
                Token.Cursive
            };
            var expected = new[]
            {
                Token.Escape.ToText().ToNode(),
                new TagNode(Tag.Cursive(Token.Cursive.Value), Tag.Text("A B").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\__A_")]
        public void Parse_ShouldEscapeCursiveCharacter_WhenSingleBoldCharacter()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Bold,
                Token.Text("A"),
                Token.Cursive
            };
            var expected = new[]
            {
                Token.Cursive.ToText().ToNode(),
                new TagNode(Tag.Cursive(Token.Cursive.Value), Tag.Text("A").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A_")]
        public void Parse_ShouldApplyFormatting_WhenCharactersSurroundsWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A"),
                token
            };
            var expected = new[]
            {
                new TagNode(token.ToTag(), Tag.Text("A").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A")]
        public void Parse_ShouldTransformFormattingToText_WhenSingleBeforeText(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(token, Token.Text("A")).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "A_")]
        public void Parse_ShouldMakeFormattingAsText_WhenSingleAfterText(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("A"),
                token
            };
            var expected = new[]
            {
                CreateTextTokenFrom(Token.Text("A"), token).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "_ A_")]
        public void Parse_ShouldNotApplyFormatting_WhenBeforeWhitespace(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text(" A"),
                token
            };
            var expected = new[]
            {
                CreateTextTokenFrom(token, Token.Text(" A"), token).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A _")]
        public void Parse_ShouldNotApplyUnderscore_WhenClosing_AndAfterWhitespace(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A "),
                token
            };
            var expected = new[]
            {
                CreateTextTokenFrom(token, Token.Text("A "), token).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "__A _B_ C__")]
        public void Parse_ShouldApplyCursive_WhenWithinBoldSurroundsSingleWord()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("A "),
                Token.Cursive,
                Token.Text("B"),
                Token.Cursive,
                Token.Text(" C"),
                Token.Bold
            };
            var expected = new[]
            {
                new TagNode(Tag.Bold(Token.Bold.Value),
                    new[]
                    {
                        Token.Text("A ").ToNode(),
                        new TagNode(Tag.Cursive(Token.Cursive.Value), Token.Text("B").ToNode()),
                        Token.Text(" C").ToNode()
                    })
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "__A _B C_ D__")]
        public void Parse_ShouldApplyCursive_WhenWithinBoldSurroundsSeveralWords()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("A "),
                Token.Cursive,
                Token.Text("B C"),
                Token.Cursive,
                Token.Text(" D"),
                Token.Bold
            };
            var expected = new[]
            {
                new TagNode(Tag.Bold(Token.Bold.Value),
                    new[]
                    {
                        Token.Text("A ").ToNode(),
                        new TagNode(Tag.Cursive(Token.Cursive.Value), Tag.Text("B C").ToNode()),
                        Token.Text(" D").ToNode()
                    })
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A __B__ C_")]
        public void Parse_ShouldNotApplyBold_WhenWithinCursiveSurroundsSingleWord()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("A "),
                Token.Bold,
                Token.Text("B"),
                Token.Bold,
                Token.Text(" C"),
                Token.Cursive
            };
            var expected = new[] { new TagNode(Tag.Cursive(Token.Cursive.Value), Tag.Text("A __B__ C").ToNode()) };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A __B C__ D_")]
        public void Parse_ShouldNotApplyBold_WhenWithinCursiveSurroundsSeveralWords()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("A "),
                Token.Bold,
                Token.Text("B C"),
                Token.Bold,
                Token.Text(" D"),
                Token.Cursive
            };
            var expected = new[] { new TagNode(Tag.Cursive(Token.Cursive.Value), Token.Text("A __B C__ D").ToNode()) };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_12_")]
        public void Parse_ShouldNotApplyFormatting_WhenSurroundsNumber(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("12"),
                token
            };
            var expected = new[]
            {
                CreateTextTokenFrom(token, Token.Text("12"), token).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A_BC")]
        public void Parse_ShouldApplyFormatting_WhenSurroundsBeginningOfWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A"),
                token,
                Token.Text("BC")
            };
            var expected = new[]
            {
                new TagNode(token.ToTag(), Token.Text("A").ToNode()),
                Token.Text("BC").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "A_B_C")]
        public void Parse_ShouldApplyFormatting_WhenSurroundsMiddleOfWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("A"),
                token,
                Token.Text("B"),
                token,
                Token.Text("C")
            };
            var expected = new[]
            {
                Token.Text("A").ToNode(),
                new TagNode(token.ToTag(), Token.Text("B").ToNode()),
                Token.Text("C").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "AB_C_")]
        public void Parse_ShouldApplyFormatting_WhenSurroundsEndOfWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("AB"),
                token,
                Token.Text("C"),
                token
            };
            var expected = new[]
            {
                Token.Text("AB").ToNode(),
                new TagNode(token.ToTag(), Token.Text("C").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A _B_ C_")]
        public void Parse_ShouldApplyFormatting_WhenSameTokenInsideFormatting(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A "),
                token,
                Token.Text("B"),
                token,
                Token.Text(" C"),
                token
            };
            var expected = new[]
            {
                new TagNode(token.ToTag(), new[]
                {
                    Token.Text("A ").ToNode(),
                    new TagNode(token.ToTag(), Token.Text("B").ToNode()),
                    Token.Text(" C").ToNode()
                })
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_AC _DE")]
        public void Parse_ShouldNotApplyFormatting_WhenBothUnderscoreBeforeWordBeginning(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("AC "),
                token,
                Token.Text("DE"),
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "_AC D_E")]
        public void Parse_ShouldNotApplyFormatting_WhenFirstUnderscoreInBeginning_AndSecondUnderscoreInMiddle(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("AC D"),
                token,
                Token.Text("E"),
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "_AB CD_")]
        public void Parse_ShouldApplyBoldFormat_WhenFirstUnderscoreInBeginning_AndSecondUnderscoreInEnding(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("AB CD"),
                token
            };
            var expected = new[]
            {
                new TagNode(token.ToTag(), Token.Text("AB CD").ToNode())
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "A_C _DE")]
        public void Parse_ShouldNotApplyFormatting_WhenFirstUnderscoreInMiddle_AndSecondUnderscoreInBeginning(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("A"),
                token,
                Token.Text("C "),
                token,
                Token.Text("DE")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "A_C D_E")]
        public void Parse_ShouldNotApplyFormatting_WhenFirstUnderscoreInMiddle_AndSecondUnderscoreInMiddle(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("A"),
                token,
                Token.Text("C D"),
                token,
                Token.Text("E")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "A_C DE_")]
        public void Parse_ShouldNotApplyFormatting_WhenFirstUnderscoreInMiddle_AndSecondUnderscoreInEnd(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("A"),
                token,
                Token.Text("C DE"),
                token,
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "AC_ _DE")]
        public void Parse_ShouldNotApplyFormatting_WhenFirstUnderscoreInEnd_AndSecondUnderscoreInBeginning(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("AC"),
                token,
                Token.Text(" "),
                token,
                Token.Text("DE"),
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "AC_ D_E")]
        public void Parse_ShouldNotApplyFormatting_WhenFirstUnderscoreInEnd_AndSecondUnderscoreInMiddle(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("AC"),
                token,
                Token.Text(" D"),
                token,
                Token.Text("E")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "AC_ DE_")]
        public void Parse_ShouldNotApplyFormatting_WhenFirstUnderscoreInEnd_AndSecondUnderscoreInEnd(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("AC"),
                token,
                Token.Text(" DE"),
                token,
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "A_B?C_D")]
        public void Parse_ShouldNotApplyFormatting_WhenInMiddleOfDifferentWords(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token,
            [ValueSource(nameof(GetWordSeparators))]
            string wordSeparator)
        {
            var tokenCharacter = token.Value;
            var tokens = new[]
            {
                Token.Text("A"),
                token,
                Token.Text($"B{wordSeparator}C"),
                token,
                Token.Text("D")
            };
            var expected = new[]
            {
                CreateTextTokenFrom(
                    Token.Text("A"),
                    token,
                    Token.Text($"B{wordSeparator}C"),
                    token,
                    Token.Text("D")
                ).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "__A _B C__")]
        public void Parse_ShouldNotApplyFormatting_WhenCursiveBetweenBold()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("A "),
                Token.Cursive,
                Token.Text("B C"),
                Token.Bold
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A __B C_")]
        public void Parse_ShouldNotApplyFormatting_WhenBoldBetweenCursive()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("A "),
                Token.Bold,
                Token.Text("B C"),
                Token.Cursive
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_A__B_C__ | __A_B__C_")]
        public void Parse_ShouldNotApplyFormatting_WhenFormattingTokensIntersected(
            [ValueSource(nameof(GetOppositeTokens))]
            (Token, Token) pair)
        {
            var (token, opposite) = pair;
            var tokens = new[]
            {
                token,
                Token.Text("A"),
                opposite,
                Token.Text("B"),
                token,
                Token.Text("C"),
                opposite
            };
            var expected = new[]
            {
                CreateTextTokenFrom(
                    token,
                    Token.Text("A"),
                    opposite,
                    Token.Text("B"),
                    token,
                    Token.Text("C"),
                    opposite
                ).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "_empty_")]
        public void Parse_ShouldNotApplyFormatting_WhenEmptyText([ValueSource(nameof(GetFormattingTokens))] Token token)
        {
            var tokens = new[]
            {
                token,
                token
            };
            var expected = new[]
            {
                CreateTextTokenFrom(token, token).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "# A")]
        public void Parse_ShouldFormatHeader_WhenJustText()
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("A")
            };
            var expected = new[]
            {
                new TagNode(Tag.Header1(Token.Header1.Value), Token.Text("A").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "# empty")]
        public void Parse_ShouldFormatHeader_WhenEmptyText()
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("")
            };
            var expected = new[]
            {
                Tag.Header1(Token.Header1.Value).ToNode()
            };
            AssertParse(tokens, expected);
        }


        [Test(Description = "# A _B_")]
        public void Parse_ShouldFormatHeader_WhenFormattingText(
            [ValueSource(nameof(GetFormattingTokens))] 
            Token token)
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("A "),
                token,
                Token.Text("B"),
                token
            };
            var expected = new[]
            {
                new TagNode(Tag.Header1(Token.Header1.Value), new[]
                {
                    Token.Text("A ").ToNode(),
                    new TagNode(token.ToTag(), Token.Text("B").ToNode())
                })
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"# A\nB")]
        public void Parse_ShouldFormatHeader_WhenTextAfterNewLine()
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("A"),
                Token.NewLine,
                Token.Text("B")
            };
            var expected = new[]
            {
                new TagNode(Tag.Header1(Token.Header1.Value), new[]
                {
                    Token.Text("A").ToNode()
                }),
                Token.Text("\nB").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"_A\nB_")]
        public void Parse_ShouldFlushFormatting_WhenNewLine(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A"),
                Token.NewLine,
                Token.Text("B"),
                token
            };
            var expected = new[]
            {
                CreateTextTokenFrom(token,
                    Token.Text("A"),
                    Token.NewLine,
                    Token.Text("B"),
                    token
                ).ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = "[A](B)")]
        public void Parse_ShouldCreateLink()
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                Token.Text("A"),
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                new TagNode(Tag.Link("B"), Tag.Text("A").ToNode())
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "[A[A](B)")]
        public void Parse_ShouldReopenLinkOnDoubleOpenSquareBracket()
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                Token.Text("A"),
                Token.OpenSquareBracket,
                Token.Text("A"),
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                Tag.Text("[A").ToNode(),
                new TagNode(Tag.Link("B"), Tag.Text("A").ToNode())
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "[A]B(C)")]
        public void Parse_ShouldNotCreateLink_WhenHasCharactersBetweenNameAndLink()
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                Token.Text("A"),
                Token.CloseSquareBracket,
                Token.Text("B"),
                Token.OpenCircleBracket,
                Token.Text("C"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "[_A_](B)")]
        public void Parse_ShouldCreateLink_WhenFormattingInsideName(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                token,
                Token.Text("A"),
                token,
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                new TagNode(Tag.Link("B"), 
                    new TagNode(token.ToTag(), 
                        Tag.Text("A").ToNode()))
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "[_A](B)")]
        public void Parse_ShouldCreateLink_WhenSingleFormattingTokenInsideName(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                token,
                Token.Text("A"),
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                new TagNode(Tag.Link("B"), CreateTextTokenFrom(token, Token.Text("A")).ToNode())
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = "[# A\n](B)")]
        public void Parse_ShouldNotAddHeaderInName_WhenCreateLink()
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                Token.Header1,
                Token.Text("A"),
                Token.NewLine,
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                new TagNode(Tag.Link("B"), 
                        CreateTextTokenFrom(
                            Token.Header1,
                            Token.Text("A"),
                            Token.NewLine).ToNode())
            };
            AssertParse(tokens, expected);
        }
        
        
        [Test(Description = @"\[A](B)")]
        public void Parse_ShouldNotCreateLink_WhenEscapeOpenSquareBracket()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.OpenSquareBracket,
                Token.Text("A"),
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens.Except(new[] {Token.Escape}).ToArray()).ToNode(),
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = @"[A\](B)")]
        public void Parse_ShouldNotCreateLink_WhenEscapeCloseSquareBracket()
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                Token.Text("A"),
                Token.Escape,
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens.Except(new[] {Token.Escape}).ToArray()).ToNode(),
            };
            AssertParse(tokens, expected);
        }
        
        [Test(Description = @"[A](B\)")]
        public void Parse_ShouldNotCreateLink_WhenEscapeCloseCircleBracket()
        {
            var tokens = new[]
            {
                Token.OpenSquareBracket,
                Token.Text("A"),
                Token.CloseSquareBracket,
                Token.OpenCircleBracket,
                Token.Text("B"),
                Token.Escape,
                Token.CloseCircleBracket
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens.Except(new[] {Token.Escape}).ToArray()).ToNode(),
            };
            AssertParse(tokens, expected);
        }

        [Test(Description = @"\]")]
        public void Parse_ShouldNotEscapeCloseBracket_WhenIsNotInLinkContext()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.CloseSquareBracket,
            };
            var expected = new[]
            {
                CreateTextTokenFrom(tokens).ToNode()
            };
            AssertParse(tokens, expected);
        }
        
        public static IEnumerable<(Token token, Token opposite)> GetOppositeTokens()
        {
            yield return (Token.Cursive, Token.Bold);
            yield return (Token.Bold, Token.Cursive);
        }

        public static IEnumerable<string> GetWordSeparators() => " !?.,;:\\|{}[]()*&^$".Select(x => x.ToString());


        public static IEnumerable<Token> GetFormattingTokens()
        {
            yield return Token.Cursive;
            yield return Token.Bold;
        }

        private void AssertParse(IEnumerable<Token> tokens, IEnumerable<TagNode> expectedNodes)
        {
            var parsed = sut.Parse(tokens);
            parsed.Should().Equal(expectedNodes);
        }

        private static Token CreateTextTokenFrom(params Token[] tokens) 
            => Token.Text(StringUtils.Join(tokens.Select(x => x.Value)));
    }
}