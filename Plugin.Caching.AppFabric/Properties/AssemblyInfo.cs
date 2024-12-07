using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("c2c3398a-0316-4cac-af34-8013735e4a9b")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.Caching.AppFabric")]
#else

[assembly: AssemblyTitle("Plugin.Caching.AppFabric")]
[assembly: AssemblyDescription("AppFabric caching service")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.Caching.AppFabric")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016")]
#endif