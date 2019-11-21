using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class SyntaxTree
    {
        public readonly SyntaxTreeType Type;
        public readonly SyntaxTree Parent;
        public List<SyntaxTree> Children;
        public bool IsHasChildren => Children != null && Children.Count > 0;
        public readonly List<Token> Tokens;
        public readonly string StartLine;
        public readonly string EndLine;

        public SyntaxTree(SyntaxTreeType type, List<Token> treeTokens, SyntaxTree parent = null, string startLine = "", string endLine = "")
        {
            Type = type;
            Tokens = treeTokens;
            Parent = parent;
            StartLine = startLine;
            EndLine = endLine;
        }

        public void AddChildTrees(Dictionary<TokenType, SyntaxTreeType> syntaxTreeTypes)
        {
            var childTrees = new List<SyntaxTree>();
            var tokens = new List<Token>();
            var isHaveNotOnlyTextTreeType = false;
            for (var i = 0; i < Tokens.Count; i++)
            {
                var token = Tokens[i];
                if (!syntaxTreeTypes.ContainsKey(token.TokenType))
                {
                    tokens.Add(token);
                    continue;
                }
                if (AddTextTree(tokens, childTrees))
                    tokens = new List<Token>();
                var childTree = GetChildSyntaxTree(syntaxTreeTypes[token.TokenType], i, token.TokenType);
                isHaveNotOnlyTextTreeType = childTree.Type != SyntaxTreeType.Text || isHaveNotOnlyTextTreeType;
                childTrees.Add(childTree);
                i += GetIndexOffset(childTree);
            }
            AddTextTree(tokens, childTrees);
            Children = isHaveNotOnlyTextTreeType ? childTrees : new List<SyntaxTree>();
        }

        private bool AddTextTree(List<Token> tokens, List<SyntaxTree> childTrees)
        {
            if (tokens.Count <= 0) 
                return false;
            var childTree = new SyntaxTree(SyntaxTreeType.Text, tokens, this);
            childTrees.Add(childTree);
            return true;
        }

        private static int GetIndexOffset(SyntaxTree childTree)
        {
            var offset = childTree.Tokens.Count - 1;
            if (childTree.StartLine.Length > 0)
                offset++;
            if (childTree.EndLine.Length > 0)
                offset++;
            return offset;
        }

        private SyntaxTree GetChildSyntaxTree(
            SyntaxTreeType treeType, int tokenIndex, TokenType stopTokenType)
        {
            var treeTokens = new List<Token>();
            for (var i = tokenIndex + 1; i < Tokens.Count; i++)
            {
                var token = Tokens[i];
                if (token.TokenType == stopTokenType)
                    return new SyntaxTree(treeType, treeTokens, this,
                        Token.DefaultStringForTokenTypes[stopTokenType], Token.DefaultStringForTokenTypes[stopTokenType]);
                treeTokens.Add(token);
            }
            return new SyntaxTree(SyntaxTreeType.Text, treeTokens, this, Token.DefaultStringForTokenTypes[stopTokenType]);
        }
    }
}
