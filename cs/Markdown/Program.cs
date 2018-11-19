using System;
using System.Linq;
using System.Threading.Tasks;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO Обернуть в try case

            var parser = new Md();

            parser.registerGlobalReader(new ParagraphRegister());
            parser.registerGlobalReader(new HorLineRegister());

            parser.registerLocalReader(new StrongRegister());

            var result = parser.Parse("some __surface__\n***\n in my life");

            Console.WriteLine(result);

        }
    }
}
