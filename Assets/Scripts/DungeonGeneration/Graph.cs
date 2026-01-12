using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Graph with nodes and connections
/// </summary>
/// <typeparam name="T"></typeparam>
public class Graph<T>
{
    public Dictionary<T, List<T>> adjacencyList;
    public Graph() { adjacencyList = new Dictionary<T, List<T>>(); }
    public void AddNode(T node)
    {
        if (!adjacencyList.ContainsKey(node)) adjacencyList[node] = new List<T>();
    }
    public void AddEdge(T fromNode, T toNode)
    {
        if (!adjacencyList.ContainsKey(fromNode) || !adjacencyList.ContainsKey(toNode))
        {
            Debug.Log($"Not all notes currently exist, from Graph");
            return;
        }
        adjacencyList[fromNode].Add(toNode);
        adjacencyList[toNode].Add(fromNode);
    }
    public void RemoveNode(T nodeToRemove)
    {
        if (!adjacencyList.ContainsKey(nodeToRemove))
        {
            Debug.LogWarning($"Node {nodeToRemove} is alreadu deleted dumbass");
            return;
        }
        adjacencyList.Remove(nodeToRemove);
        foreach(KeyValuePair<T, List<T>> node in adjacencyList)
        {
            if (node.Value.Contains(nodeToRemove)) node.Value.Remove(nodeToRemove);
        }
    }
    public void RemoveNodeAndConnectedNodes(T nodeToRemove)
    {
        for (int i = adjacencyList[nodeToRemove].Count; i > 0; i--)
        {
            RemoveNode(adjacencyList[nodeToRemove][i-1]);
        }
        RemoveNode(nodeToRemove);
    }
    public List<T> GetNeighbours(T node)
    {
        if (adjacencyList.ContainsKey(node)) return adjacencyList[node];
        else return null;
    }

    public void PrintGraph()
    {
        foreach(KeyValuePair<T, List<T>> kvp in adjacencyList) Debug.Log($"{kvp.Key}, { kvp.Value}");
    }

    public List<T> BFS(T start)
    {
        if (!adjacencyList.ContainsKey(start)) {
            Debug.LogWarning($"{start} doesn't exist dumbass, from {this}");
            return new();
        }

        List<T> visitedList = new();
        Queue<T> queue = new();
        queue.Enqueue(start);
        visitedList.Add(start);

        while(queue.Count > 0)
        {
            start = queue.Dequeue();

            foreach (T w in adjacencyList[start]) 
            {
                if (!visitedList.Contains(w))
                {
                    queue.Enqueue(w);
                    visitedList.Add(w);
                }
            }
        }
        return visitedList;
    }
    public List<T> BFSWithout(T start, T exclude)
    {
        if (!adjacencyList.ContainsKey(start)) {
            Debug.LogWarning($"{start} doesn't exist dumbass, from {this}");
            return new();
        }

        List<T> visitedList = new();
        Queue<T> queue = new();
        queue.Enqueue(start);
        visitedList.Add(start);

        while(queue.Count > 0)
        {
            start = queue.Dequeue();
            //Debug.Log(_start);

            foreach (T w in adjacencyList[start]) 
            {
                if (w.Equals(exclude)) { continue; }
                if (!visitedList.Contains(w))
                {
                    queue.Enqueue(w);
                    visitedList.Add(w);
                }
            }
        }
        return visitedList;
    }

    public void DFS(T start)
    {
        if (!adjacencyList.ContainsKey(start))
        {
            Debug.LogWarning($"{start} doesn't exist dumbass, from {this}");
            return;
        }

        List<T> visitedList = new();
        Stack<T> stack = new();
        stack.Push(start);
        visitedList.Add (start);

        while(stack.Count > 0)
        {
            start = stack.Pop();
            Debug.Log(start);

            foreach(T w in adjacencyList[start])
            {
                if (!visitedList.Contains(w))
                {
                    stack.Push(w);
                    visitedList.Add(w);
                }
            }
        }
    }
    public Graph<T> CloneGraph()
    {
        Graph<T> graph = new();
        graph.adjacencyList = new();
        foreach(KeyValuePair<T, List<T>> kvp in adjacencyList)
        {
            graph.adjacencyList[kvp.Key] = new(kvp.Value);
        }
        return graph;
    }
}

