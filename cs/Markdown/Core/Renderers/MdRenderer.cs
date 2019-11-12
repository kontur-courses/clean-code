using System.Text;

namespace Markdown
{
    public class MdRenderer
    {   
        private readonly MdTokenRenderer fieldRenderer = new MdTokenRenderer();
        private readonly MdNormalizer normalizer = new MdNormalizer();
        
        public string Render(string source)
        {
            var mdReader = new MdReader(source);
            var result = new StringBuilder();
            var fields = mdReader.ReadMdTokens();
            fields = normalizer.NormalizeFields(fields);
            foreach (var field in fields)
            {
                result.Append(fieldRenderer.RenderField(field));
            }
            return result.ToString();
        }
    }
}