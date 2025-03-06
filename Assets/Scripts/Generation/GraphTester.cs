using UnityEngine;

public class GraphTester : MonoBehaviour
{
    Graph<string> graph = new Graph<string>();
    void Start()
    {
        graph.AddNode("A");
        graph.AddNode("B");
        graph.AddNode("C");
        graph.AddNode("D");
        graph.AddNode("E");


        graph.AddEdge("A", "B");
        graph.AddEdge("A", "C");
        graph.AddEdge("B", "C");
        graph.AddEdge("C", "D");
        graph.AddEdge("D", "E");
        graph.AddEdge("A", "E");
    }
    [ContextMenu("Print graph")]
    public void PrintGraph() => graph.PrintGraph();
}
