using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UsefulDataStructure
{
    class IndexHeapNode<T>
    {
        public int Index;
        public T Content;
        public IndexHeapNode(T content)
        {
            this.Content = content;
        }
    }

    class IndexHeap<T> : IEnumerable
    {
        private IndexHeapNode<T>[] heap;
        public int Count { private set; get; }
        public delegate int CustomCompareFunction(T x, T y);
        private CustomCompareFunction CompareFunction;

        private int Compare(int i, int j)
        {
            T val1 = heap[i].Content;
            T val2 = heap[j].Content;
            return CompareFunction(val1, val2);
        }

        public IndexHeap(int count, CustomCompareFunction compare)
        {
            heap = new IndexHeapNode<T>[count];
            CompareFunction = compare;
        }

        public T this[int i]
        {
            get { return heap[i].Content; }
        }

        public IndexHeapNode<T> Push(T value)
        {
            if (Count >= heap.Length)
            {
                throw new IndexOutOfRangeException(String.Format("Index out of bound exception. Maximum Length:{0}, Count:{1}", heap.Length, Count));
            }

            IndexHeapNode<T> curNode = new IndexHeapNode<T>(value);
            heap[Count] = curNode;
            curNode.Index = Count++;
            BubbleUp(curNode.Index);
            return curNode;
        }

        public T Poll()
        {
            if (Count == 0)
                throw new InvalidOperationException("Heap is empty");

            T ret = heap[0].Content;
            Swap(0, Count - 1);
            Count--;
            heap[Count] = null;
            BubbleDown(0);
            return ret;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("Heap is empty");
            return heap[0].Content;
        }

        public IndexHeapNode<T> GetIndexHeapNode(int index)
        {
            return heap[index];
        }

        public void Remove(int index)
        {
            Swap(index, Count - 1);
            Count--;
            heap[Count] = null;
            if (index < Count)
            {
                BubbleUp(index);
                BubbleDown(index);
            }
        }

        private void BubbleDown(int index)
        {
            while (index < Count)
            {
                T parentContent = heap[index].Content;
                int largest = index;
                if (index * 2 < Count && Compare(largest, index * 2) < 0)
                {
                    largest = index * 2;
                }

                if (index * 2 + 1 < Count && Compare(largest, index * 2 + 1) < 0)
                {
                    largest = index * 2 + 1;
                }

                if (index != largest)
                {
                    Swap(index, largest);
                    index = largest;
                }
                else
                    break;
            }
        }

        private void BubbleUp(int index)
        {
            for (int i = index; i > 0; i /= 2)
            {
                if (Compare(i / 2, i) < 0)
                {
                    Swap(i / 2, i);
                }
                else
                {
                    break;
                }

            }
        }

        private void Swap(int x, int y)
        {
            IndexHeapNode<T> temp = heap[x];
            heap[x] = heap[y];
            heap[x].Index = x;
            heap[y] = temp;
            heap[y].Index = y;
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return heap[i];
            }
        }
    }

    public class Test
    {
        public int IntCompare(int x, int y)
        {
            if (x == y)
                return 0;
            else if (x > y)
                return 1;
            else
                return -1;
        }
        [Fact]
        public void Test1()
        {
            int[] array = new []{ 1, 2 };
            IndexHeap<int> heap = new IndexHeap<int>(array.Length, IntCompare);
            heap.Push(1);
            heap.Push(2);
            Assert.Equal(2, heap[0]);
            Assert.Equal(1, heap[1]);
        }

        [Fact]
        public void Test2()
        {
            int[] array = new int[] { 2, 5, 1, 4, 6, 7, 8, 3, 9, 10};
            IndexHeap<int> heap = new IndexHeap<int>(array.Length, IntCompare);
            foreach (var e in array)
            {
                heap.Push(e);
            }

            Array.Sort(array);
            Array.Reverse(array);
            foreach (var e in array)
            {
                int val = heap.Poll();
                Assert.Equal(e, val);
            }
            Assert.Equal(0, heap.Count);
        }
    }


}
