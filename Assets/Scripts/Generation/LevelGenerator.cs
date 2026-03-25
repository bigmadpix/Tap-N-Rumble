using UnityEngine;
using System.Collections.Generic;
using System;
public class LevelGenerator : MonoBehaviour
{
    public TreeNode<LevelNode> node;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int n = 5;
        TreeGenerator.generateRandomTree(n);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class TreeGenerator
{
    public static void printTreeEdges(int[] prufer, int m)
    {
        int vertices = m + 2;
        int[] vertex_set = new int[vertices];

        // Initialize the array of vertices
        for (int i = 0; i < vertices; i++)
            vertex_set[i] = 0;

        // Number of occurrences of vertex in code
        for (int i = 0; i < vertices - 2; i++)
            vertex_set[prufer[i] - 1] += 1;

        Debug.Log("\nThe edge set E(G) is :\n");

        // Find the smallest label not present in
        // prufer[].
        int j = 0;
        for (int i = 0; i < vertices - 2; i++)
        {
            for (j = 0; j < vertices; j++)
            {
                // If j+1 is not present in prufer set
                if (vertex_set[j] == 0)
                {
                    // Remove from Prufer set and print
                    // pair.
                    vertex_set[j] = -1;
                    Debug.Log("(" + (j + 1) + ", "
                                  + prufer[i] + ") ");

                    vertex_set[prufer[i] - 1]--;

                    break;
                }
            }
        }

        j = 0;
        // For the last element
        for (int i = 0; i < vertices; i++)
        {
            if (vertex_set[i] == 0 && j == 0)
            {
                Debug.Log("(" + (i + 1) + ", ");
                j++;
            }
            else if (vertex_set[i] == 0 && j == 1)
                Debug.Log((i + 1) + ")\n");
        }
    }

    // Function to Generate Random Tree
    public static void generateRandomTree(int n)
    {

        System.Random rand = new System.Random();
        int length = n - 2;
        int[] arr = new int[length];

        // Loop to Generate Random Array
        for (int i = 0; i < length; i++)
        {
            arr[i] = rand.Next(length + 1) + 1;
        }
        printTreeEdges(arr, length);
    }
}
public class LevelNode
{
    public int level;
}
public class TreeNode<T>
{
    public T Data;
    public List<TreeNode<T>> Children;
    public TreeNode(T data) => Data = data;

    public void AddChild(T data) => Children.Add(new TreeNode<T>(data));

}