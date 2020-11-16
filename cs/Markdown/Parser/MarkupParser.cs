using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown.Parser
{
    public class MarkupParser : IMarkupParser
    {
        private readonly Dictionary<string, ITagData> openBorderToTag;
        private readonly PrefixTree openTagTree;
        private readonly PrefixTree closeTagTree;
        
        private TextData fullTextData;
        private List<TextToken> currentTokens;
        
        public MarkupParser(params ITagData[] tags)
        {
            openBorderToTag = tags.ToDictionary(tag => tag.IncomingBorder.Open,
                tag => tag);

            openTagTree = new PrefixTree(tags
                .Select(tag => tag.IncomingBorder.Open).ToList());
            closeTagTree = new PrefixTree(tags
                .Select(tag => tag.IncomingBorder.Close).ToList());
        }

        public TextData Parse(string text)
        {
            fullTextData = new TextData(text);
            currentTokens = new List<TextToken>();
            
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                    AtLineEnd(i + 1);
                if (text[i] == '\\')
                    throw new NotImplementedException();
                var offset = TryToCloseTokenAndGetOffset(text, i);
                if (offset > 0)
                {
                    i += offset - 1;
                    continue;
                }

                offset = TryToOpenTokenAndGetOffset(text, i);
                if (offset > 0)
                    i += offset - 1;
            }
            
            AtLineEnd(text.Length);

            return fullTextData;
        }

        private void AtLineEnd(int position)
        {
            for (var i = currentTokens.Count - 1; i >= 0; i--)
            {
                var atLineEndTokenAction = currentTokens[i].Tag.AtLineEndAction;
                if (atLineEndTokenAction == EndOfLineAction.Continue)
                    break;
                
                switch (atLineEndTokenAction)
                {
                    case EndOfLineAction.Cancel:
                        currentTokens.RemoveAt(i);
                        break;
                    
                    case EndOfLineAction.Complete:
                        CloseToken(i, position);
                        break;
                }
            }
        }
        
        private int TryToOpenTokenAndGetOffset(string text, int position)
        {
            var openBorder = GetMatchesFromTree(openTagTree, text, position).LastOrDefault();
            if (openBorder == null)
                return 0;
            OpenToken(openBorder, position);
            return openBorder.Length;
        }

        private void OpenToken(string openBorder, int position)
        {
            var newToken = new TextToken(openBorderToTag[openBorder], position);
            currentTokens.Add(newToken);
        }

        private int TryToCloseTokenAndGetOffset(string text, int position)
        {
            var closeBorders = GetMatchesFromTree(closeTagTree, text, position);
            if (closeBorders.Count == 0)
                return 0;
            for (var i = currentTokens.Count - 1; i >= 0; i--)
            {
                var closeBorder = currentTokens[i].Tag.IncomingBorder.Close;
                if (closeBorders.Contains(closeBorder))
                {
                    CloseToken(i, position);
                    return closeBorder.Length;
                }
            }
            return 0;
        }

        private void CloseToken(int tokenNumber, int position)
        {
            CancelSubsequentTokens(tokenNumber);
            
            var token = currentTokens[tokenNumber];
            token.End = position;
            if (tokenNumber > 0)
            {
                currentTokens[tokenNumber - 1].AddNestedTokens(token);
            }
            else
            {
                fullTextData.AddTokens(token);
            }
            currentTokens.RemoveAt(tokenNumber);
        }

        private void CancelSubsequentTokens(int lastTokenNumber)
        {
            for (var i = currentTokens.Count - 1; i > lastTokenNumber; i--)
            {
                currentTokens[lastTokenNumber]
                    .AddNestedTokens(currentTokens[i].SubTokens.ToArray());
                currentTokens.RemoveAt(i);
            }
        }

        private static List<string> GetMatchesFromTree(PrefixTree tree, string text, int pos)
        {
            var matches = new List<string>();
            var currentTreeNode = tree.Root;
            while (currentTreeNode.Connections.ContainsKey(text[pos]))
            {
                currentTreeNode = currentTreeNode.Connections[text[pos]];
                if (currentTreeNode.IsFinishNode)
                    matches.Add(currentTreeNode.Value);
                pos++;
                if (pos == text.Length)
                    break;
            }

            return matches;
        }
    }
}