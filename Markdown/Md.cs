using System;

namespace Markdown
{
    public class Md
    {
        private Tokenizer tokenizer = new Tokenizer();
        private List<IBlockProcessor> blockProcessors = new List<IBlockProcessor>
        {
            new HeaderProcessor()
        };
        private List<ITokenProcessor> tokenProcessors = new List<ITokenProcessor> 
        { 
            new EscapeProcessor(),
            new EmphasisProcessor()
        };
        private RenderProcess renderProcess = new RenderProcess();

        public string Render(string input)
        {
            if (input == null) return null;
            var tokens = tokenizer.Tokenize(input);
            var blocks = new List<Block>();
            blocks.Add(new Block(tokens));
            foreach (var blockProcesor in blockProcessors)
            {
                blocks = blockProcesor.Process(blocks);
            }
            foreach (var tokenProcesor in tokenProcessors)
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    blocks[i] = new Block(tokenProcesor.Process(blocks[i].tokens));
                }
            }

            return renderProcess.Render(blocks);
        }
    }
}
