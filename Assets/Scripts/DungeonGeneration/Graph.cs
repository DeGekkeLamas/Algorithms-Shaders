using System.Collections.Generic;
using UnityEngine;

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

    public void PrintGraph()
    {
        foreach(KeyValuePair<T, List<T>> kvp in adjacencyList) Debug.Log($"{kvp.Key}, { kvp.Value}");
    }

    public List<T> BFS(T _start)
    {
        if (!adjacencyList.ContainsKey(_start)) {
            Debug.LogWarning($"{_start} doesn't exist dumbass, from {this}");
            return new();
        }

        List<T> visitedList = new();
        Queue<T> queue = new();
        queue.Enqueue(_start);
        visitedList.Add(_start);

        while(queue.Count > 0)
        {
            _start = queue.Dequeue();
            //Debug.Log(_start);

            foreach (T w in adjacencyList[_start]) 
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
    public void DFS(T _start)
    {
        if (!adjacencyList.ContainsKey(_start))
        {
            Debug.LogWarning($"{_start} doesn't exist dumbass, from {this}");
            return;
        }

        List<T> visitedList = new();
        Stack<T> stack = new();
        stack.Push(_start);
        visitedList.Add (_start);

        while(stack.Count > 0)
        {
            _start = stack.Pop();
            Debug.Log(_start);

            foreach(T w in adjacencyList[_start])
            {
                if (!visitedList.Contains(w))
                {
                    stack.Push(w);
                    visitedList.Add(w);
                }
            }
        }
    }
}

