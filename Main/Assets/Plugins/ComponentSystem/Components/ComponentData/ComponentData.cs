using System;

namespace ComponentSystem
{
    public struct ComponentData
    {
        public static AccessModifier DefaultAccessModifier => AccessModifier.Everything;

        public AccessModifier @AccessModifier => _accessModifier;

        private AccessModifier _accessModifier;
        private Type _abstractRepresentation;

        public ComponentData(AccessModifier accessModifier, Type abstractRepresentation)
        {
            _accessModifier = accessModifier;

            _abstractRepresentation = abstractRepresentation;
        }

        public Type GetAbstractRepresentation() => _abstractRepresentation;
    }
}
