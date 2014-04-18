using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asgn
{
    /// <summary>
    /// Class representing a min-heap.
    /// </summary>
    class MinHeap<T> where T : IComparable<T>
    {
        List<T> data = new List<T>();

        public int length
        {
            get
            {
                return data.Count;
            }
        }

        public static int parent(int i)
        {
            return (i - 1) / 2;
        }

        public static int left(int i)
        {
            return 2 * i + 1;
        }

        public static int right(int i)
        {
            return 2 * i + 2;
        }

        public void heapify(int i)
        {
            int l = left(i);
            int r = right(i);
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
                heapify(smallest);
            }
        }

        public void insert(T value)
        {
            data.Add(value);
            int i = data.Count - 1;
            while (i > 0 && data[parent(i)].CompareTo(data[i]) > 0)
            {
                T temp = data[i];
                data[i] = data[parent(i)];
                data[parent(i)] = temp;
                i = parent(i);
            }
        }

        public T remove()
        {
            T value = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            heapify(0);
            return value;
        }
    }
}
