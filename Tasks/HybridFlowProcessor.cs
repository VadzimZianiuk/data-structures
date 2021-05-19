using System;
using Tasks.DoNotChange;

namespace Tasks
{
    public class HybridFlowProcessor<T> : IHybridFlowProcessor<T>
    {
        private readonly DoublyLinkedList<T> linkedList;

        public HybridFlowProcessor()
        {
            linkedList = new DoublyLinkedList<T>();
        }

        public T Dequeue()
        {
            if (linkedList.Length == 0)
            {
                throw new InvalidOperationException();
            }
            return linkedList.RemoveAt(0);
        }

        public void Enqueue(T item)
        {
            linkedList.Add(item);
        }

        public T Pop()
        {
            if (linkedList.Length == 0)
            {
                throw new InvalidOperationException();
            }
            return linkedList.RemoveAt(linkedList.Length - 1);
        }

        public void Push(T item)
        {
            linkedList.Add(item);
        }
    }
}
