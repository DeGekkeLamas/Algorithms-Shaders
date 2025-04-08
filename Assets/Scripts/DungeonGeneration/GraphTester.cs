using UnityEngine;

public class GraphTester : MonoBehaviour
{
    Graph<string> graph = new Graph<string>();

    public int[] array1 = new int[] {1, 4, 7, 9, 2}; 
    public int[] array2; 
    void Start()
    {
        array2 = array1;
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
}
