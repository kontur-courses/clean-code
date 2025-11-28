namespace Markdown
{
    internal class HeaderProcessor : IBlockProcessor
    {
        public List<Block> Process(List<Block> blocks)
        {
            var result = new List<Block>();
            foreach(var block in blocks)
            {
                result.Add(new Block(new List<Token>()));
                for (int i = 0; i < block.tokens.Count; i++) {
                    if (block.tokens[i].Type == TokenType.NewLine)
                    {
                        result.Add(new Block(new List<Token>()));
                    }
                    else if(block.tokens[i].Type == TokenType.Grid &&
                        i != block.tokens.Count -1 &&
                        block.tokens[i+1].Type == TokenType.Whitespace &&
                        result[result.Count - 1].tokens.Count == 0)
                    {
                        result[result.Count - 1].tokens.Add(new Token(TokenType.Tag, false, false, "<h1>"));
                        i++; // пропуск пробела
                    }
                    else
                    {
                        result[result.Count - 1].tokens.Add(block.tokens[i]);
                    }
                }
            }
            for(int block = 0; block < result.Count; block++)
            {
                if (result[block].tokens[0].Type == TokenType.Tag &&
                    result[block].tokens[0].Text == "<h1>")
                {
                    result[block].tokens.Add(new Token(TokenType.Tag, false, false, "</h1>"));
                }
            }
            return result;
        }
    }
}