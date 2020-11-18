using System.Text;

namespace Markdown
{
    public class StrongTokenConverter : TagTokenConverter
    {
        public StrongTokenConverter()
        {
            OpenTag = "<strong>";
            CloseTag = "</strong>";
        }
    }
}