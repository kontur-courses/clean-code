using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.DataStructures;

namespace Markdown.Logic
{
    public class HTMLBuilder : IHTMLBuilder
    {
        public string Build(string text, TokensTree tree, List<int> escapeSymbolsIndexes)
        {
            StringBuilder builder = new StringBuilder();
            Dfs(text, tree.RootToken, escapeSymbolsIndexes, builder);
            return builder.ToString();
        }

        public void Dfs(string text, Token token, List<int> escapeSymbolsIndexes, StringBuilder builder)
        {
            if (token.Tag != null)
            builder.Append(token.Tag.OpeningTag);
            for (int i = token.StartIndex; i <= token.EndIndex; i++)
            {
                if (escapeSymbolsIndexes.Contains(i))
                    continue;
                if (token.Children.Any(c => c.StartIndex - c.Tag.MarkdownName.Length == i))
                {
                    var f = token.Children.First(c => c.StartIndex - c.Tag.MarkdownName.Length == i);
                    Dfs(text, f, escapeSymbolsIndexes, builder);
                    i = f.EndIndex + f.Tag.MarkdownName.Length;
                }
                else
                    builder.Append(text[i]);
            }

            if (token.Tag != null)
                builder.Append(token.Tag.ClosingTag);
        }
    }
}