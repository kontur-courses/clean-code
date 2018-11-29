using System.Collections.Generic;
using System.Text;

namespace MarkdownNew
{
    class Renderer : IMarkdownRenderer
    {
        private readonly Stack<Token> stack;
        private readonly Dictionary<Tag, Tag> pairs;
        private readonly StringBuilder renderedString;

        public Renderer()
        {
            stack = new Stack<Token>();
            renderedString = new StringBuilder();
            pairs = TagsPairsDictionary.GetTagsPairs();
        }

        public string Converter(string markdown)
        {
            MakeTokensFromMarkdown(markdown);
            while (stack.Count > 0)
            {
                var tmp = stack.Pop();
                if (stack.Count > 0)
                    stack.Peek().Content.Append(tmp.Tag.Open + tmp.Content);
                else
                {
                    renderedString.Append(tmp.Tag.Open + tmp.Content);
                }
            }
            return renderedString.ToString();
        }

        private void MakeTokensFromMarkdown(string markdown)
        {
            var usedTags = new List<string>();
            for (var index = 0; index < markdown.Length; index++)
            {
                var isNotToken = true;
                if (TrySkipShelding(markdown, index))
                {
                    index++;
                    continue;
                }
                var openTag = TryReadOpenTag(markdown, index, usedTags);
                var closeTag = TryReadCloseTag(markdown, index, usedTags);
                if (openTag != null)
                {
                    isNotToken = false;
                    index += openTag.Length - 1;
                }
                else if (closeTag != null)
                {
                    isNotToken = false;
                    index += closeTag.Length - 1;
                }
                if (!isNotToken) continue;
                if (stack.Count > 0)
                    stack.Peek().Content.Append(markdown[index]);
                else
                    renderedString.Append(markdown[index]);
            }
        }

        private string TryReadOpenTag(string markdown, int index, List<string> usedTags)
        {
            foreach (var tag in pairs.Keys)
            {
                if (tag.IsValidCloseTagFromPosition(markdown, index) 
                    && usedTags.Contains(tag.Open))
                {
                    RemoveTokenFromStack(tag, usedTags);
                    return tag.Open;
                }
            }
            return null;
        }

        private string TryReadCloseTag(string markdown, int index, List<string> usedTags)
        {
            foreach (var tag in pairs.Keys)
            {
                if (!tag.IsValidOpenTagFromPosition(markdown, index) 
                    || usedTags.Contains(tag.Open)) continue;
                index += tag.Close.Length - 1;
                stack.Push(new Token(index, tag));
                usedTags.Add(tag.Open);
                return tag.Close;
            }
            return null;
        }

        private bool TrySkipShelding(string markdown, int position)
        {
            if (markdown[position] != '\\' 
                || position + 1 >= markdown.Length) return false;
            if (stack.Count > 0)
                stack.Peek().Content.Append(markdown[position + 1]);
            else
                renderedString.Append(markdown[position + 1]);
            return true;
        }

        private void ReplaceTokenContent(Token token, bool isClosed)
        {
            var content = new StringBuilder();
            if (!isClosed)
                content.Append(token.Tag.Open + token.Content);
            else
            {
                var newTag = pairs[token.Tag];
                content.Append(newTag.Open + token.Content + newTag.Close);
            }
            if (stack.Count > 0)
                stack.Peek().Content.Append(content);
            else
                renderedString.Append(content);
        } 

        private void RemoveTokenFromStack(Tag tag, List<string> usedTags)
        {
            while (stack.Count > 0 && usedTags.Contains(tag.Open))
            {
                var token = stack.Pop();
                usedTags.Remove(token.Tag.Open);
                ReplaceTokenContent(token, tag.Open == token.Tag.Open);
            }
        }
    }
}
