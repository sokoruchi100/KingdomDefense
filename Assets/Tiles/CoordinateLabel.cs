using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabel : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color blockedColor = Color.red;
    [SerializeField] private Color exploredColor = Color.yellow;
    [SerializeField] private Color pathColor = Color.magenta;

    private TextMeshPro label;
    private Vector2Int coordinates = new Vector2Int();
    private GridManager gridManager;

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        DisplayCoordinates();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

        if (!Application.isPlaying) {
            DisplayCoordinates();
            UpdateObjectName();
        }

        SetLabelColor();
        ToggleLabels();
    }

    private void ToggleLabels() {
        if (Input.GetKeyDown(KeyCode.C)) {
            label.enabled = !label.IsActive();
        }
    }

    private void SetLabelColor() {
        if (gridManager == null) return;

        Node node = gridManager.GetNode(coordinates);

        if (node == null) return;

        if (!node.isWalkable) {
            label.color = blockedColor;
        } else if (node.isPath) {
            label.color = pathColor;
        } else if (node.isExplored) {
            label.color = exploredColor;
        } else {
            label.color = defaultColor;
        }
    }

    private void DisplayCoordinates() {
        if (gridManager == null) return;

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);

        label.text = $"{coordinates.x},{coordinates.y}";
    }

    private void UpdateObjectName() {
        transform.parent.name = coordinates.ToString();
    }
}
