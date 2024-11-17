using System;

namespace ComponentSystem
{
    [Flags]
    public enum AccessModifier
    {
        None = 0b00,
        Inside = 0b01,
        Outside = 0b10,
        Everything = 0b11
    }
}
