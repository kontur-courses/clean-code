using System;

namespace Markdown
{
    public class Program
    {
        public static void Main()
        {
            var text1 = @"\_alp\\ha beta_";
            var text2 = "Текст, _окруженный с двух сторон_ одинарными символами подчерка,";
            var text3 = @"_alpha_ _beta";
            var text4 = @"Внутри __двойного выделения _одинарное_ тоже__ работает.";
            var text5 = @";s;sdlkf __dflskdjf__ lsdkfj";
            var md = new Md();
            md.Render(text1);
            md.Render(text2);
            md.Render(text3);
            md.Render(text4);
            md.Render(text5);

            throw new Exception();
        }
    }
}
