using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[System.Serializable]
public class WeightedItem
{
    public LevelNode.NodeType type;
    public int weight;
}
public class LevelGen2D : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject nodePrefab; // A UI Button
    public GameObject linePrefab; // A UI Image (White Square)
    public RectTransform canvasRoot;

    [Header("Generation Settings")]
    public int maxDepth = 3;
    public float branchLength = 200f;
    [Range(20, 160)] public float totalSpread = 120f;

    [Header("Visuals")]
    public Color activeColor = Color.cyan;
    public Color defaultColor = Color.white;

    public GameObject currentNode; 
    private GameObject finalShop;
    private GameObject finalNode;
    private List<GameObject> leafNodes = new List<GameObject>();

    public List<WeightedItem> lootTable;
    void Start()
    {
        if (nodePrefab == null || linePrefab == null || canvasRoot == null)
        {
            Debug.LogError("Missing Prefab assignments in the Inspector!");
            return;
        }

        // Generate the tree starting from the Root
        // Start at (0, -400) and 0 degrees => Tree Direction is Bottom to the Top
        GenerateNode(new Vector2(0, -400), 0f, totalSpread, 0, canvasRoot.gameObject);

        //Create the last target (boss)
        CreateFinalShop();
        CreateFinalTarget();

        // Select the Root to start the level
        // The root is the second from last child of the canvasRoot ig
        if (canvasRoot.childCount > 0)
            SelectNode(canvasRoot.GetChild(canvasRoot.childCount-3).gameObject);

        canvasRoot.GetChild(canvasRoot.childCount - 3).gameObject.GetComponent<LevelNode>().nodeType = LevelNode.NodeType.Start;
    }

    private void Update()
    {
        LevelManager.instance.currentNode = currentNode.GetComponent<LevelNode>();
    }


 

        public LevelNode.NodeType GetWeightedRandomItem()
        {
            int totalWeight = 0;
            foreach (var item in lootTable)
            {
                totalWeight += item.weight;
            }

            int randomPoint = Random.Range(0, totalWeight);

            foreach (var item in lootTable)
            {
                if (randomPoint < item.weight)
                {
                    return item.type;
                }
                randomPoint -= item.weight;
            }
            return LevelNode.NodeType.Combat;
        }
    
    void GenerateNode(Vector2 localPos, float currentAngle, float availableSpread, int depth, GameObject parentObj)
    {
        // Instantiate as a child of the parentObj to create a real hierarchy
        GameObject instance = Instantiate(nodePrefab, parentObj.transform);
        instance.name = $"Level_{depth}_Node";

        RectTransform rect = instance.GetComponent<RectTransform>();
        rect.localPosition = localPos;
        /*
        if(depth == (maxDepth))
        {
            instance.GetComponent<LevelNode>().nodeType = LevelNode.NodeType.Rest;
        }
        else
        {
            instance.GetComponent<LevelNode>().nodeType = LevelNode.NodeType.Combat;
        }
        */

        var rand = Random.Range(1,4);
        instance.GetComponent<LevelNode>().nodeType = GetWeightedRandomItem();
        // Add the click listener
        Button btn = instance.GetComponent<Button>();
        btn.onClick.AddListener(() => OnNodeClicked(instance));

        // Draw line back to parent (if not the root)
        if (depth > 0)
        {
            DrawLine(instance, parentObj);
        }

        // Base Case
        if (depth >= maxDepth)
        {
            leafNodes.Add(instance);
            return;
        }

        // Non-Overlapping Math
        int numChildren = Random.Range(2, 4);
        float angleStep = availableSpread / (numChildren + 1);
        float startAngle = currentAngle - (availableSpread / 2f);

        for (int i = 1; i <= numChildren; i++)
        {
            float childAngle = startAngle + (angleStep * i);
            Vector2 dir = new Vector2(Mathf.Sin(childAngle * Mathf.Deg2Rad), Mathf.Cos(childAngle * Mathf.Deg2Rad));

            GenerateNode(dir * branchLength, childAngle, availableSpread / numChildren, depth + 1, instance);
        }
    }
    void CreateFinalShop()
    {
        // Place the final node at the very top center
        finalShop = Instantiate(nodePrefab, canvasRoot);
        finalShop.name = "FINAL_SHOP";
        finalShop.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 282 * maxDepth);
        finalShop.GetComponent<Image>().color = Color.yellow; // Make it special

        finalShop.GetComponent<LevelNode>().nodeType = LevelNode.NodeType.Shop;

        Button btn = finalShop.GetComponent<Button>();
        btn.onClick.AddListener(() => OnNodeClicked(finalShop));

        // Connect all leaf nodes to this one final target
        foreach (GameObject leaf in leafNodes)
        {
            DrawLine(finalShop, leaf);
        }
    }
    void CreateFinalTarget()
    {
        // Place the final node at the very top center
        finalNode = Instantiate(nodePrefab, canvasRoot);
        finalNode.name = "FINAL_GOAL";
        finalNode.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 282*(maxDepth+1));
        finalNode.GetComponent<Image>().color = Color.red; // Make it special

        finalNode.GetComponent<LevelNode>().nodeType = LevelNode.NodeType.Boss;

        Button btn = finalNode.GetComponent<Button>();
        btn.onClick.AddListener(() => OnNodeClicked(finalNode));
        DrawLine(finalNode, finalShop);

    }
    public void OnNodeClicked(GameObject clickedNode)
    {
        // DOWNWARD ONLY CHECK: 
        // Is the clicked node's parent the one we are currently standing on?

        bool isChild = clickedNode.transform.parent == currentNode.transform;
        bool isFinalGoal = (clickedNode == finalNode && currentNode == finalShop);
        bool isFinalShop = (clickedNode == finalShop && leafNodes.Contains(currentNode));
        if (isChild || isFinalGoal || isFinalShop)
        {
            SelectNode(clickedNode);
            clickedNode.GetComponent<LevelNode>().OnNodeEnter();
        }
        else
        {
            Debug.Log("Invalid move! You can only move to a direct child.");
        }

        // Run the node's code based on Node Type
        
    }

    void SelectNode(GameObject newNode)
    {
        if (currentNode != null)
            currentNode.GetComponent<Image>().color = defaultColor;

        currentNode = newNode;
        currentNode.GetComponent<Image>().color = activeColor;
    }

    void DrawLine(GameObject child, GameObject parent)
    {
        // Parent lines to the Canvas root so they stay in UI space
        GameObject line = Instantiate(linePrefab, canvasRoot);
        line.transform.SetAsFirstSibling(); // Ensure lines stay behind buttons

        RectTransform lineRect = line.GetComponent<RectTransform>();

        // Use world positions to ignore nested parent offsets
        Vector3 startPos = child.transform.position;
        Vector3 endPos = parent.transform.position;

        // Set the line's world position to the midpoint
        lineRect.position = (startPos + endPos) / 2f;

        // Calculate the direction and distance in screen space
        Vector3 diff = endPos - startPos;
        float distance = diff.magnitude/3f;

        lineRect.sizeDelta = new Vector2(distance, 5f);

        // Rotate to face the parent
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        lineRect.rotation = Quaternion.Euler(0, 0, angle);
    }
}