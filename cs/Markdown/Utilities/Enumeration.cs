using System;

namespace Markdown
{
    public abstract class Enumeration : IComparable
    {
        public int Id { get; }
        public string Name { get; }

        public Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object other)
        {
            throw new NotImplementedException();
        }
    }
}
