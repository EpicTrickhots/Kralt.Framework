using Kralt.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kralt.Framework.UI;

public class Drawable : IEnumerable<Drawable>
{
    private readonly List<Drawable> children = [];

    public Drawable? parent { get; private set; }

    public Rect layout { get; internal set; }

    public void Add(Drawable child)
    {
        ArgumentNullException.ThrowIfNull(child);

        if (child == this)
            throw new InvalidOperationException("A drawable cannot be its own child.");

        for(Drawable? node = this; node != null; node = node.parent)
        {
            if (node == child)
                throw new InvalidOperationException("A drawable may not be added to one of its descendants.");
        }

        child.parent?.Remove(child);

        children.Add(child);
        child.parent = this;
    }

    public void Remove(Drawable child)
    {
        if (children.Remove(child))
            child.parent = null;
    }

    public Drawable this[int i] => children[i];

    public IEnumerator<Drawable> GetEnumerator()
    {
        foreach (Drawable transform in children)
            yield return transform;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
