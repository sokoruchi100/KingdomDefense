using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower towerPrefab;

    [SerializeField] private bool isPlaceable;

    [SerializeField] private GameObject selected;
    [SerializeField] private GameObject redSelected;
    public bool IsPlaceable { get { return isPlaceable; } }

    private GridManager gridManager;
    private Pathfinder pathfinder;
    private Vector2Int coordinates = new Vector2Int();

    private void Awake() {
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    private void Start() {
        if (gridManager != null) {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable) {
                gridManager.BlockNode(coordinates);
            }
        }
    }

    private void OnMouseDown() {
        if (gridManager.GetNode(coordinates).isWalkable && !pathfinder.WillBlockPath(coordinates)) {
            bool isSuccessful = towerPrefab.TryCreateTower(towerPrefab, transform.position);
            isPlaceable = !isSuccessful;
            if (isSuccessful) {
                gridManager.BlockNode(coordinates);
                pathfinder.NotifyReceivers();
                if (selected.activeInHierarchy) {
                    selected.SetActive(false);
                }
            }
        }
    }

    private void OnMouseOver() {
        if (IsPlaceable && !pathfinder.WillBlockPath(coordinates)) {
            if (towerPrefab.CanCreateTower()) {
                ActivateSelected();
                DeactivateRedSelected();
            } else {
                ActivateRedSelected();
                DeactivateSelected();
            }
        }
    }

    private void OnMouseExit() {
        DeactivateRedSelected();
        DeactivateSelected();
    }

    private void ActivateSelected() {
        if (!selected.activeInHierarchy) {
            selected.SetActive(true);
        }
    }

    private void DeactivateSelected() {
        if (selected.activeInHierarchy) {
            selected.SetActive(false);
        }
    }

    private void ActivateRedSelected() {
        if (!redSelected.activeInHierarchy) {
            redSelected.SetActive(true);
        }
    }

    private void DeactivateRedSelected() {
        if (redSelected.activeInHierarchy) {
            redSelected.SetActive(false);
        }
    }
}