# LeeTeke.Dependency

基于Xamarin.Forms.Dependency创作的的IOC

#创作#   

创作来源于 Xamarin.Forms.Dependency

#原文地址#   

>https://github.com/xamarin/Xamarin.Forms/tree/5.0.0/Xamarin.Forms.Core
>https://github.com/xamarin/Xamarin.Forms/blob/5.0.0/Xamarin.Forms.Core/DependencyService.cs
>https://github.com/xamarin/Xamarin.Forms/blob/5.0.0/LICENSE



#使用方法#   
```C#
///自动注册
Leeteke.Dependency.DependencyService.Initialize(params Assembly[] assemblies);

///手动注册
Leeteke.Dependency.DependencyService.Regedit<T>();

///获取全局实例
Leeteke.Dependency.DependencyService.Get<T>(DependencyFetchTarget fetchTarget = DependencyFetchTarget.GlobalInstance);

///获取新建实例
Leeteke.Dependency.DependencyService.Get<T>(DependencyFetchTarget fetchTarget = DependencyFetchTarget.NewInstance);

///获取所有的全局实例
Leeteke.Dependency.DependencyService.GetAllGlobalInstance<T>(）;
///替换
Leeteke.Dependency.DependencyService.Replace<OldT, NewT>()；

///清理资源
Leeteke.Dependency.DependencyService.Release();
```




