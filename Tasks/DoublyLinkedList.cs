using System;
using System.Collections;
using System.Collections.Generic;
using Tasks.DoNotChange;

namespace Tasks
{
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        public int Length { get; private set; }
        private Node head;
        private Node tail;
        private int version;

        public DoublyLinkedList()
        {
        }

        public DoublyLinkedList(IEnumerable<T> sequence)
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
            if (tail is null)
            {
                head = tail = new Node(e);
            }
            else
            {
                tail.NextNode = new Node(e) { PreviousNode = tail };
                tail = tail.NextNode;
            }

            Length++;
            version++;
        }

        public void AddAt(int index, T e)
        {
            if (index < 0 || index > Length)
            {
                throw new IndexOutOfRangeException();
            }

            if (index == Length)
            {
                Add(e);
                return;
            }

            var node = NodeAt(index);
            var newNode = new Node(e) { NextNode = node };
            if (node == head)
            {
                head = newNode;
            }
            else
            {
                node.PreviousNode.NextNode = newNode;
            }

            node.PreviousNode = newNode;
            Length++;
            version++;
        }

        public T ElementAt(int index)
        {
            if (index < 0 || index >= this.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return NodeAt(index).Value;
        }

        public void Remove(T item)
        {
            var node = head;
            while (node != null)
            {
                if (IsEquals(node.Value, item))
                {
                    if (node == head)
                    {
                        head = head.NextNode;
                        head.PreviousNode = null;
                    }
                    else if (node == tail)
                    {
                        tail = tail.PreviousNode;
                        tail.NextNode = null;
                    }
                    else
                    {
                        node.PreviousNode.NextNode = node.NextNode;
                        node.NextNode.PreviousNode = node.PreviousNode;
                    }

                    Length--;
                    version++;
                    return;
                }

                node = node.NextNode;
            }
        }

        public T RemoveAt(int index)
        {
            if (index < 0 || index >= this.Length)
            {
                throw new IndexOutOfRangeException();
            }

            var node = NodeAt(index);
            if (node == head)
            {
                head = node.NextNode;
            }

            if (node == tail)
            {
                tail = node.PreviousNode;
            }

            if (node.NextNode != null)
            {
                node.NextNode.PreviousNode = node.PreviousNode;
            }

            if (node.PreviousNode != null)
            {
                node.PreviousNode.NextNode = node.NextNode;
            }

            node.PreviousNode = null;
            node.NextNode = null;
            Length--;
            version++;
            return node.Value;
        }

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            private readonly int version;
            private readonly DoublyLinkedList<T> list;
            private Node current;
            private int index;

            internal Enumerator(DoublyLinkedList<T> list)
            {
                this.list = list ?? throw new ArgumentNullException(nameof(list));
                version = list.version;
                current = null;
                index = 0;
            }

            public T Current
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
                if (index == list.Length)
                {
                    current = null;
                    return false;
                }

                current = index++ == 0 ? list.head : current.NextNode;
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
                if (version != list.version)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private static bool IsEquals(T a, T b) => a?.Equals(b) ?? b == null;

        private Node NodeAt(int index)
        {
            Node node;
            if (index <= Length / 2)
            {
                node = head;
                while (--index >= 0)
                {
                    node = node.NextNode;
                }

                return node;
            }

            node = tail;
            index = Length - index - 1;
            while (--index >= 0)
            {
                node = node.PreviousNode;
            }

            return node;
        }

        private class Node
        {
            internal T Value { get; }

            internal Node PreviousNode { get; set; }

            internal Node NextNode { get; set; }

            internal Node(T value)
            {
                this.Value = value;
            }
        }
    }
}
