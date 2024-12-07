using MarkDownConverter.Enums;
using MarkDownConverter.Interfaces;
using MarkDownConverter.Nodes;
using MarkDownConverter.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter
{
    public class MarkDownParser : IParser
    {
        private string doubleLowLine = "__";
        private string lowLine = "_";
        public ParentNode Parse(List<IToken> tokens)
        {
            var root = new ParentNode();
            ParseChildren(tokens, root, 0, tokens.Count);
            return root;
        }

        private void ParseChildren(List<IToken> tokens, INodeWithChildren parent, int left, int right)
        {
            if (left < 0 || right > tokens.Count) return;
            if (left >= right) return;
            var index = left;
            while (index >= left && index < right)
            {
                var token = tokens[index];
                switch (token)
                {
                    case SymbolsToken:
                    case SpaceToken:
                    case NewLineToken:
                    case NumbersToken:
                        parent.Children.Add(new SymbolsNode(token.Value));
                        index++;
                        break;
                    case HeadingToken:
                        {
                            var heading = new HeadingNode();
                            var next = FindIndexOfCloseHeadingToken(tokens, index);
                            ParseChildren(tokens, heading, index + 1, next == -1 ? right : next);
                            parent.Children.Add(heading);
                            index = next == -1 ? right : next;
                            break;
                        }
                    case ItalicToken:
                        {
                            index = ParseItalicWithChildren(tokens, parent, index, right);
                            break;
                        }
                    case BoldToken:
                        {
                            index = ParseBoldWithChildren(tokens, parent, index);
                            break;
                        }
                }
            }
        }

        private int ParseItalicWithChildren(List<IToken> tokens, INodeWithChildren parent, int start, int right)
        {
            var italic = new ItalicNode();
            var next = FindIndexOfCloseItalicToken(tokens, start);

            if (next == -1 || next >= right)
            {
                parent.Children.Add(new SymbolsNode(lowLine));
                return start + 1;
            }

            if (parent is ItalicNode)
            {
                parent.Children.Add(new SymbolsNode(lowLine));
                ParseChildren(tokens, parent, start + 1, next);
                parent.Children.Add(new SymbolsNode(lowLine));
                return next + 1;
            }

            if (TokenInWord(tokens, start) && TokenInWord(tokens, next) &&
                ContainsToken(tokens, TokenState.Space, start, next))
            {
                parent.Children.Add(new SymbolsNode(lowLine));
                for (var j = start + 1; j < next; j++) parent.Children.Add(new SymbolsNode(tokens[j].Value));
                parent.Children.Add(new SymbolsNode(lowLine));
                return next + 1;
            }

            ParseChildren(tokens, italic, start + 1, next);
            if (italic.Children.Count == 0)
            {
                parent.Children.Add(new SymbolsNode(lowLine + lowLine));
                return start + 2;
            }

            parent.Children.Add(italic);
            return next + 1;
        }

        private int ParseBoldWithChildren(List<IToken> tokens, INodeWithChildren parent, int i)
        {
            var bold = new BoldNode();
            var next = FindIndexOfCloseBoldToken(tokens, i);
            if (next == -1 || parent is ItalicNode)
            {
                parent.Children.Add(new SymbolsNode(doubleLowLine));
                return i + 1;
            }

            var indexOfIntersection = FindIndexOfIntersection(tokens, i + 1, next);
            if (indexOfIntersection.start > 0)
            {
                parent.Children.Add(new SymbolsNode(doubleLowLine));
                ParseChildren(tokens, parent, i + 1, indexOfIntersection.start);
                parent.Children.Add(new SymbolsNode(lowLine));
                ParseChildren(tokens, parent, indexOfIntersection.start + 1, next);
                parent.Children.Add(new SymbolsNode(doubleLowLine));
                ParseChildren(tokens, parent, next + 1, indexOfIntersection.end);
                parent.Children.Add(new SymbolsNode(lowLine));
                return indexOfIntersection.end + 1;
            }

            ParseChildren(tokens, bold, i + 1, next);
            if (bold.Children.Count == 0)
            {
                parent.Children.Add(new SymbolsNode(doubleLowLine + doubleLowLine));
                return i + 2;
            }

            parent.Children.Add(bold);
            return next + 1;
        }

        private int FindIndexOfCloseItalicToken(List<IToken> tokens, int start)
        {
            var index = start + 1;
            if (index < tokens.Count && tokens[index].Is(TokenState.Space)) return -1;
            while (index < tokens.Count && tokens[index].State != TokenState.NewLine)
            {
                if (!tokens[index].Is(TokenState.Italic))
                {
                    index++;
                    continue;
                }

                if (index + 1 < tokens.Count && tokens[index + 1].Is(TokenState.Italic))
                {
                    index += 2;
                    continue;
                }

                if (index > 0 && !tokens[index - 1].Is(TokenState.Space)) return index;
                index++;
            }

            return -1;
        }

        private int FindIndexOfCloseBoldToken(List<IToken> tokens, int start)
        {
            var index = start + 1;
            if (index >= tokens.Count || tokens[index].Is(TokenState.Space)) return -1;
            while (index < tokens.Count && tokens[index].State != TokenState.NewLine)
            {
                if (index > 0 && tokens[index].Is(TokenState.Bold) && !tokens[index - 1].Is(TokenState.Space))
                    return index;
                index++;
            }

            return -1;
        }

        private int FindIndexOfCloseHeadingToken(List<IToken> tokens, int start)
        {
            var index = start;
            while (index < tokens.Count && !tokens[index].Is(TokenState.NewLine))
                index++;
            return index == tokens.Count ? -1 : index;
        }

        private (int start, int end) FindIndexOfIntersection(List<IToken> tokens, int left, int right)
        {
            for (var i = left; i < right; i++)
                if (tokens[i] is ItalicToken)
                {
                    var end = FindIndexOfCloseItalicToken(tokens, i);
                    if (end > right) return (i, end);
                    if (end == -1) continue;
                    i = end + 1;
                }

            return (-1, -1);
        }

        private bool TokenInWord(List<IToken> tokens, int index)
            => index > 0 && tokens[index - 1].Is(TokenState.Symbols) && index + 1 < tokens.Count &&
               tokens[index + 1].Is(TokenState.Symbols);

        private bool ContainsToken(List<IToken> tokens, TokenState expected, int left, int right)
        {
            for (var i = left; i < right; i++)
                if (tokens[i].Is(expected))
                    return true;
            return false;
        }
    }
}
