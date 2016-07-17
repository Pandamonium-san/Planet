using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  /// <summary>
  /// Linked list containing a fixed amount of elements.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class FixedList<T> : LinkedList<T>
  {
    public int Size { get; private set; }

    public FixedList(int size)
    {
      this.Size = size;
    }

    public new void AddFirst(T obj)
    {
      base.AddFirst(obj);
      if (base.Count > Size)
        base.RemoveLast();
    }

    public new void AddLast(T obj)
    {
      base.AddLast(obj);
      if (base.Count > Size)
        base.RemoveFirst();
    }

    public T Pop()
    {
      if (Count <= 0)
        return default(T);
      T pop = base.First.Value;
      base.RemoveFirst();
      return pop;
    }

    public T PopLast()
    {
      if (Count <= 0)
        return default(T);
      T pop = base.Last.Value;
      base.RemoveLast();
      return pop;
    }

    public T Peek()
    {
      if (Count <= 0)
        return default(T);
      return (First.Value);
    }

    public T PeekLast()
    {
      if (Count <= 0)
        return default(T);
      return (Last.Value);
    }
  }
}
