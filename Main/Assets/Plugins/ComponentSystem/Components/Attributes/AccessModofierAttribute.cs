using System;

namespace ComponentSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AccessModofierAttribute : Attribute
    {
        public AccessModifier Value { get; private set; }

        public AccessModofierAttribute(AccessModifier value)
        {
            Value = value;
        }
    }
}
