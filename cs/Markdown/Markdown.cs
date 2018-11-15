using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class Markdown
    {
        private Dictionary<string, string> tokenToTags;

        public Markdown()
        {
            tokenToTags = new Dictionary<string, string>(); 
        }
        
        public void AddNewTag(string pref, string tag)
        {
            tokenToTags[pref] = tag;
        }
        
        public string Render(string input)
        {
            var tokenReader = new TokenReader(tokenToTags.Keys);
            var tokens = tokenReader.ReadTokens(input).ToList();
            tokens = tokens.Take(tokens.Count - 1).ToList();

            foreach (var token in tokens)
            {
                Console.WriteLine(token.Value);
            }

            var tokenStacks = new Dictionary<string, Stack<Token>>();
            foreach (var key in tokenToTags.Keys)
                tokenStacks[key] = new Stack<Token>();

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].IsTaged)
                {
                    if (tokens.IsOpen(i))
                        tokenStacks[tokens[i].Value].Push(tokens[i]);
                    else if (tokens.IsClose(i))
                    {
                        if (tokenStacks[tokens[i].Value].Count != 0)
                        {
                            var rtoken = tokenStacks[tokens[i].Value].Pop();
                            rtoken.Value = "<" + tokenToTags[rtoken.Value] + ">";
                            tokens[i].Value = "</" + tokenToTags[tokens[i].Value] + ">";
                        }
                    }
                    
                }
            }
            return string.Join("",tokens.Select(x => x.Value));
        }

       
        
    }


}