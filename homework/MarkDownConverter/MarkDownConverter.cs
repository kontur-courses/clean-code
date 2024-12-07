using MarkDownConverter.Interfaces;
using MarkDownConverter.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter
{
    public class MarkDownConverter(ITokenizer tokenizer, IParser parser)
    {
        public ITokenizer Tokenizer { get; } = tokenizer;
        public IParser Parser { get; } = parser;

        public string Convert(string input)
        {
            var tokens = Tokenizer.Tokenize(input);
            var node = Parser.Parse(tokens);
            return ConvertNodeToHtml(node);
        }

        private string ConvertNodeToHtml(ParentNode node)
        {
            var output = new StringBuilder();
            CreateHtml(node, output);
            return output.ToString();
        }

        private void CreateHtml(Node node, StringBuilder html)
        {
            switch (node)
            {
                case ParentNode patent:
                    {
                        foreach (var child in patent.Children)
                            CreateHtml(child, html);
                        break;
                    }

                case HeadingNode headingNode:
                    html.Append("<h1>");
                    foreach (var child in headingNode.Children)
                        CreateHtml(child, html);
                    html.Append("</h1>");
                    break;

                case SymbolsNode textNode:
                    html.Append(textNode.Value);
                    break;

                case ItalicNode italicNode:
                    html.Append("<em>");
                    foreach (var child in italicNode.Children)
                        CreateHtml(child, html);
                    html.Append("</em>");
                    break;

                case BoldNode boldNode:
                    html.Append("<strong>");
                    foreach (var child in boldNode.Children)
                        CreateHtml(child, html);
                    html.Append("</strong>");
                    break;
            }
        }
    }
}
