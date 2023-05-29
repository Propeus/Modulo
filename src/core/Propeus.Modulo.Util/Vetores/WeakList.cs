using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Propeus.Modulo.Util.Vetores
{
    //https://stackoverflow.com/questions/18922985/concurrent-hashsett-in-net-framework
    public class ConcurrentDictionaryWeakReference<TKey, TValue> : ConcurrentDictionary<TKey, WeakReference>
    {
        public ConcurrentDictionaryWeakReference()
        {
        }

        public ConcurrentDictionaryWeakReference(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection.ToDictionary(k => k.Key, v => new WeakReference(v.Value)))
        {
        }

        public ConcurrentDictionaryWeakReference(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(collection.ToDictionary(k => k.Key, v => new WeakReference(v.Value)), comparer)
        {
        }

        public ConcurrentDictionaryWeakReference(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public ConcurrentDictionaryWeakReference(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(concurrencyLevel, collection.ToDictionary(k => k.Key, v => new WeakReference(v.Value)), comparer)
        {
        }

        public ConcurrentDictionaryWeakReference(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
        {
        }

        public ConcurrentDictionaryWeakReference(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer) : base(concurrencyLevel, capacity, comparer)
        {
        }


        /// <summary>
        /// Attempts to add the specified key and value to the <see cref="ConcurrentDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be a null reference (Nothing
        /// in Visual Basic) for reference types.</param>
        /// <returns>
        /// true if the key/value pair was added to the <see cref="ConcurrentDictionary{TKey, TValue}"/> successfully; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="OverflowException">The <see cref="ConcurrentDictionary{TKey, TValue}"/> contains too many elements.</exception>
        public bool TryAdd(TKey key, TValue value)
        {
            RemoveWeaksIsDeads();
            return TryAdd(key, new WeakReference(value));
        }

        /// <summary>
        /// Determines whether the <see cref="ConcurrentDictionary{TKey, TValue}"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="ConcurrentDictionary{TKey, TValue}"/>.</param>
        /// <returns>true if the <see cref="ConcurrentDictionary{TKey, TValue}"/> contains an element with the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool ContainsKey(TKey key)
        {
            RemoveWeaksIsDeads();
            return base.ContainsKey(key);
        }

        /// <summary>
        /// Attempts to remove and return the value with the specified key from the <see cref="ConcurrentDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove and return.</param>
        /// <param name="value">
        /// When this method returns, <paramref name="value"/> contains the object removed from the
        /// <see cref="ConcurrentDictionary{TKey,TValue}"/> or the default value of <typeparamref
        /// name="TValue"/> if the operation failed.
        /// </param>
        /// <returns>true if an object was removed successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool TryRemove(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            RemoveWeaksIsDeads();
            if (TryRemove(key, out WeakReference weakReference))
            {
                value = (TValue)weakReference.Target;
                return value is not null;
            }
            value = default;
            return false;

        }

        /// <summary>Removes a key and value from the dictionary.</summary>
        /// <param name="item">The <see cref="KeyValuePair{TKey,TValue}"/> representing the key and value to remove.</param>
        /// <returns>
        /// true if the key and value represented by <paramref name="item"/> are successfully
        /// found and removed; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Both the specified key and value must match the entry in the dictionary for it to be removed.
        /// The key is compared using the dictionary's comparer (or the default comparer for <typeparamref name="TKey"/>
        /// if no comparer was provided to the dictionary when it was constructed).  The value is compared using the
        /// default comparer for <typeparamref name="TValue"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// The <see cref="KeyValuePair{TKey, TValue}.Key"/> property of <paramref name="item"/> is a null reference.
        /// </exception>
        public bool TryRemove(KeyValuePair<TKey, TValue> item)
        {
            RemoveWeaksIsDeads();
            return TryRemove(new KeyValuePair<TKey, WeakReference>(item.Key, new WeakReference(item.Value)));
        }

        /// <summary>
        /// Attempts to get the value associated with the specified key from the <see cref="ConcurrentDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        /// When this method returns, <paramref name="value"/> contains the object from
        /// the <see cref="ConcurrentDictionary{TKey,TValue}"/> with the specified key or the default value of
        /// <typeparamref name="TValue"/>, if the operation failed.
        /// </param>
        /// <returns>true if the key was found in the <see cref="ConcurrentDictionary{TKey,TValue}"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference (Nothing in Visual Basic).</exception>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            RemoveWeaksIsDeads();
            if (TryGetValue(key, out WeakReference weakReference))
            {
                value = (TValue)weakReference.Target;
                return value is not null;
            }
            value = default;
            return false;

        }


        /// <summary>
        /// Updates the value associated with <paramref name="key"/> to <paramref name="newValue"/> if the existing value is equal
        /// to <paramref name="comparisonValue"/>.
        /// </summary>
        /// <param name="key">The key whose value is compared with <paramref name="comparisonValue"/> and
        /// possibly replaced.</param>
        /// <param name="newValue">The value that replaces the value of the element with <paramref
        /// name="key"/> if the comparison results in equality.</param>
        /// <param name="comparisonValue">The value that is compared to the value of the element with
        /// <paramref name="key"/>.</param>
        /// <returns>
        /// true if the value with <paramref name="key"/> was equal to <paramref name="comparisonValue"/> and
        /// replaced with <paramref name="newValue"/>; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is a null reference.</exception>
        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
        {
            RemoveWeaksIsDeads();
            return TryUpdate(key, new WeakReference(newValue), new WeakReference(comparisonValue));
        }

        private void RemoveWeaksIsDeads()
        {
            foreach (var item in this)
            {
                if (!item.Value.IsAlive)
                {
                    base.TryRemove(item.Key, out _);
                }
            }
        }

    }
}
