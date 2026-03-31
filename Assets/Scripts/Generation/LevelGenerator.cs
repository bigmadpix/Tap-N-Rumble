using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch; // Required for Touch.activeTouches
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
[Serializable]
public class LevelGenerator : MonoBehaviour
{
    [Header("UI Settings")]
    public Canvas worldCanvas;
    public GameObject uiMarkerPrefab;

    public GameObject nodePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Recursive Print Method
    void PrintTree(Node node, string indent)
    {
        Debug.Log(indent + "+-- " + node.name);
        foreach (var child in node.children)
        {
            PrintTree(child, indent + "    "); // Recursion
        }
    }
    [Header("State")]
    public Transform currentNode;
    private Color originalColor;
    private Renderer currentRenderer;
    public Color highlightColor = Color.yellow;

    [Header("Generator Settings")]
    public int maxDepth = 3;
    public int maxChildren = 3;
    private int nodeCounter = 0;

    public float branchLength = 2.0f;
    [Range(0, 90)] public float maxSpreadAngle = 30f;
    public Material lineMaterial;
    public float lineWidth = 0.1f;

    public Transform finalTarget;
    void Start()
    {
        if (nodePrefab == null)
        {
            Debug.LogError("Please assign a Node Prefab in the Inspector!");
            return;
        }
        // 1. Generate the tree starting from depth 0
        Node root = GenerateTree(0);

        // 2. Print the generated tree recursively
        PrintTree(root, "");

        GenerateNode(transform.position, transform.rotation, 0, transform, null);
    }

    // --- Generator Logic ---
    void GenerateNode(Vector3 position, Quaternion rotation, int depth, Transform parentTransform, GameObject parentObj)
    {
        // 1. Instantiate the node
        GameObject instance = Instantiate(nodePrefab, position, rotation, parentTransform);
        if (uiMarkerPrefab != null && worldCanvas != null)
        {
            // Place the UI marker at the node's position
            GameObject marker = Instantiate(uiMarkerPrefab, position, Quaternion.identity, worldCanvas.transform);
            marker.name = $"UI_Marker_{depth}";

            // Optional: If you want to click the UI instead of the 3D mesh, 
            // ensure the UI Image has "Raycast Target" enabled.
        }
        // 2. Add LineRenderer if this node has a parent to connect to
        if (parentObj != null)
        {
            LineRenderer lr = instance.AddComponent<LineRenderer>();
            lr.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 2;

            // Set line start (this node) and end (parent node)
            lr.SetPosition(0, instance.transform.position);
            lr.SetPosition(1, parentObj.transform.position);

            // Optional: Make line static in world space
            lr.useWorldSpace = true;
        }

        // 3. Base Case
        // 2. Base Case: If at the end, connect to the FINAL TARGET
        if (depth >= maxDepth)
        {
            //AddLine(instance, finalTarget.position);
            return;
        }

        // 4. Recursive branching
        int numChildren = UnityEngine.Random.Range(1, maxChildren + 1);
        for (int i = 0; i < numChildren; i++)
        {
            Quaternion randomSpread = Quaternion.Euler(
                UnityEngine.Random.Range(-maxSpreadAngle, maxSpreadAngle),
                UnityEngine.Random.Range(0, 360),
                0
            );
            Quaternion childRotation = rotation * randomSpread;
            Vector3 childPosition = position + (childRotation * Vector3.up * branchLength);

            // Pass the current 'instance' as the parent for the next node
            GenerateNode(childPosition, childRotation, depth + 1, instance.transform, instance);
        }
    }
    void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                HandleTouch(touch.screenPosition);
            }
        }
    }
    void HandleTouch(Vector2 touchPosition)
    {
        Debug.Log("HANDLE TOUCH");
        // Create a ray from the touch position
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform clickedTransform = hit.transform;

            // Navigation Logic: Only move to direct neighbors
            bool isChild = clickedTransform.parent == currentNode;
            bool isParent = currentNode.parent == clickedTransform;
            bool isFinalTarget = (clickedTransform == finalTarget && currentNode.childCount == 0);

            if (isChild || isParent || isFinalTarget)
            {
                SelectNode(clickedTransform);
            }
        }
    }
    void SelectNode(Transform newNode)
    {
        if (currentRenderer != null)
            currentRenderer.material.color = originalColor;

        currentNode = newNode;
        currentRenderer = currentNode.GetComponent<Renderer>();

        if (currentRenderer != null)
        {
            originalColor = currentRenderer.material.color;
            currentRenderer.material.color = highlightColor;
        }
    }
    void AddLine(GameObject fromObj, Vector3 toPosition)
    {
        LineRenderer lr = fromObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial != null ? lineMaterial : new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, fromObj.transform.position);
        lr.SetPosition(1, toPosition);
        lr.useWorldSpace = true;
    }
    Node GenerateTree(int currentDepth)
    {
        Node node = new Node("Node_" + nodeCounter++);

        // Only add children if we haven't hit the max depth
        if (currentDepth < maxDepth)
        {
            int numChildren = UnityEngine.Random.Range(1, maxChildren + 1);
            for (int i = 0; i < numChildren; i++)
            {
                node.children.Add(GenerateTree(currentDepth + 1));
            }
        }
        return node;
    }

[Serializable]
public class Node
{
    public string name;
    public int depth = 0;
    public List<Node> children = new List<Node>();

    public Node(string name) { 
            this.name = name+ ":"; 
        }
}

}