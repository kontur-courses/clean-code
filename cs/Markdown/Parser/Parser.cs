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

            if (current.PairType == TokenPairType.Open)
            {
                openingTokens.Push(current);
            }

            if (current.PairType == TokenPairType.Close)
            {
                if (openingTokens.Count != 0)
                {
                    var peek = openingTokens.Peek();

                    if (peek.Type == current.Type)
                    {
                        openingTokens.Pop();

                        return current;
                    }
                }

                current.Type = MdSpecification.Text;
                current.PairType = TokenPairType.NotPair;
            }

            parent.Children.Add(current);

            var result = BuildTree(current.PairType == TokenPairType.Open ? current : parent, str);

            if (result == null)
            {
                return current;
            }

            if (current.PairType == TokenPairType.Open)
            {
                if (result.PairType == TokenPairType.Close && current.Type == result.Type)
                {
                    parent.Children.Add(result);

                    return BuildTree(parent, str);
                }

                if (result.PairType != TokenPairType.Close)
                {
                    current.Type = MdSpecification.Text;
                    current.PairType = TokenPairType.NotPair;
                }
            }

            return result.PairType == TokenPairType.Close ? result : current;
        }
    }
}