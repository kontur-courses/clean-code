using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownNew
{
    class Renderer : IMarkdownRenderer
    {
        private List<Token> tokens;
        private Stack<Token> stack;
        private Dictionary<Tag, Tag> pairs;
        public Renderer()
        {
            tokens = new List<Token>();
            stack = new Stack<Token>();
            pairs = TagsPairsDictionary.GetTagsPairs();
        }

        public string ConvertFromMarkdownToHtml(string markdown)
        {
            MakeTokensFromMarkdown(markdown);
            return MakeStringFromTokens(markdown);
        }

        private void MakeTokensFromMarkdown(string markdown)
        {
            var usedTags = new List<string>();
            for (var index = 0; index < markdown.Length; index++)
            {
                foreach(var tag in pairs.Keys)
                {
                    if (tag.IsValidCloseTagFromPosition(markdown, index))
                    {
                        if (!usedTags.Contains(tag.open)) continue;
                        RemoveTokenFromStack(tag, usedTags, index);
                        continue;
                    }
                    if (tag.IsValidOpenTagFromPosition(markdown, index))
                    {
                        if (usedTags.Contains(tag.open)) continue;
                        stack.Push(new Token(index, tag));
                        usedTags.Add(tag.open);
                    }
                }
            }

        }

        private void RemoveTokenFromStack(Tag tag, List<string> usedTags, int index)
        {
            while (stack.Count > 0 && usedTags.Contains(tag.open))
            {
                var token = stack.Pop();
                usedTags.Remove(token.StartTag.open);
                if (tag.open == token.StartTag.open)
                {
                    token.SetEndOfToken(index);
                    tokens.Add(token);
                }
            }
        }

        private string MakeStringFromTokens(string markdown)
        {
            var resultString = new StringBuilder();
            for (var index = 0; index < markdown.Length; index++)
            {
                bool isTag = false;
                foreach (var token in tokens)
                {
                    if (token.Start == index)
                    {
                        isTag = true;
                        index += token.StartTag.open.Length - 1;
                        var htmlTag = pairs[token.StartTag].open;
                        resultString.Append(htmlTag);
                    }
                    if (token.End == index)
                    {
                        isTag = true;
                        index += token.StartTag.close.Length - 1;
                        var htmlTag = pairs[token.StartTag].close;
                        resultString.Append(htmlTag);
                    }
                }
                if (!isTag)
                    resultString.Append(markdown[index]);

            }
            return resultString.ToString();
        }
    }
}
