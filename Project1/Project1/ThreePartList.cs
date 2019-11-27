using System.Collections;
using System.Collections.Generic;

namespace Programm
{
    class ThreePartList<T> : IEnumerable<T>
    {
        public List<T> Beginning = new List<T>();
        public List<T> Middle = new List<T>();
        public List<T> Ending = new List<T>();

        public ThreePartList()
        {
            ;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var x in Beginning)
                yield return x;
            foreach (var x in Middle)
                yield return x;
            foreach (var x in Ending)
                yield return x;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}