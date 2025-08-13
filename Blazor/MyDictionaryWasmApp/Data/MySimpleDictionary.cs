using System;
using System.Collections;
using System.Collections.Generic;

namespace MyDictionaryWasmApp.Data
{
    public enum HashMethod
    {
        DefaultDotNet,
        Division,
        MidSquare,
        Multiplication,
        Fibonacci
    }

    public class MySimpleDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private const int InitialCapacity = 16;
        private LinkedList<KeyValuePair<TKey, TValue>>[] buckets;
        private HashMethod hashMethod = HashMethod.DefaultDotNet;
        private int divisor;

        public MySimpleDictionary(HashMethod method = HashMethod.Division)
        {
            buckets = new LinkedList<KeyValuePair<TKey, TValue>>[InitialCapacity];
            hashMethod = method;
            divisor = 17; // prost broj 
        }

        public HashMethod CurrentHashMethod
        {
            get => hashMethod;
            set
            {
                if (hashMethod != value)
                {
                    hashMethod = value;
                    Clear();
                    Console.WriteLine($"[MySimpleDictionary] Promenjena hash metoda na {hashMethod}");
                }
            }
        }


        private int GetBucketIndex(TKey key)
        {
            Console.WriteLine($"[MySimpleDictionary] Koristi se hash metoda: {hashMethod}");
            int hash = key?.GetHashCode() ?? 0;
            int size = buckets.Length;

            switch (hashMethod)
            {
                case HashMethod.DefaultDotNet:
                    return Math.Abs(hash) % size;

                case HashMethod.Division:
                    return Math.Abs(hash) % size;  // koristi veličinu buckets niza

                case HashMethod.MidSquare:
                    ulong square = (ulong)(hash) * (ulong)(hash);
                    int mid = (int)((square >> 8) & 0xFFFF);
                    return Math.Abs(mid) % size;

                case HashMethod.Multiplication:
                    double A = 0.6180339887; // preporučena konstanta (zlatni rez - 1)
                    double hashTimesA = hash * A;
                    double frac = hashTimesA - Math.Floor(hashTimesA); // samo decimalni deo
                    return (int)(size * frac);


                case HashMethod.Fibonacci:
                    const uint fibConstant = 2654435769u;
                    uint uhash = (uint)hash;
                    int bits = Math.Max(1, (int)Math.Log(size, 2));
                    int shift = 32 - bits;
                    int result = (int)((uhash * fibConstant) >> shift);
                    return Math.Abs(result) % size;

                default:
                    throw new NotSupportedException($"Hash metoda {hashMethod} nije podržana.");
            }
        }

        public void Add(TKey key, TValue value)
        {
            int index = GetBucketIndex(key);
            if (buckets[index] == null)
                buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();

            foreach (var pair in buckets[index])
            {
                if (Equals(pair.Key, key))
                    throw new ArgumentException($"Key '{key}' already exists.");
            }
            buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            int index = GetBucketIndex(key);
            if (buckets[index] == null)
                return false;

            var current = buckets[index].First;
            while (current != null)
            {
                if (Equals(current.Value.Key, key))
                {
                    buckets[index].Remove(current);
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < buckets.Length; i++)
                buckets[i] = null;
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (var bucket in buckets)
                    if (bucket != null)
                        count += bucket.Count;
                return count;
            }
        }

        //dohvatanje vrednosti na osnovu ključa
        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = GetBucketIndex(key);
            if (buckets[index] != null)
            {
                foreach (var pair in buckets[index])
                {
                    if (Equals(pair.Key, key))
                    {
                        value = pair.Value;
                        return true;
                    }
                }
            }
            value = default!;
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var value))
                    return value;
                throw new KeyNotFoundException($"Key '{key}' not found.");
            }
            set
            {
                int index = GetBucketIndex(key);
                if (buckets[index] == null)
                    buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();

                // ako postoji ključ, ažuriraj vrednost
                var node = buckets[index].First;
                while (node != null)
                {
                    if (Equals(node.Value.Key, key))
                    {
                        buckets[index].Remove(node);
                        buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
                        return;
                    }
                    node = node.Next;
                }

                // ako ne postoji, dodaj
                buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        //provera postojanja ključa
        public bool ContainsKey(TKey key)
        {
            int index = GetBucketIndex(key);
            if (buckets[index] != null)
            {
                foreach (var pair in buckets[index])
                {
                    if (Equals(pair.Key, key))
                        return true;
                }
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var bucket in buckets)
                if (bucket != null)
                    foreach (var pair in bucket)
                        yield return pair;
        }

        //provera postojanja vrednosti
        public bool ContainsValue(TValue value)
        {
            foreach (var bucket in buckets)
            {
                if (bucket != null)
                {
                    foreach (var pair in bucket)
                    {
                        if (Equals(pair.Value, value))
                            return true;
                    }
                }
            }
            return false;
        }

        //Lista svih ključeva i vrednosti
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<(int BucketIndex, TKey Key, TValue Value)> GetBucketsDetails()
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != null)
                {
                    foreach (var pair in buckets[i])
                        yield return (i, pair.Key, pair.Value);
                }
            }
        }
    }
}



