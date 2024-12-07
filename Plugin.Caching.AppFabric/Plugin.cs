using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SAL.Interface.Caching;
using SAL.Flatbed;

namespace Plugin.Caching.AppFabric
{
	public class Plugin : IPlugin, ICachePlugin
	{
		private TraceSource _trace;
		private Dictionary<String, ICacheModule> _modules;
		private readonly Object _moduleLock = new Object();

		internal TraceSource Trace => this._trace ?? (this._trace = Plugin.CreateTraceSource<Plugin>());

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			this._modules = new Dictionary<String, ICacheModule>();
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
			=> true;

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}

		Int32 ICachePlugin.Count => this._modules.Count;

		ICacheModule ICachePlugin.this[String name]
		{
			get
			{
				if(!this._modules.TryGetValue(name, out ICacheModule result))
					lock(this._moduleLock)
						if(!this._modules.TryGetValue(name, out result))
						{
							result = new AppFabricCacheModule(name);
							this._modules.Add(name, result);
						}
				return result;
			}
		}

		Boolean ICachePlugin.Remove(String name)
		{
			lock(this._moduleLock)
			{
				this.Trace.TraceEvent(TraceEventType.Warning, 5, "AppFabric plugin does not support Remove operation, use AppFabric administration console to remove cache by name. Instance '{0}' still active.", name);
				return this._modules.Remove(name);
			}
		}

		public IEnumerator<ICacheModule> GetEnumerator()
			=> this._modules.Values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
			=> this.GetEnumerator();
	}
}