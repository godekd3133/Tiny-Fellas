using System.Collections.Generic;

public enum eACT_FSM
{
    Default, Idle, Attack, Defense, Dead,
    MAX
}

public enum eACT_BT
{
    Default, Idle, Discovery, Dead,
    MAX
}

public abstract class Node
{
    public abstract bool Invoke();
}

public class CompositeNode : Node
{
    private Stack<Node> Childs = new Stack<Node>();

    public override bool Invoke()
    {
        throw new System.NotImplementedException();
    }

    public void AddChild(Node node)
    {
        Childs.Push(node);
    }

    public Stack<Node> GetChilds()
    {
        return Childs;
    }
}

public class Selector : CompositeNode
{
    public override bool Invoke()
    {
        foreach (var node in GetChilds())
            if (node.Invoke())
                return true;
        return false;
    }
}

public class Sequence : CompositeNode
{
    public override bool Invoke()
    {
        foreach (var node in GetChilds())
            if (!node.Invoke())
                return false;
        return true;
    }
}