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
                bool isNotToken = true;
                if (TrySkipShelding(markdown, index))
                {
                    index++;
                    continue;
                }
                foreach(var tag in pairs.Keys)
                {
                    if (tag.IsValidCloseTagFromPosition(markdown, index))
                    {
                        if (!usedTags.Contains(tag.Open)) continue;
                        isNotToken = false;
                        index += tag.Open.Length - 1;
                        RemoveTokenFromStack(tag, usedTags, index);
                        continue;
                    }
                    else
                        if (tag.IsValidOpenTagFromPosition(markdown, index))
                        {
                            if (usedTags.Contains(tag.Open)) continue;
                            isNotToken = false;
                            index += tag.Close.Length - 1;
                            stack.Push(new Token(index, tag));
                            usedTags.Add(tag.Open);

                        }
                }

                if (isNotToken)
                {
                    if (stack.Count > 0)
                        stack.Peek().Content.Append(markdown[index]);
                    else
                    {
                        renderedString.Append(markdown[index]);
                    }
                }
            }

        }


        private bool TrySkipShelding(string markdown, int position)
        {
            if (markdown[position] == '\\' && position + 1 < markdown.Length)
            {
                if (stack.Count > 0)
                    stack.Peek().Content.Append(markdown[position + 1]);
                else
                    renderedString.Append(markdown[position + 1]);
                return true;
            }
            return false;
        }

        private void ReplaceTokenContent(Token token, bool isClosed)
        {
            var content = new StringBuilder();
            if (isClosed)
            {
                var newTag = pairs[token.Tag];
                content.Append(newTag.Open + token.Content + newTag.Close);
            }
            else
            {
                content.Append(token.Tag.Open + token.Content);
            }
            if (stack.Count > 0)
            {
                stack.Peek().Content.Append(content);
            }
            else
            {
                renderedString.Append(content);
            }

            return;
        } 

        private void RemoveTokenFromStack(Tag tag, List<string> usedTags, int index)
        {
            while (stack.Count > 0 && usedTags.Contains(tag.Open))
            {
                var token = stack.Pop();
                usedTags.Remove(token.Tag.Open);
                if (tag.Open == token.Tag.Open)
                {
                    ReplaceTokenContent(token, true);
                }
                else
                {
                    ReplaceTokenContent(token, false);
                }
            }
        }
    }
}
