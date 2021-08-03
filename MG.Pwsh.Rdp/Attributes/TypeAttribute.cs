using MG.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MG.Pwsh.Rdp.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class TypeAttribute : Attribute, IValueAttribute
    {
        private Type _storedType;

        public Type Value => _storedType;
        object IValueAttribute.Value => this.Value;

        public TypeAttribute(Type type)
            : base()
        {
            _storedType.GetTypeInfo().TypeInitializer.
            _storedType = type;
        }

        public T GetAs<T>() => throw new NotImplementedException();
        public bool ValueIsString() => false;
    }
}
