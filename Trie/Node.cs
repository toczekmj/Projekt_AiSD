namespace Backend;

public class Node
{
    public char Value { get; }
    public Dictionary<char, Node> Children { get; }
    public bool IsWord { get; set; }

    public Node(char value)
    {
        Value = value;
        Children = new Dictionary<char, Node>();
        IsWord = false;
    }
}