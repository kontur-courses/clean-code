using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class Component
    {
        public virtual void Add(Component component)
        {
            throw new NotImplementedException();
        }

       public abstract string GetStringRepresentation();
    }
    
    
    public class Leaf : Component
    {
        private readonly string value;

        public Leaf(string value)
        {
            this.value = value;
        }

        public override string GetStringRepresentation() => throw new NotImplementedException();
    }


    public class Tree : Component
    {
        private readonly string openTag;
        private readonly string closeTag;
        private readonly List<Component> children = new();

        public Tree(string openTag, string closeTag)
        {
            this.openTag = openTag;
            this.closeTag = closeTag;
        }

        public override void Add(Component component)
        {
            throw new NotImplementedException();
        }

        public override string GetStringRepresentation() => throw new NotImplementedException();
    }
}