using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing a minimum heap.
    /// </summary>
    class MinHeap<T> where T : IComparable<T>
    {
        List<T> data = new List<T>();

        public static int Parent(int i)
        {
            return (i - 1) / 2;
        }

        public static int Left(int i)
        {
            return 2 * i + 1;
        }

        public static int Right(int i)
        {
            return 2 * i + 2;
        }

        public void Heapify(int i)
        {
            int l = Left(i);
            int r = Right(i);
            int smallest = i;
            if (l < data.Count && data[l].CompareTo(data[smallest]) < 0)
                smallest = l;
            if (r < data.Count && data[r].CompareTo(data[smallest]) < 0)
                smallest = r;
            if (smallest != i)
            {
                T temp = data[i];
                data[i] = data[smallest];
                data[smallest] = temp;
                Heapify(smallest);
            }
        }

        public void Insert(T value)
        {
            data.Add(value);
            int i = data.Count - 1;
            while (i > 0 && data[Parent(i)].CompareTo(data[i]) > 0)
            {
                T temp = data[i];
                data[i] = data[Parent(i)];
                data[Parent(i)] = temp;
                i = Parent(i);
            }
        }

        public T Remove()
        {
            T value = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            Heapify(0);
            return value;
        }
    }
}
