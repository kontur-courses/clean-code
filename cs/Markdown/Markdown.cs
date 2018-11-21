using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework.Constraints;

namespace Markdown
{
    public class Markdown
    {
        public readonly Dictionary<string, Tag> Tags;

        public Markdown()
        {
            Tags = new Dictionary<string, Tag>();
        }
        
        public string Render(string input,TokenSelector tokenSelector)
        {
            var tokenReader = new TokenReader(Tags);
            var tokens = tokenReader.ReadTokens(input);
            var tags = tokenSelector.SelectTokens(tokens);
            HandleTags(tags,new List<Tag>());
           
            return string.Join("",tokens.Select(x => x.Value));
        }

        public void HandleTags(IEnumerable<Token> tags, List<Tag> upperTags)
        {
            int OpenTagPos = 0;
            Token OpenTag = null;
            
            foreach (var tag in tags)
            {
                if (tag.IsOpen && tag.PosibleTag.IsValidTag(upperTags))
                {
                    OpenTag = tag;
                    break;
                }
                OpenTagPos++;
            }
            
            if(OpenTag == null)
                return;
            

            int CloseTagPos = OpenTagPos + 1;
            Token CloseTag = null;
            
            foreach (var tag in tags.Skip(OpenTagPos + 1))
            {
                if (tag.IsClose && tag.PosibleTag == OpenTag.PosibleTag)
                {
                    CloseTag = tag;
                    break;
                }
                CloseTagPos++;
            }

            if (CloseTag == null)
                HandleTags(tags.Skip(OpenTagPos + 1),upperTags);
            
            else
            {
                upperTags.Add(OpenTag.PosibleTag);
                HandleTags(tags.Skip(OpenTagPos + 1).Take(CloseTagPos - OpenTagPos - 1),upperTags);
                upperTags.RemoveRange(upperTags.Count - 1,1);
                HandleTags(tags.Skip(CloseTagPos + 1),upperTags);
                OpenTag.Value = "<" + OpenTag.PosibleTag.HtmlRepresentation + ">";
                CloseTag.Value = "</" + CloseTag.PosibleTag.HtmlRepresentation + ">";
            }

        }

    }
}