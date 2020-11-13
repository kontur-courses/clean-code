using System;

namespace Markdown
{
    public class MdRawTextToken : MdToken
    {
        public override string Parse(MdTokenReader reader) => reader.Text.Substring(StartPosition, Length);

        public static bool TryRead(MdTokenReader reader, out MdToken result)
        {
            throw new NotImplementedException();
        }
    }
}