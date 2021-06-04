using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        public int Length { get; protected set; }
        private Node<T> root;
        private int version;

        public DoublyLinkedList()
        {
        }

        public DoublyLinkedList(IEnumerable<T> sequence)
            : this()
        {
            if (sequence is null)
            {
                throw new ArgumentNullException(nameof(sequence));
            }

            foreach (var item in sequence)
            {
                Add(item);
            }
        }

        public void Add(T e)
        {
            if (Length == 0)
            {
                root = new Node<T>(e);
                Length++;
                version++;
            }
            else
            {
                InsertBefore(e, root);
            }
        }

        public void AddAt(int index, T e)
        {
            if (index == Length)
            {
                Add(e);
            }
            else
            {
                var node = NodeAt(index);
                InsertBefore(e, node);
                if (index == 0)
                {
                    root = root.Previous;
                }
            }
        }

        public T ElementAt(int index) => NodeAt(index).Value;

        public void Remove(T item)
        {
            var node = root;
            for (int i = 0; i < Length; i++)
            {
                if (IsEquals(node.Value, item))
                {
                    Remove(node);
                    return;
                }

                node = node.Next;
            }
        }

        public T RemoveAt(int index)
        {
            var node = NodeAt(index);
            Remove(node);
            return node.Value;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private static bool IsEquals(T a, T b) => a?.Equals(b) ?? b == null;

        private void InsertBefore(T e, Node<T> node)
        {
            var previous = node.Previous ?? node; 
            var newNode = new Node<T>(e) { Next = node, Previous = previous};
            previous.Next = newNode;
            node.Previous = newNode;

            Length++;
            version++;
        }

        private void Remove(Node<T> node)
        {
            if (node == root)
            {
                root = node.Next;
            }

            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }

            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }

            Length--;
            version++;
        }

        private Node<T> NodeAt(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException();
            }

            var node = root;
            if (index < Length / 2)
            {
                for (int i = 0; i < index; i++)
                {
                    node = node.Next;
                }
            }
            else
            {
                for (int i = Length; i > index; i--)
                {
                    node = node.Previous;
                }
            }

            return node;
        }

        private struct Enumerator : IEnumerator<T>
        {
            private readonly int version;
            private readonly DoublyLinkedList<T> linkedList;
            private Node<T> current;
            private int index;

            internal Enumerator(DoublyLinkedList<T> linkedList)
            {
                this.linkedList = linkedList ?? throw new ArgumentNullException(nameof(linkedList));
                version = linkedList.version;
                current = null;
                index = 0;
            }

            private T Current
            {
                get
                {
                    CheckVersion();
                    if (current is null)
                    {
                        throw new InvalidOperationException("Call MoveNext or Reset at first.");
                    }

                    return current.Value;
                }
            }

            public bool MoveNext()
            {
                CheckVersion();
                if (index == linkedList.Length)
                {
                    current = null;
                    return false;
                }

                current = index++ == 0 ? linkedList.root : current.Next;
                return true;
            }

            void IEnumerator.Reset()
            {
                CheckVersion();
                index = 0;
            }

            /// <inheritdoc/>
            object IEnumerator.Current => this.Current;

            /// <inheritdoc/>
            T IEnumerator<T>.Current => this.Current;

            /// <inheritdoc/>
            public void Dispose()
            {
            }

            private void CheckVersion()
            {
                if (version != linkedList.version)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
