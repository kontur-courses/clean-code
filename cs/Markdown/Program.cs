using System;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var md = new Md();
            //var f = md.Render("#hello world");
            //Console.WriteLine(f);
            //var a = @"__c\_e_t_f\_f__".SplitKp(new[] { '_', '\\', '#' }).UnionSameStringByTwo();
            //var b = @"__c\_e_t_f\_f__".SplitKeepSeparators(new[] { '_', '\\', '#' });
            foreach (var s in @"#_#aa".SplitKeepSeparators(new[] { '_', '\\', '#' }).UnionSameStringByTwo())
                Console.WriteLine(s);
            Console.WriteLine();

        }
    }
}
