using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new Markdown().Render("# _s __dsf__ f_ \n" +
                                                    "* _s # d_\n" +
                                                    "* _s # d_\n" +
                                                    "* _s # d_\n" +
                                                    "__asf__\n"+
                                                    "## _sdf __sdf_ sadf__"));
        }
    }
}
