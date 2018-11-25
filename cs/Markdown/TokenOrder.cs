using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Markdown
{
    public class TokenOrder : IEnumerable<ITokenMatcher>
    {
        public IEnumerator<ITokenMatcher> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}