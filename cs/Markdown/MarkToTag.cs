using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class MarkToTag
    {
        // Вынесено в отдельный класс, чтобы проще было изменять, если
        // вдруг понадобится расширить Mark или Tag. Вероятно, будет использоваться в 
        // Md.GetRenderedInput().
        public static string GetTag(Mark mark)
        {
            throw new NotImplementedException();
        }
    }
}
