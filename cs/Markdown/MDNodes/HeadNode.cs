using Markdown.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDElements
{
    public class HeadNode : MarkdownNode
    {

        //Отдает работа детям, если нет детей должен возращаеть пустую строку
        // Делает то же самое что и в PairedNode, нарушаем dry,
        // но не критично в этом случае.
        // Если будем расширят набор подобных символов,
        // то можно вынести этот метод в MarkdownNode
        //
        public override string Render()
        {
            throw new NotImplementedException();
        }
    }
}
