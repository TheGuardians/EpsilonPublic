using System;

namespace EpsilonLib.Core
{
    public class ExplicitViewAttribute : Attribute
    {
        public Type ViewType { get; }

        public ExplicitViewAttribute(Type viewType)
        {
            ViewType = viewType;
        }
    }
}
