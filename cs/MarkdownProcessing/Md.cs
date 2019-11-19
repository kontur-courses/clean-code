using System;
using MarkdownProcessing.Converters;

namespace MarkdownProcessing
{
    public class Md
    {
        public static void Main()
        {
            var result = new MarkdownToTokenConverter("___Helloooo_____wooooorld!!!__")
                .ParseInputIntoTokens();
            Console.WriteLine(result);
        }
    }
}