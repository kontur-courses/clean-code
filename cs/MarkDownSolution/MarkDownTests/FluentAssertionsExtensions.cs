using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace MarkDownTests
{
    public static class FluentAssertionsExtensions
    {
        public static void BeTheSameAs(this ObjectAssertions should, object Object)
        {
            should.BeEquivalentTo(Object, p => p.IgnoringCyclicReferences());
        }
    }
}
