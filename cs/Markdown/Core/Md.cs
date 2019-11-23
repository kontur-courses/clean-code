using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Markdown.Core.Parsers;
using Markdown.Core.Rules;

namespace Markdown.Core
{
    public static class Md
    {
        public static string Render(string markdown)
        {
            var rules = RuleFactory.CreateAllRules();
            var sourceTags = rules.Select(rule => rule.SourceTag);
            var renderedLines = new List<string>();
            var parser = new MainParser(sourceTags);
            var render = new Render(rules);
            var escapedText = EscapeSpecialSymbols(markdown);
            foreach (var line in Regex.Split(escapedText, Environment.NewLine))
            {
                var tokens = parser.ParseLine(line);
                var renderedLine = render.RenderLine(line, tokens);
                renderedLines.Add(renderedLine);
            }

            return string.Join(Environment.NewLine, renderedLines);
        }

        private static string EscapeSpecialSymbols(string text)
        {
            var result = new StringBuilder(text);
            for (var i = 0; i < result.Length; i++)
            {
                if (result[i] == '>')
                {
                    result.Remove(i, 1);
                    result.Insert(i, "&gt;");
                }
                else if (result[i] == '<')
                {
                    result.Remove(i, 1);
                    result.Insert(i, "&lt;");
                }
            }

            return result.ToString();
        }
    }
}