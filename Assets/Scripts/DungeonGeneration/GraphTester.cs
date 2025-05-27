using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphTester : MonoBehaviour
{
    Graph<string> graph = new Graph<string>();

    public List<int> array1 = new() {1, 4, 7, 9, 2}; 
    public List<int> array2 = new() {1, 4, 7, 9, 2};
    void Start()
    {
        Debug.Log( ListsAreEqual(array1, array2) );
        //array2 = array1;
        graph.AddNode("A"); graph.AddNode("B"); graph.AddNode("C");
        graph.AddNode("D"); graph.AddNode("E");
        graph.AddEdge("A", "B"); graph.AddEdge("A", "C");
        graph.AddEdge("B", "D"); graph.AddEdge("C", "D");
        graph.AddEdge("D", "E");

        //Debug.Log("Graph Structure:");
        //PrintGraph();


        //Debug.Log("BFS Traversal:");
        //graph.DFS("A", "E");
    }
    [ContextMenu("Print graph")]
    public void PrintGraph() => graph.PrintGraph();

    bool ListsAreEqual<T>(List<T> list1, List<T> list2)
    {
        if (list1.Count == list2.Count)
        {
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
            }
            }
            return true;
    }
        return false;
    }
}
