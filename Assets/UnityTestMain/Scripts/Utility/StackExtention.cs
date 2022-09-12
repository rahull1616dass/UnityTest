using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;


/// <summary>
/// This stack class is used as same way with one extra method called 'Remove' by which an Item can be removed from anywhere in stack
/// </summary>
public class StackExtention<T>
{
    private List<T> items = new List<T>();

    public int Count
    {
        get
        {
           return items.Count;
        }
    }

    public void Push(T item)
    {
        items.Add(item);
    }

    /// <summary>
    /// Get the last entered val
    /// </summary>

    public T Pop()
    {
        if (items.Count > 0)
        {
            T temp = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            return temp;
        }
        else
            return default(T);
    }
    /// <summary>
    /// Remove the item with the position of the item
    /// </summary>
    public void Remove(int itemAtPosition)
    {
        items.RemoveAt(itemAtPosition);
    }

    public StackExtention<T> ListToStack(List<T> list)
    {
        items = list;
        return this;
    }

    public List<T> StackToList()
    {
        return items;
    }
}
