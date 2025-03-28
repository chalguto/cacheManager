namespace Infraestructure.Service
{
    using System;
    using System.Linq;
    using System.Runtime.Caching;    

    public class CacheManager : ICacheManager
    {
        private readonly MemoryCache cache = MemoryCache.Default;

        /// <summary>
        /// Adds or updates an item in the cache.
        /// </summary>
        /// <param name="key">Key of the item to add or update.</param>
        /// <param name="value">Value of the item to add or update.</param>
        /// <param name="expirationTime">Expiration time of the item. If not provided, 10 minutes will be used by default.</param>
        public void AddOrUpdate<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(value), "Key cannot be null or empty");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null");
            }

            CacheItemPolicy policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTimeOffset.Now.Add(expirationTime ?? TimeSpan.FromMinutes(10))
            };

            cache.Set(key, value, policy);
        }

        /// <summary>
        /// Gets an item from the cache.
        /// </summary>
        /// <param name="key">Key of the item to get.</param>
        /// <returns>The value associated with the key.</returns>
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null or empty");
            }

            if (!(cache.Get(key) is T value))
            {
                throw new Exception(key + " does not exist in the cache");
            }

            return value;
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">Key of the item to remove.</param>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null or empty");
            }

            cache.Remove(key);
        }

        /// <summary>
        /// Checks if an item exists in the cache.
        /// </summary>
        /// <param name="key">Key of the item to check.</param>
        /// <returns>True if the item exists in the cache, False otherwise.</returns>
        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key cannot be null or empty");
            }

            return cache.Contains(key);
        }

        /// <summary>
        /// Clears all items from the cache.
        /// </summary>
        public void Clear()
        {
            cache.ToList().ForEach(item => cache.Remove(item.Key));
        }
    }
}