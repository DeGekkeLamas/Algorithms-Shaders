using System.Collections.Generic;
using UnityEngine;

public class Graph<T>
{
    public Dictionary<T, List<T>> _adjacencyList;
    public Graph() { _adjacencyList = new Dictionary<T, List<T>>(); }
    public void AddNode(T node)
    {
        if (!_adjacencyList.ContainsKey(node)) _adjacencyList[node] = new List<T>();
    }
    public void AddEdge(T fromNode, T toNode)
    {
        if (!_adjacencyList.ContainsKey(fromNode) || !_adjacencyList.ContainsKey(toNode))
        {
            Debug.Log($"Not all notes currently exist, from Graph");
            return;
        }
        _adjacencyList[fromNode].Add(toNode);
        _adjacencyList[toNode].Add(fromNode);
    }
    public void RemoveNode(T nodeToRemove)
    {
        if (!_adjacencyList.ContainsKey(nodeToRemove))
        {
            Debug.LogWarning($"Node {nodeToRemove} is alreadu deleted dumbass");
            return;
        }
        _adjacencyList.Remove(nodeToRemove);
        foreach(KeyValuePair<T, List<T>> node in _adjacencyList)
        {
            if (node.Value.Contains(nodeToRemove)) node.Value.Remove(nodeToRemove);
        }
    }
    public void RemoveNodeAndConnectedNodes(T nodeToRemove)
    {
        for (int i = _adjacencyList[nodeToRemove].Count; i > 0; i--)
        {
            RemoveNode(_adjacencyList[nodeToRemove][i-1]);
        }
        RemoveNode(nodeToRemove);
    }
    public List<T> GetNeighbours(T node)
    {
        if (_adjacencyList.ContainsKey(node)) return _adjacencyList[node];
        else return null;
    }

    public void PrintGraph()
    {
        foreach(KeyValuePair<T, List<T>> kvp in _adjacencyList) Debug.Log($"{kvp.Key}, { kvp.Value}");
    }

    public List<T> BFS(T start)
    {
        if (!_adjacencyList.ContainsKey(start)) {
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

            foreach (T w in _adjacencyList[start]) 
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
        if (!_adjacencyList.ContainsKey(start)) {
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

            foreach (T w in _adjacencyList[start]) 
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
        if (!_adjacencyList.ContainsKey(start))
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

            foreach(T w in _adjacencyList[start])
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
        graph._adjacencyList = new();
        foreach(KeyValuePair<T, List<T>> kvp in _adjacencyList)
        {
            graph._adjacencyList[kvp.Key] = new(kvp.Value);
        }
        return graph;
    }
}

