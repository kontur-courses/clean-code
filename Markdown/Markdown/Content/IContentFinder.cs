using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Content
{
    interface IContentFinder
    {
        (int lenght, string content) GetBlockquoteContent();
        (int lenght, string content) GetCodeContent();
        (int lenght, string content) GetEmContent();
        (int lenght, string content) GetHeaderContent();
        (int lenght, string content) GetHorizontalContent();
        (int lenght, string content) GetListContent();
        (int lenght, string content) GetSimpleContent();
        (int lenght, string content) GetStrikeContent();
        (int lenght, string content) GetStrongContent();

    }
}
