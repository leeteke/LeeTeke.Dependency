
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

//本项目基于 Xamarin.Forms.Dependency 修改与创新制作
//源项目项目地址：https://github.com/xamarin/Xamarin.Forms/
//园项目具体目录地址：https://github.com/xamarin/Xamarin.Forms/blob/5.0.0/Xamarin.Forms.Core/DependencyService.cs
//源项目开源协议地址：https://github.com/xamarin/Xamarin.Forms/blob/5.0.0/LICENSE
//
namespace Leeteke.Dependency
{
   
    public static class DependencyService
    {
        private class DependencyData
        {
            public object GlobalInstance
            {
                get;
                set;
            }

            public Type ImplementorType
            {
                get;
                set;
            }

            public DependencyData(Type imp) => ImplementorType = imp;

        }

        private static bool s_initialized;



        private static readonly object s_initializeLock = new object();

        private static readonly List<Type> DependencyTypes = new List<Type>();

        private static readonly ConcurrentDictionary<Type, DependencyData> DependencyImplementations = new ConcurrentDictionary<Type, DependencyData>();


        //
        // 摘要:
        //     Returns the platform-specific implementation of type T.
        //
        // 参数:
        //   fetchTarget:
        //     The dependency fetch target.
        //
        // 类型参数:
        //   T:
        //     The type of object to fetch.
        //
        // 返回结果:
        //     To be added.
        //
        // 言论：
        //     To be added.
        public static T Get<T>(DependencyFetchTarget fetchTarget = DependencyFetchTarget.GlobalInstance) where T : class
        {
            ///查找与赋予
            DependencyData value;
            Type targetHandle = typeof(T);
            if (!DependencyImplementations.TryGetValue(targetHandle, out value))
            {
                Type type = FindImplementor(targetHandle);
                if (type != null)
                {
                    var sameType = DependencyImplementations.Where(p => p.Value.ImplementorType.Equals(type)).FirstOrDefault();
                    value = sameType.Value ?? new DependencyData(type);
                    DependencyImplementations.TryAdd(targetHandle, value);
                }
            }

            //没有就算了
            if (value == null)
            {
                return null;
            }

            if (fetchTarget == DependencyFetchTarget.GlobalInstance)
            {
                if (value.GlobalInstance == null)
                {
                    lock (value)
                    {
                        if (value.GlobalInstance == null)
                        {
                            value.GlobalInstance = Activator.CreateInstance(value.ImplementorType);
                        }
                    }
                }

                return (T)value.GlobalInstance;
            }

            return (T)Activator.CreateInstance(value.ImplementorType);
        }

        //
        // 摘要:
        //     Registers the platform-specific implementation of type T.
        //
        // 类型参数:
        //   T:
        //     The type of object to register.
        //
        // 言论：
        //     To be added.
        public static void Register<T>() where T : class
        {
            Type typeFromHandle = typeof(T);
            if (!DependencyTypes.Contains(typeFromHandle))
            {
                DependencyTypes.Add(typeFromHandle);
            }
        }
        //
        // 摘要:
        //    替换掉现有的 
        //     Registers the platform-specific implementation of type T.
        //
        // 类型参数:
        //   T:
        //     The type of object to register.
        //
        // 言论：
        //     To be replace.
        public static void Replace<OldT, NewT>() where NewT : class where OldT : class
        {
            Type oldT = typeof(OldT);
            Type newT = typeof(NewT);
            lock (DependencyTypes)
            {
                if (DependencyTypes.Contains(oldT))
                {
                    ///删除旧的
                    DependencyTypes.Remove(oldT);

                    if (!DependencyTypes.Contains(newT))
                        DependencyTypes.Add(newT);

                    var olddi = DependencyImplementations.Where(p => p.Value?.ImplementorType == oldT);
                    if (olddi.Any())
                    {
                        var newData = new DependencyData(newT);
                        foreach (var item in olddi)
                        {
                            var sameType = DependencyImplementations.Where(p => p.Value.ImplementorType.Equals(newData)).FirstOrDefault();
                            DependencyImplementations.TryUpdate(item.Key, sameType.Value??newData, item.Value);
                        }
                    }
                }
            }

        }

        public static List<T> GetAllGlobalInstance<T>()
        {
            Type toalHandle = typeof(T);
            var allDeps = DependencyImplementations.Where(p => toalHandle.IsAssignableFrom(p.Value.GlobalInstance?.GetType()));
            if (allDeps.Any())
            {
                var result = new List<T>();
                foreach (var item in allDeps)
                {
                    var resultT = (T)item.Value.GlobalInstance;
                    if (!result.Contains(resultT))
                    {
                        result.Add(resultT);
                    }
                }
                return result;
            }
            return null;
        }


        private static Type FindImplementor(Type target)
        {
            return DependencyTypes.FirstOrDefault((Type t) => target.IsAssignableFrom(t));
        }


        public static void Initialize(params Assembly[] assemblies)
        {
            if (s_initialized)
            {
                return;
            }

            lock (s_initializeLock)
            {
                if (s_initialized)
                {
                    return;
                }

                for (int i = 0; i < assemblies.Length; i++)
                {

                    var customAttributes = assemblies[i].GetCustomAttributes(typeof(DependencyAttribute));
                    if (customAttributes == null)
                    {
                        continue;
                    }

                    foreach (DependencyAttribute attribute in customAttributes)
                    {
                        if (!DependencyTypes.Contains(attribute.Implementor))
                        {
                            DependencyTypes.Add(attribute.Implementor);
                        }
                    }

                }

                s_initialized = true;
            }
        }
    }
}
