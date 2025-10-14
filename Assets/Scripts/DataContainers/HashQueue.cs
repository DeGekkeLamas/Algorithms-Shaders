using System.Collections.Generic;

/// <summary>
/// Frankensteined combination of a hashset and a queu
/// </summary>
public struct HashQueue<T>
{
    HashSet<T> hashSet;
    Queue<T> queue;
    public int Count { get { return hashSet.Count; } }

    public void Initialize()
    {
        hashSet = new();
        queue = new();
    }
    public void Enqueue(T element)
    {
        if (hashSet.Add(element)) queue.Enqueue(element);

    }
    public T Dequeue()
    {
        T value = queue.Dequeue();
        hashSet.Remove(value);
        return value;
    }
}