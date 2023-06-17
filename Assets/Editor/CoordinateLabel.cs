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

    private TextMeshPro label;
    private Vector2Int coordinates = new Vector2Int();
    private Waypoint waypoint;

    private void Awake() {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        waypoint = GetComponentInParent<Waypoint>();
        DisplayCoordinates();
    }

    private void Update() {
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
        if (waypoint.IsPlaceable) {
            label.color = defaultColor;
        } else {
            label.color = blockedColor;
        }
    }

    //REMEMBER TO PLACE IN EDITOR FOLDER WHEN BUILDING
    private void DisplayCoordinates() {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        label.text = $"{coordinates.x},{coordinates.y}";
    }

    private void UpdateObjectName() {
        transform.parent.name = coordinates.ToString();
    }
}
