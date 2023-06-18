using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] private int poolSize = 5;
    [SerializeField] [Range(0.1f, 30f)] private float spawnTimer = 1f;

    private GameObject[] pool;
    private Pathfinder pathfinder;

    private void Awake() {
        pathfinder = FindObjectOfType<Pathfinder>();
        PopulatePool();
    }

    private void Start() {
        pathfinder.GetNewPath();
        StartCoroutine(SpawnEnemy());
    }

    private void PopulatePool() {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++) {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    private void EnableObjectInPool() {
        for (int i = 0; i < pool.Length; i++) {
            if (!pool[i].activeInHierarchy) {
                pool[i].SetActive(true);
                return;
            }
        }
    }

    private IEnumerator SpawnEnemy() {
        while (true) {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
