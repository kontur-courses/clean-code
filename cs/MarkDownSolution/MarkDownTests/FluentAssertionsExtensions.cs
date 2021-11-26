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
