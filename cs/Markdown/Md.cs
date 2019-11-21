using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private const char EscapeCharacter = '\\';

        private static readonly Dictionary<SyntaxTreeType, Tuple<string, string>> TagsForTokenTypes = new Dictionary<SyntaxTreeType, Tuple<string, string>>
        {
            { SyntaxTreeType.TextInUnderscores, Tuple.Create("<em>", "</em>") },
            { SyntaxTreeType.TextInDoubleUnderscores, Tuple.Create("<strong>", "</strong>") }
        };

        private static readonly Dictionary<TokenType, SyntaxTreeType> TreeTypesFromTokenType = new Dictionary<TokenType, SyntaxTreeType>
        {
            { TokenType.Underscore, SyntaxTreeType.TextInUnderscores },
            { TokenType.DoubleUnderscores, SyntaxTreeType.TextInDoubleUnderscores }
        };

        public static readonly List<TokenDescription> MdTokenDescriptions = new List<TokenDescription>
        {
            new TokenDescription((text, position) => TokenReader.ReadEscapedSymbol(text, position, EscapeCharacter)),
            new TokenDescription((text, position) => TokenReader.ReadSubstringToken(text, position, "__", TokenType.DoubleUnderscores)),
            new TokenDescription((text, position) => TokenReader.ReadSubstringToken(text, position, "_", TokenType.Underscore)),
            new TokenDescription((text, position) => TokenReader.ReadTokenWithRuleForSymbols(text, position, char.IsWhiteSpace, TokenType.Whitespaces)),
            new TokenDescription((text, position) => TokenReader.ReadTokenWithRuleForSymbols(text, position, char.IsLetter, TokenType.Letters)),
            new TokenDescription((text, position) => TokenReader.ReadTokenWithRuleForSymbols(text, position, char.IsDigit, TokenType.Number)),
        };

        public string Render(string markdownText)
        {
            var reader = new TokenReader(MdTokenDescriptions);
            var tokens = reader.SplitToTokens(markdownText);
            var rootTree = new SyntaxTree(SyntaxTreeType.Text, tokens);
            rootTree.AddChildTrees(TreeTypesFromTokenType);
            foreach (var childTree in rootTree.Children)
            {
                if (childTree.Type == SyntaxTreeType.TextInDoubleUnderscores)
                    childTree.AddChildTrees(TreeTypesFromTokenType);
            }
            var converter = new TreeConverter(TagsForTokenTypes, RuleForTagsAdding.IsNeedToAddTags);
            return converter.GetTaggedText(rootTree, 0);
        }
    }
}
