using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class Renderer : IConverter
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

        public object Convert(object obj)
        {
            var markdown = obj as string;
            MakeTokensFromMarkdown(markdown);
            while (stack.Count > 0)
            {
                var tmp = stack.Pop();
                if (stack.Count > 0)
                    stack.Peek().Content.Append(tmp.Tag.Open + tmp.Content);
                else
                    renderedString.Append(tmp.Tag.Open + tmp.Content);
            }
            return renderedString.ToString();
        }

        private void MakeTokensFromMarkdown(string markdown)
        {
            var usedTags = new List<string>();
            for (var index = 0; index < markdown.Length; index++)
            {
                if (TryEscape(markdown, index))
                {
                    index++;
                    continue;
                }
                if (TryReadToken(markdown, index, usedTags, out var currentTag))
                {
                    index += currentTag.Length - 1;
                    continue;
                }
                if (stack.Count > 0)
                    stack.Peek().Content.Append(markdown[index]);
                else
                    renderedString.Append(markdown[index]);
            }
        }

        private bool TryReadToken(string markdown, int index, 
            List<string> usedTags, out string currentTag)
        {
            return TryReadCloseTag(markdown, index, usedTags, out currentTag) 
                   || TryReadOpenTag(markdown, index, usedTags, out currentTag);
        }

        private bool TryReadCloseTag(string markdown, int index, 
            List<string> usedTags, out string tagString)
        {
            tagString = "";
            foreach (var tag in pairs.Keys)
            {
                if (tag.IsValidCloseTagFromPosition(markdown, index) 
                    && usedTags.Contains(tag.Open))
                {
                    RemoveTokenFromStack(tag, usedTags);
                    tagString = tag.Open;
                    return true;
                }
            }
            return false;
        }

        private bool TryReadOpenTag(string markdown, int index, 
            List<string> usedTags, out string tagString)
        {
            tagString = "";
            foreach (var tag in pairs.Keys)
            {
                if (!tag.IsValidOpenTagFromPosition(markdown, index) 
                    || usedTags.Contains(tag.Open)) continue;
                index += tag.Close.Length - 1;
                stack.Push(new Token(index, tag));
                usedTags.Add(tag.Open);
                tagString = tag.Close;
                return true;
            }
            return false;
        }

        private bool TryEscape(string markdown, int position)
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
