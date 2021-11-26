using System;

namespace Markdown
{
    internal class OneTimeSettedValue<T>
    {
        private T? value;
        public bool Setted { get; private set; }

        public void SetValue(T valueToSet)
        {
            if (Setted) throw new InvalidOperationException("can not set value, when it is already setted");
            value = valueToSet;
            Setted = true;
        }

        public T GetValue()
        {
            if (value is null) throw new InvalidOperationException("can not get value, when it is not setted");
            return value;
        }
    }
}