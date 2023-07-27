namespace ConsoleApp1.Sturctures;

public class BinaryNode<T>
{
    public BinaryNode<T>? Left { get; set; }
    public BinaryNode<T>? Right { get; set; }

    public T Value { get; set; }

    public BinaryNode(BinaryNode<T> left, BinaryNode<T> right, T value)
    {
        Left = left;
        Right = right;
        Value = value;
    }

    public BinaryNode(T value)
        => Value = value;

    public T GetValue() => Value;
}
