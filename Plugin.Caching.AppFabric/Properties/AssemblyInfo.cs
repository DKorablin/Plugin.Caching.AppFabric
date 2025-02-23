using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("c2c3398a-0316-4cac-af34-8013735e4a9b")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.Caching.AppFabric")]
#else

[assembly: AssemblyDescription("AppFabric caching service")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2016-2024")]
#endif