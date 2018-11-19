namespace Markdown
{
    public class MarkdownParser
    {
        private string _input;
        
        public MarkdownParser(string input)
        {
            _input = input;
        }
        
        public MarkdownDocument Parse()
        {
            var document = new MarkdownDocument();
            ParseSegment(0, _input.Length, document.RootNode);
            return document;
        }

        private void ParseSegment(int startPos, int endPos, TreeNode parentNode)
        {
            var currentPos = startPos;
            TreeNode tagNode;
            Token tagToken;
            
            while (TryFindFirstTag(currentPos, endPos, parentNode, out tagNode, out tagToken))
            {
                Token leftTextToken;
                if (TokenReader.TryParseText(_input, currentPos, tagToken.Position, out leftTextToken))
                {
                    parentNode.AppendTextNode(_input.Substring(leftTextToken.Position, leftTextToken.Length));
                }

                parentNode.AppendNode(tagNode);
                
                currentPos = tagToken.Position + tagToken.Length;

                ParseSegment(
                    tagToken.Position + tagToken.ContentLeftOffset,
                    tagToken.Position + tagToken.ContentLeftOffset + tagToken.ContentLength,
                    tagNode);
            }
            
            Token rightTextToken;
            if (TokenReader.TryParseText(_input, currentPos, endPos, out rightTextToken))
            {
                parentNode.AppendTextNode(_input.Substring(rightTextToken.Position, rightTextToken.Length));
            }
        }

        private bool TryFindFirstTag(int begin, int end, TreeNode parentNode, out TreeNode node, out Token token)
        {
            node = new TreeNode();
            token = new Token();
            
            var currentPos = begin;
            while (currentPos < end)
            {
                if (TokenReader.TryParseNumberWithUnderlines(_input, currentPos, end, out token))
                {
                    currentPos = token.Position + token.Length;
                    continue;
                }
                
                if (TokenReader.TryParseDoubleUnderlineTag(_input, currentPos, end, out token))
                {
                    if (!parentNode.IsRoot && parentNode.Type == NodeType.SingleUnderlineTag)
                    {
                        currentPos = token.Position + token.Length;
                        continue;
                    }
                    
                    node.Type = NodeType.DoubleUnderlineTag;
                    return true;
                }

                if (TokenReader.TryParseSingleUnderlineTag(_input, currentPos, end, out token))
                {
                    node.Type = NodeType.SingleUnderlineTag;
                    return true;
                }

                currentPos++;
            }
            
            return false;
        }
    }
}