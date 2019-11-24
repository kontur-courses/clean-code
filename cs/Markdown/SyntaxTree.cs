using System.Collections.Generic;

namespace Markdown
{
    public class SyntaxTree
    {
        public readonly SyntaxTreeType Type;
        public readonly SyntaxTree Parent;
        public List<SyntaxTree> Children;
        public bool IsHasChildren => Children.Count > 0;
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
            Children = new List<SyntaxTree>();
        }

        public void AddChildTrees(Dictionary<TokenType, SyntaxTreeType> syntaxTreeTypes, Dictionary<TokenType, TokenType> stopTokenTypes)
        {
            var childTrees = new List<SyntaxTree>();
            var tokens = new List<Token>();
            var haveOnlyTextTypeTrees = true;
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
                var childTree = GetChildSyntaxTree(syntaxTreeTypes[token.TokenType], i, token.TokenType, stopTokenTypes[token.TokenType]);
                haveOnlyTextTypeTrees = childTree.Type == SyntaxTreeType.Text && haveOnlyTextTypeTrees;
                childTrees.Add(childTree);
                i += GetIndexOffset(childTree);
            }
            AddTextTree(tokens, childTrees);
            if (!haveOnlyTextTypeTrees)
                Children = childTrees;
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
            SyntaxTreeType treeType, int tokenIndex, TokenType startTokenType, TokenType stopTokenType)
        {
            var treeTokens = new List<Token>();
            for (var i = tokenIndex + 1; i < Tokens.Count; i++)
            {
                var token = Tokens[i];
                if (token.TokenType == stopTokenType)
                    return new SyntaxTree(treeType, treeTokens, this,
                        Token.DefaultStringForTokenTypes[startTokenType], Token.DefaultStringForTokenTypes[stopTokenType]);
                treeTokens.Add(token);
            }
            return new SyntaxTree(SyntaxTreeType.Text, treeTokens, this, Token.DefaultStringForTokenTypes[startTokenType]);
        }
    }
}
