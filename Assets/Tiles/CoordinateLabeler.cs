using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColour;
    [SerializeField] Color restrictedColour = Color.grey;
    [SerializeField] Color exploredColour = Color.yellow;
    [SerializeField] Color pathColour = Color.blue;
    
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();

    GridManager gridManager;

    void Awake() 
    {
        label = GetComponent<TextMeshPro>();
        gridManager = FindObjectOfType<GridManager>();
        DisplayCoordinates();
        defaultColour = label.color;
        label.enabled = false;
    }

    void Update()
    {
        if(!Application.isPlaying)
        {
            label.enabled = true;
            DisplayCoordinates();
            UpdateObjectName();
        }

        SetLabelColour(); // changes coordinate colours depending on placeability
        ToggleLabels(); // toggle visibility of labels on and off
    }

    void DisplayCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        label.text = coordinates.x + ", " + coordinates.y;
    }

    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }

    void SetLabelColour()
    {
        if(gridManager == null) { return; }

        Node node = gridManager.GetNodeFromDictionary(coordinates);

        if(node == null) { return; }

        if (node.walkingRestricted)
        {
            label.color = restrictedColour;
        }

        else if (node.isPath)
        {
            label.color = pathColour;
        }

        else if (node.isExplored)
        {
            label.color = exploredColour;
        }

        else
        {
            label.color = defaultColour;
        }
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.enabled;
        }
    }
}
