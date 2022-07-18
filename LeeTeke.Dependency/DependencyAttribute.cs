using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeeTeke.Dependency
{
    //
    // 摘要:
    //     An attribute that indicates that the specified type provides a concrete implementation
    //     of a needed interface.
    //
    // 言论：
    //     To be added.
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class DependencyAttribute : Attribute
    {
        internal Type Implementor
        {
            get;
            private set;
        }

        public DependencyAttribute(Type implementorType)
        {
            Implementor = implementorType;
        }
    }
}
