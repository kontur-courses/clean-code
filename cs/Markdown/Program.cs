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
            parser.registerLocalReader(new EmRegister());

            var result = parser.Parse("_some __dick_ is__");

            Console.WriteLine(result);

        }
    }
}
