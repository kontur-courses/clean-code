using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDParser
{
    public class Parser : IParser
    {
        public DOM BuildDom(string text)
        {
            var tokens = text.Split("\n", StringSplitOptions.None)
                    .Select(line => new Token(line, TokenProperty.Paragraph))
                    .Select(t => t = t.Value.StartsWith('#') ? new Token(t.Value[1..], TokenProperty.Head) : t)
                    .ToList()
                ;

            foreach (var token in tokens)
            {
                var queue = new Queue<Token>();
                queue.Enqueue(token);
                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    FindChildren(current);
                    foreach (var child in current.Children)
                        queue.Enqueue(child);
                }
            }
            return new DOM(tokens);
        }

        private static void FindChildren(Token token)
        {
            if (!Complex.Properties.Contains(token.Property))
                return;
            var hidden = new Hidden();
            var italic = new Italic();
            var norm = new Normal();
            var link = new Link();

            var i = 0;
            var text = token.Value;
            var len = token.Value.Length;
            while (i < len)
            {
                var start = text[i];
                switch (start)
                {
                    case '_':
                        {
                            var child = italic.TryFindToken(text, i);
                            token.Children.Add(child);

                            i += child.Property switch
                            {
                                TokenProperty.Italic => child.Value.Length + 2,
                                TokenProperty.Bold => child.Value.Length + 4,
                                _ => child.Value.Length,
                            };
                            break;
                        }

                    case '\\':
                        {
                            var child = hidden.TryFindToken(text, i);
                            i += child.Value.Length + 1;
                            if (child.Value == string.Empty)
                                child.Value = "\\";
                            token.Children.Add(child);
                            break;
                        }
                    case '[':
                        {
                            var child = link.TryFindToken(text, i);
                            token.Children.Add(child);
                            i += child.Property switch
                            {
                                TokenProperty.Link => child.Value.Length + 1,
                                _ => 1
                            };
                            break;
                        }

                    default:
                        {
                            var child = norm.TryFindToken(text, i);
                            token.Children.Add(child);
                            i += child.Value.Length;
                            break;
                        }
                }
            }
        }
    }
}
