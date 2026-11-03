
using System.Text;

namespace Markdown
{
    public class Md
    {
        // _aaa_ - > <em>aaa</em>
        // __aaa__ -> <strong>aaa</strong>
        // #aaa -> <h1>aaa</h1>


        public Dictionary<string, string> _replacements = new Dictionary<string, string>
        {
            { "__", "<strong>" },
            { "_", "<em>" },
            { "#", "<h1>" }
        };


        Stack<string> _tagStack = new Stack<string>();


        public string Render(string markdown)
        {
            var result = new StringBuilder();

            var index = 0;

            while (index < markdown.Length)
            {
                if (markdown[index] == '#')
                {
                    result.Append(RenderH1(markdown, ref index));
                    continue;
                }

                if (index + 1 < markdown.Length && markdown[index + 1] == '_' && markdown[index] == '_')
                {
                    result.Append(RenderStrong(markdown, ref index));
                    continue;
                }

                if (markdown[index] == '_')
                {
                    result.Append(RenderEmphasis(markdown, ref index));
                    continue;
                }

                else
                {
                    result.Append(markdown[index]);
                }

                index++;
            }
            return result.ToString();
        }

        private void CheckSymbol(string markDown, ref int index)
        {
            
        }

        private StringBuilder RenderH1(string markdown, ref int index)
        {
            var result = new StringBuilder();

            _tagStack.Push("#");

            index++; // Skip the '#' character
            result.Append("<h1>");

            while (index < markdown.Length && markdown[index] != '\n')
            {
                result.Append(markdown[index]);
                index++;
            }

            result.Append("</h1>");
            _tagStack.Pop();

            return result;

        }


        public StringBuilder RenderEmphasis(string markdown, ref int index)
        {
            var result = new StringBuilder();
            _tagStack.Push("_");
            index++; // Skip the '_' character
            result.Append("<em>");
            while (index < markdown.Length && markdown[index] != '_')
            {
                result.Append(markdown[index]);
                index++;
            }
            result.Append("</em>");
            _tagStack.Pop();
            index++; // Skip the closing '_' character
            return result;
        }

        private StringBuilder RenderStrong(string markdown, ref int index)
        {
            var result = new StringBuilder();
            _tagStack.Push("__");
            index += 2; // Skip the '__' characters
            result.Append("<strong>");
            while (index < markdown.Length && !(markdown[index] == '_' && markdown[index + 1] == '_'))
            {
                result.Append(markdown[index]);
                index++;
            }
            result.Append("</strong>");
            _tagStack.Pop();
            index+=2;
            return result;
        }


        private StringBuilder RenderSpecSymbol(string markdown, ref int index)
        {
            var result = new StringBuilder();
            return result;

        }
    } 

}
