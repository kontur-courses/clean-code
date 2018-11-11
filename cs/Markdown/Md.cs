using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var result = new StringBuilder(markdown);

//            var allIndexesOf = markdown.AllIndexesOf("_");
//            if (allIndexesOf.Count == 0)
//                return markdown;

            var emOpened = false;
            var strongOpened = false;

            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == '_')
                {
                    if (i == 0 || (i - 1 >= 0 && result[i - 1] != '\\'))
                    {
                        if (result[i + 1] == '_')
                        {
                            result.Remove(i, 2);
                            result.Insert(i, strongOpened ? "</strong>" : "<strong>");
                            strongOpened = !strongOpened;


                        }
                        else
                        {
                            var check = i - 1 >= 0;
                            if (i == 0 || check && result[i - 1] != '_')
                            {
                                result.Remove(i, 1);
                                result.Insert(i, emOpened ? "</em>" : "<em>");
                                emOpened = !emOpened;
                            }
                        }
                    }
                }
            }

            return result.ToString();
        }
    }
}
