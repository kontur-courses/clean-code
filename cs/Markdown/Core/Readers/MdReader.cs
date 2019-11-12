using System.Collections.Generic;

namespace Markdown
{
    public class MdReader
    {
        private int currentPosition;
        private readonly string source;
        private readonly MdTokenReader fieldReader;
        private readonly List<IMdToken> fields;
        
        public MdReader(string source)
        {
            this.source = source;
            fieldReader = new MdTokenReader();
            fields = new List<IMdToken>();
        }

        public List<IMdToken> ReadMdTokens()
        {
            while (IsReadingAvailable())
            {
                var field = fieldReader.ReadField(source, currentPosition);
                fields.Add(field);
                currentPosition += field.Length;
            }
            return fields;
        }

        private bool IsReadingAvailable()
        {
            return currentPosition < source.Length;
        }
    }
}