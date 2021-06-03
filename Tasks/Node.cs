namespace Tasks
{
    internal class Node<T>
    {
        internal T Value { get; }

        internal Node<T> Previous { get; set; }

        internal Node<T> Next { get; set; }

        internal Node(T value)
        {
            this.Value = value;
        }
    }
}
