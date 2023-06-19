using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int cost = 100;
    [SerializeField] private float buildDelay = 1f;

    private void Start() {
        StartCoroutine(Build());
    }

    private IEnumerator Build() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
            foreach (Transform grandChild in child) {
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandChild in child) {
                grandChild.gameObject.SetActive(true);
            }
            
        }
    }

    public bool TryCreateTower(Tower towerPrefab, Vector3 position) {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null) {
            return false;
        }

        if (bank.CurrentBalance >= cost) {
            Instantiate(towerPrefab.gameObject, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }

    public bool CanCreateTower() {
        Bank bank = FindObjectOfType<Bank>();

        if (bank == null) {
            return false;
        }

        if (bank.CurrentBalance >= cost) {
            return true;
        }

        return false;
    }
}
