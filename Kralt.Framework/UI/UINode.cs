using System;
using System.Collections.Generic;
using Kralt.Mathematics;

namespace Kralt.Framework.UI;

public class UINode
{
    private readonly List<UINode> children = [];

    public UINode? parent { get; private set; }

    public Rect layout { get; internal set; }

    public Style style { get; set; } = new();

    public void Add(UINode child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (child == this)
            throw new InvalidOperationException("A UI node cannot be its own child.");

        for(UINode? node = this; node != null; node = parent)
        {
            if (node == child)
                throw new InvalidOperationException("A UI node may not be added to one of its descendants.");
        }

        child.parent?.Remove(child);

        children.Add(child);
        child.parent = this;
    }

    public void Remove(UINode child)
    {
        if (children.Remove(child))
            child.parent = null;
    }
}
