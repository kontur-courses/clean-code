using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenParser
    {
        public readonly List<Tag> Tags;
        private HashSet<int> usedIndices;

        public TokenParser(List<Tag> tags)
        {
            Tags = tags;
            usedIndices = new HashSet<int>();
        }

        public List<Token> ParseTokens(string text)
        {
            var tokens = FindAllTokens(text);
            tokens = RemoveIncorrectInterractedTokens(tokens);
            return tokens;
        }

        private List<Token> RemoveIncorrectInterractedTokens(List<Token> tokens)
        {
            return tokens
                .Where(t => 
                    t.Tag.TagInteractionRules
                        .All(rule => rule(t, tokens)))
                .ToList();
        }
        
        private List<Token> FindAllTokens(string text)
        {
            var tokens = new List<Token>();
            foreach (var tag in Tags)
            {
                var (startIndices, endIndices) = FindAllIndices(tag, text);
                var rightestIndex = -1;
                foreach (var openMarkInd in startIndices)
                {
                    if (openMarkInd <= rightestIndex) continue;
                    foreach (var closeMarkInd in endIndices)
                    {
                        if (closeMarkInd <= openMarkInd) continue;

                        var textStart = openMarkInd + tag.OpenMark.Length;
                        var textEnd = closeMarkInd;
                        var txt = text.Substring(textStart, textEnd - textStart);
                        var isPartial = text.TryGetChar(openMarkInd - 1, out var ch) && ch != ' ';
                        if (!tag.TextContentRules.All(f => 
                                f(tag, isPartial, txt)))
                            continue;
                        rightestIndex = closeMarkInd;
                        tokens.Add(new Token(tag, openMarkInd, closeMarkInd + tag.CloseMark.Length - 1));
                        break;
                    }
                }
            }
            return tokens;
        }

        private (List<int>, List<int>) FindAllIndices(Tag tag, string text)
        {
            var startIndices = FindUnusedIndices(text, tag.OpenMark);
            var endIndices = tag.OpenMark != tag.CloseMark ? 
                FindUnusedIndices(text, tag.CloseMark)
                : startIndices;

            if (tag.CloseMark == Environment.NewLine)
                endIndices.Add(text.Length);
            return (startIndices, endIndices);
        }

        private List<int> FindUnusedIndices(string text, string toFind)
        {
            var indices = text
                .AllIndicesOf(toFind)
                .Where(ind =>
                    !usedIndices.Contains(ind)
                    && text.CountOfCharBeforeIndex('\\', ind) % 2 == 0)
                .ToList();

            foreach (var index in indices)
                AddIndicesToUsed(index, toFind.Length);

            return indices;
        }

        private void AddIndicesToUsed(int startIndex, int length)
        {
            for (int i = startIndex; i < startIndex + length; i++)
                usedIndices.Add(i);
        }
    }
}
