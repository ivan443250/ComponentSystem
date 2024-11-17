using System;

namespace ComponentSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AbstractRepresentationAttribute : Attribute
    {
        public Type Value { get; private set; }

        public AbstractRepresentationAttribute(Type value)
        {
            Value = value;
        }
    }
}
