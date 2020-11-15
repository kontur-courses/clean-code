using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown.Parser
{
    public class MarkupParser : IMarkupParser
    {
        private readonly Dictionary<FormattingState, ITagData> stateToTag;
        private readonly Dictionary<string, ITagData> openKeyToTag;
        private readonly PrefixTree openTagTree;
        private readonly PrefixTree closeTagTree;

        private Stack<FormattingState> currentStates = new Stack<FormattingState>();
        private TextToken currentToken;
        
        public MarkupParser(params ITagData[] tags)
        {
            stateToTag = tags.ToDictionary(tag => tag.State,
                tag => tag);

            openKeyToTag = tags.ToDictionary(tag => tag.IncomingBorder.Open,
                tag => tag);

            openTagTree = new PrefixTree(tags
                .Select(tag => tag.IncomingBorder.Open).ToList());
            closeTagTree = new PrefixTree(tags
                .Select(tag => tag.IncomingBorder.Close).ToList());
        }

        public List<TextToken> Parse(string text)
        {
            currentStates.Clear();
            currentStates.Push(FormattingState.NoFormatting);
            for (var i = 0; i < text.Length; i++)
            {
                TryToCloseTag(text, i);
            }
            throw new NotImplementedException();
        }

        private bool TryToCloseTag(string text, int pos)
        {
            var closeBorder = GetBestMatchFromTree(closeTagTree, text, pos);
            if (closeBorder.Length > 0 && currentStates.Peek() != FormattingState.NoFormatting)
            {
                var currTag = stateToTag[currentStates.Peek()];
                if (currTag.IncomingBorder.Close == closeBorder)
                {
                    currentToken.TokenState = currentStates.Pop();
                }
            }

            return false;
        }

        private static string GetBestMatchFromTree(PrefixTree tree, string text, int pos)
        {
            string bestMatch = null;
            var currentTreeNode = tree.Root;
            while (currentTreeNode.Connections.ContainsKey(text[pos]))
            {
                currentTreeNode = currentTreeNode.Connections[text[pos]];
                if (currentTreeNode.IsFinishNode)
                    bestMatch = currentTreeNode.Value;
                pos++;
            }

            return bestMatch;
        }
    }
}