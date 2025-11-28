using System.Text;

namespace Markdown
{
    internal class RenderProcess
    {
        internal string Render(List<Block> blocks)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < blocks.Count; i++)
            {
                foreach (var token in blocks[i].tokens)
                {
                    sb.Append(token.Text);
                }
                if (i != blocks.Count - 1)
                {
                    sb.Append('\n');
                }
            }
            return sb.ToString();
        }
    }
}