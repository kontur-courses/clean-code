using System;
using System.Collections.Generic;
using Markdown.Md;
using Markdown.Md.TagHandlers;

namespace Markdown
{
    public class Parser : IParser
    {
        private readonly TagHandler tagHandler;
        private readonly Stack<TokenNode> openingTokens;
        private int position;

        public Parser(TagHandler tagHandler)
        {
            openingTokens = new Stack<TokenNode>();
            this.tagHandler = tagHandler;
        }

        public ITokenNode Parse(string str)
        {
            if (str == null)
            {
                throw new ArgumentException("Given string can't be null", nameof(str));
            }

            var root = new TokenNode("root", "", TokenPairType.NotPair);
            BuildTree(root, str);

            return root;
        }

        private TokenNode BuildTree(ITokenNode parent, string str)
        {
            if (position >= str.Length)
            {
                return null;
            }

            var current = tagHandler.Handle(str, position, openingTokens);
            position += current.Value.Length;

            if (current.PairType == TokenPairType.Close)
            {
                return current;
            }

            parent.Children.Add(current);

            var recursiveParentNode = parent;

            if (current.PairType == TokenPairType.Open)
            {
                recursiveParentNode = current;
            }

            var result = BuildTree(recursiveParentNode, str);

            if (result == null)
            {
                return current;
            }

            if (current.PairType == TokenPairType.Open)
            {
                if (current.PairType != result.PairType && current.Type == result.Type)
                {
                    parent.Children.Add(result);

                    return BuildTree(parent, str);
                }

                if (current.PairType != result.PairType)
                {
                    current.Type = MdSpecification.Text;
                    current.PairType = TokenPairType.NotPair;
                }
            }

            return result.PairType == TokenPairType.Close ? result : current;
        }
    }
}