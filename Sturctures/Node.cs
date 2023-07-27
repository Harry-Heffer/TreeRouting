using System.Collections.Generic;

namespace ConsoleApp1.Sturctures;

public class Node<T>
{
    public Node<T>? Previous { get; set; }
    public List<Node<T>> Next { get; set; } = new List<Node<T>>();

    public T Value { get; set; }

    public Node(Node<T> previous, Node<T> next, T value)
    {
        Previous = previous;
        Next.Add(next);
        Value = value;
    }
    public Node(Node<T> node, T value, bool isNext = false)
    {
        if (isNext)
            Next.Add(node);
        else
            Previous = node;
        Value = value;
    }

    public Node(T value)
        => Value = value;

    public T GetValue() => Value;

    public IEnumerable<Node<T>>? GetNext => Next.Select(x => x);
}
