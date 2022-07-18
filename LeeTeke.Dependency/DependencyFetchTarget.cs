using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leeteke.Dependency
{
    //
    // 摘要:
    //     Enumeration specifying whether Xamarin.Forms.DependencyService.Get``1(Xamarin.Forms.DependencyFetchTarget)
    //     should return a reference to a global or new instance.
    //
    // 言论：
    //     The following example shows how Xamarin.Forms.DependencyFetchTarget can be used
    //     to specify a new instance:
    //     var secondFetch = DependencyService.Get<IDependencyTest> (DependencyFetchTarget.NewInstance);
    public enum DependencyFetchTarget
    {
        //
        // 摘要:
        //     Return a global instance.
        GlobalInstance,
        //
        // 摘要:
        //     Return a new instance.
        NewInstance
    }
}
