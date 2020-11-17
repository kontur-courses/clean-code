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
                        CancelSubsequentTokens(i - 1);
                        break;
                    
                    case EndOfLineAction.Complete:
                        if (IsValidToClose(i, position))
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
            
            var tag = openBorderToTag[openBorder];
            
            if (!IsTagAllowed(tag))
                return 0;
            
            OpenToken(tag, position);
            return openBorder.Length;

        }

        private bool IsValidToOpen(ITagData tag, int startPosition)
        {
            return IsTagAllowed(tag) && tag.IsValidAtOpen(fullTextData.Value, startPosition);
        }

        private bool IsTagAllowed(ITagData tag)
        {
            foreach (var token in currentTokens)
            {
                if (tag == token.Tag || !token.Tag.CanNested(tag))
                    return false;
            }
            return true;
        }

        private void OpenToken(ITagData tag, int position)
        {
            var newToken = new TextToken(tag, position);
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
                    if (IsValidToClose(i, position))
                    {
                        CloseToken(i, position);
                        return closeBorder.Length;
                    }
                }
            }
            return 0;
        }

        private bool IsValidToClose(int tokenNumber, int endPosition)
        {
            return currentTokens[tokenNumber].Tag.IsValidAtClose(fullTextData.Value,
                currentTokens[tokenNumber].Start, endPosition);
        }

        private void CloseToken(int tokenNumber, int endPosition)
        {
            CancelSubsequentTokens(tokenNumber);
            
            var token = currentTokens[tokenNumber];
            token.End = endPosition;
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
            if (lastTokenNumber < 0)
                for (var i = currentTokens.Count - 1; i >= 0; i--)
                {
                    fullTextData.AddTokens(currentTokens[i].SubTokens.ToArray());
                    currentTokens.RemoveAt(i);
                }
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