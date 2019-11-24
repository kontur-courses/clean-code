using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        private const char EscapeCharacter = '\\';

        public static readonly Dictionary<SyntaxTreeType, Tuple<string, string>> TagsForTokenTypes = new Dictionary<SyntaxTreeType, Tuple<string, string>>
        {
            { SyntaxTreeType.ItalicText, Tuple.Create("<em>", "</em>") },
            { SyntaxTreeType.BoldText, Tuple.Create("<strong>", "</strong>") }
        };

        private static readonly List<SyntaxTreesDescription> SyntaxTreesDescriptions = new List<SyntaxTreesDescription>
        {
            new SyntaxTreesDescription(HtmlTreeConverter.TryAddSurroundingTags),
            new SyntaxTreesDescription(HtmlTreeConverter.TryAddLinkTag),
            new SyntaxTreesDescription(HtmlTreeConverter.AddDefaultText)
        };

        private static readonly Dictionary<TokenType, SyntaxTreeType> TreeTypesFromTokenType = new Dictionary<TokenType, SyntaxTreeType>
        {
            { TokenType.Underscore, SyntaxTreeType.ItalicText },
            { TokenType.DoubleUnderscores, SyntaxTreeType.BoldText },
            { TokenType.LeftSquareBracket, SyntaxTreeType.TextInSquareBrackets },
            { TokenType.LeftParenthesis, SyntaxTreeType.TextInParentheses },
        };

        private static readonly Dictionary<TokenType, TokenType> StopTokenTypes = new Dictionary<TokenType, TokenType>
        {
            { TokenType.Underscore, TokenType.Underscore },
            { TokenType.DoubleUnderscores, TokenType.DoubleUnderscores },
            { TokenType.LeftSquareBracket, TokenType.RightSquareBracket },
            { TokenType.LeftParenthesis, TokenType.RightParenthesis },
        };

        public static readonly List<TokenDescription> MdTokenDescriptions = new List<TokenDescription>
        {
            new TokenDescription((text, position, index) => 
                TokenReader.ReadEscapedSymbol(text, position, index, EscapeCharacter)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadSubstringToken(text, position, index, "__", TokenType.DoubleUnderscores)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadSubstringToken(text, position, index, "_", TokenType.Underscore)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadSubstringToken(text, position, index, "[", TokenType.LeftSquareBracket)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadSubstringToken(text, position, index, "]", TokenType.RightSquareBracket)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadSubstringToken(text, position, index, "(", TokenType.LeftParenthesis)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadSubstringToken(text, position, index, ")", TokenType.RightParenthesis)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadTokenWithRuleForSymbols(text, position, index, char.IsWhiteSpace, TokenType.Whitespaces)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadTokenWithRuleForSymbols(text, position, index, char.IsLetter, TokenType.Letters)),
            new TokenDescription((text, position, index) => 
                TokenReader.ReadTokenWithRuleForSymbols(text, position, index, char.IsDigit, TokenType.Number))
        };

        public string Render(string markdownText)
        {
            var reader = new TokenReader(MdTokenDescriptions);
            var tokens = reader.SplitToTokens(markdownText);
            var rootTree = new SyntaxTree(SyntaxTreeType.Text, tokens);
            AddChildTrees(rootTree);
            var converter = new TreeConverter(SyntaxTreesDescriptions, rootTree);
            return converter.GetTaggedText();
        }

        public void AddChildTrees(SyntaxTree tree)
        {
            tree.AddChildTrees(TreeTypesFromTokenType, StopTokenTypes);
            foreach (var childTree in tree.Children)
            {
                if (childTree.Type == SyntaxTreeType.BoldText || childTree.Type == SyntaxTreeType.TextInSquareBrackets)
                    AddChildTrees(childTree);
            }
        }
    }
}
