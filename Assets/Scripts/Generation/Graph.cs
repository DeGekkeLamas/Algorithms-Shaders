using System.Collections.Generic;
using UnityEngine;

public class Graph<T>
{
     Dictionary<T, List<T>> adjacencyList;
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

    public void PrintGraph()
    {
        foreach(KeyValuePair<T, List<T>> kvp in adjacencyList) Debug.Log($"{kvp.Key}, { kvp.Value}");
    }
}

