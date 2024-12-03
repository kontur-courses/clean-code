using Markdown.HTMLConverter;
using Markdown.MDParser;
using System.Text;

namespace Markdown
{
    internal class Program
    {
        public static void FindChildren(Token token)
        {
            if (!Complex.Properties.Contains(token.Property))
                return;

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
                            var italic = new Italic();
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
                            var hidden = new Hidden();
                            var child = hidden.TryFindToken(text, i);
                            i += child.Value.Length + 1;
                            if (child.Value == string.Empty)
                                child.Value = "\\";
                            token.Children.Add(child);
                            break;
                        }

                    default:
                        {
                            var norm = new Normal();
                            var child = norm.TryFindToken(text, i);
                            token.Children.Add(child);
                            i += child.Value.Length;
                            break;
                        }
                }
            }
        }

        public static string PrintToken(Token token)
        {
            var result = new StringBuilder();
            var stack = new Stack<Token>();
            var closings = new Stack<string>();
            stack.Push(token);
            result.Append(Tags.Open[token.Property]);
            closings.Push(Tags.Close[token.Property]);
            while (stack.Count != 0)
            {
                var current = stack.Pop();
                foreach (var next in current.Children)
                {
                    result.Append(Tags.Open[next.Property]);
                    if (next.Property is TokenProperty.Normal or TokenProperty.Italic)
                    {
                        result.Append(next.Value);
                        result.Append(Tags.Close[next.Property]);
                    }
                    else closings.Push(Tags.Close[next.Property]);
                    stack.Push(next);
                }
            }
                while (closings.Count != 0)
                    result.Append(closings.Pop());
            return result.ToString();
        }
        static void Main()
        {
            var text = "Внутри __двойного выделения _одинарное_ тоже__ работает";

            var parser = new Parser();
            var dom = parser.BuildDom(text);
            var converter = new Converter();

            var result = new StringBuilder();
            result.Append(converter.Convert(dom));
            Console.WriteLine(result);

        }
    }
}
