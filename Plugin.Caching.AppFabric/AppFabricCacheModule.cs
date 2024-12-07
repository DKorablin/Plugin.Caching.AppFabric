using System;
using SAL.Interface.Caching;
using Microsoft.ApplicationServer.Caching;

namespace Plugin.Caching.AppFabric
{
	public class AppFabricCacheModule : ICacheModule
	{
		private static DataCacheFactory _factory = null;

		private static DataCacheFactory Factory
		{
			get
			{
				if(AppFabricCacheModule._factory == null)
				{
					DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration()
					{
						LocalCacheProperties = new DataCacheLocalCacheProperties(),
					};
					DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Off);

					//configuration.SecurityProperties = new DataCacheSecurity();

					AppFabricCacheModule._factory = new DataCacheFactory(configuration);
				}
				return AppFabricCacheModule._factory;
			}
		}
		
		public String Name { get; }

		private DataCache Cache { get; }

		public AppFabricCacheModule(String cacheName)
		{
			if(String.IsNullOrEmpty(cacheName))
				throw new ArgumentNullException(nameof(cacheName));

			this.Name = cacheName;
			this.Cache = AppFabricCacheModule.Factory.GetCache(this.Name);
		}

		public T Get<T>(String key) where T : class
			=> (T)this.Cache.Get(key);

		public T Get<T>(String key, Func<T> fallback, TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration) where T : class
		{
			T result = this.Get<T>(key);
			if(result == null && fallback != null)
			{
				result = fallback();

				this.Add<T>(key, result, slidingExpiration, absoluteExpiration);
			} else if(result != null && slidingExpiration.HasValue)//Обнулить slidingExpiration, если объект взят из кеша
			{
				this.Cache.ResetObjectTimeout(key, slidingExpiration.Value);
			}
			return result;
		}

		public void Add<T>(String key, T value, TimeSpan? slidingExpiration, DateTimeOffset? absoluteExpiration)
		{
			TimeSpan expiration;
			if(slidingExpiration.HasValue)
				expiration = slidingExpiration.Value;
			else if(absoluteExpiration.HasValue)
				expiration = absoluteExpiration.Value - DateTime.Now;
			else
				throw new ArgumentNullException(nameof(slidingExpiration),"You must specify sliding or absolute expiration");

			this.Cache.Add(key, value, expiration);
		}

		public void Remove(String key)
			=> this.Cache.Remove(key);
	}
}