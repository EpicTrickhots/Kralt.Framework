using Kralt.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kralt.Framework.UI;

public class Node : IEnumerable<Node>
{
    private readonly List<Node> children = [];

    public IReadOnlyList<Node> Children => children;

    public Node? parent { get; private set; }

    public Rect layout { get; internal set; }

    // public Style style { get; set; } = new();

    public void Add(Node child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (child == this)
            throw new InvalidOperationException("A UI node cannot be its own child.");

        for(Node? node = this; node != null; node = node.parent)
        {
            if (node == child)
                throw new InvalidOperationException("A UI node may not be added to one of its descendants.");
        }

        child.parent?.Remove(child);

        children.Add(child);
        child.parent = this;
    }

    public void Remove(Node child)
    {
        if (children.Remove(child))
            child.parent = null;
    }

    public Node this[int i] => children[i];

    public IEnumerator<Node> GetEnumerator()
    {
        foreach (Node transform in children)
            yield return transform;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
